using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Service.Contracts;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using CompanyEmployees.Presentation.ActionFilters;
using Shared.RequestFeatures;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Entities.LinkModels;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _services;
        public EmployeesController(IServiceManager services)
        {
            _services = services;
        }
        [HttpHead]
        [HttpGet]
        
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var linkParams = new LinkParameters(employeeParameters, HttpContext);
            var result = await _services.EmployeeService.GetEmployeesAsync(companyId,linkParams, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));
            return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
        }
        //[HttpGet("{id:guid}")]
        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]

        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var employee =await _services.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
            return Ok(employee);


        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            //if (employee == null)
            //    return BadRequest("EmployeeForCreationDto object is null");
            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);
            var employeeToRetrun =await _services.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToRetrun.Id }, employeeToRetrun);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
           await _services.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
            return NoContent();


        }
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            //if (employee == null)
            //    return BadRequest("EmployeeForUpdateDto object is null");

            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);
           await _services.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, trackChanges: false, empTrackChanges: true);

            return NoContent();
        }
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartialUpdateEmployeeForComapny(Guid companyId , Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc )
        {
            if (patchDoc == null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = await _services.EmployeeService.GetEmployeeForPatch(companyId, id, trackChanges: false, empTrackChanges: true);
            patchDoc.ApplyTo(result.employeeToPatch,ModelState);
            TryValidateModel(result.employeeToPatch);
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            _services.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

            return NoContent();
        }
    }

}
