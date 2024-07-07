using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    //internal class CompanyForUpdateDto
    //{
    //}
    public record CompanyForUpdateDto(string Name, string Address, string Country,IEnumerable<EmployeeForCreationDto> Employees);

}
