namespace UltimateBook_MiddleWare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Logic before executing the next delegate in the Use method");
                await next.Invoke();
                Console.WriteLine($"Logic after executing the next delegate in the Use method");
            });
            ////https://localhost:7175/usingmapbranch
            app.Map("/usingmapbranch", builder =>
            {
                builder.Use(async (context, next) =>
                {
                    Console.WriteLine("Map branch logic in the Use method before the next delegate");

                    await next.Invoke();

                    Console.WriteLine("Map branch logic in the Use method after the next delegate");

                });
                builder.Run(async context =>
                {
                    Console.WriteLine($"Map branch response to the client in the Run method");
                    await context.Response.WriteAsync("Hello from the map branch.");
                });


            });
            ////https://localhost:7175?testquerystring=test
            app.MapWhen(context => context.Request.Query.ContainsKey("testquerystring"), builder =>
            {
                builder.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello from mapWhen branch");
                });
            });

            //app.Run(async context =>
            //{
            //    Console.WriteLine($"Writing the response to the client in the Run method");
            //    await context.Response.WriteAsync("Hello from the middleware component.");
            //});

            app.MapPost("/", (HttpRequest httpRequest) => Results.Stream(httpRequest.Body));
           
            app.MapControllers();

            app.Run();
        }
    }
}