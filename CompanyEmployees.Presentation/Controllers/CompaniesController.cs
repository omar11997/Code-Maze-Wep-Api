using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;


namespace CompanyEmployees.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]

    //[ResponseCache(CacheProfileName = "120SecondsDuration")]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CompaniesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet(Name = "GetCompanies")]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> GetAllCompanies()
        {
            //throw new Exception("Exception");

            var companies =await _serviceManager.CompanyService.GetAllCompanies(trackChanges: false);
            

            return Ok(companies);



        }
        //[HttpGet("{id:guid}")]
        [HttpGet("{id:guid}", Name = "CompanyById")]
        //[ResponseCache(Duration = 60)]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge =60)]
        [HttpCacheValidation(MustRevalidate =false)]
        
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var compamy = await _serviceManager.CompanyService.GetCompany(id, trackChanges: true);
            return Ok(compamy);
        }
        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto company)
        {
            //if (company is null)
            //    return BadRequest("CompanyForCreationDto object is null");

            var createdCompany = await _serviceManager.CompanyService.CreateCompany(company);

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
        }
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var companies =await _serviceManager.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody]IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result =await _serviceManager.CompanyService.CreateCompanyCollection(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { result.ids },result.companies);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
           await _serviceManager.CompanyService.DeleteCompany(id, trackChanges: false);
            return NoContent();
        }
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id , [FromBody]CompanyForUpdateDto company)
        {
            //if (company == null)
            //{
            //    return BadRequest("CompanyForUpdateDto object is null");
            //}
           await _serviceManager.CompanyService.UpdateCompany(id,company,trackChanges:true);

            return NoContent();
        }
        [HttpOptions]
        public async Task<IActionResult> GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST,DELETE,DISPATCH");
            return Ok();
        }
    }
}
