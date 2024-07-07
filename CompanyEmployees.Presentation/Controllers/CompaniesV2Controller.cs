using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiVersion("2.0")]
    //[Route("api/{v:apiversion}/companies")]
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CompaniesV2Controller :ControllerBase
    {
        private readonly IServiceManager _ServiceManger;

        public CompaniesV2Controller(IServiceManager serviceManger)
        {
            _ServiceManger = serviceManger;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _ServiceManger.CompanyService.GetAllCompanies(trackChanges:false);
            var companiesV2 = companies.Select(x => $"{x.Name} V2");
            return Ok(companiesV2);
        }
    }
}
