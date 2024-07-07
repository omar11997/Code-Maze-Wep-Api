using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;
namespace Ultmate_Book.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        /// <summary>
        /// this class making like mocking for the dbcontext because our dbcontext class in another project from the project we make migration inside 
        ///  b => b.MigrationsAssembly("Ultmate Book")); =====> this option indicates the assemble name that you want to create the folder of migrations inside it 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json")
                             .Build();

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                            .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                            b => b.MigrationsAssembly("Ultmate Book"));

            return new RepositoryContext(builder.Options);

        }
    }
}
