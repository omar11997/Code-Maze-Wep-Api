using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges);
        Task<CompanyDto> GetCompany(Guid id,bool trackChanges);

        Task<CompanyDto> CreateCompany(CompanyForCreationDto company);
        Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection (IEnumerable<CompanyForCreationDto> companyCollection);

         Task DeleteCompany(Guid comapnyId, bool trackChanges);

        Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate,bool trackChanges);

    }
}
