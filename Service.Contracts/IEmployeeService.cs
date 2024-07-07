using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId, LinkParameters linkParameters, bool trackChanges);
        Task<EmployeeDto> GetEmployee(Guid companyId, Guid id, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges);
        Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);

        Task UpdateEmployeeForCompany(Guid companyId,Guid id, EmployeeForUpdateDto employeeForUpdate, bool trackChanges,bool empTrackChanges);
        Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid id, bool trackChanges, bool empTrackChanges);

        void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity);

    }
}
