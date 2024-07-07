using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            
        }

        public async Task<CompanyDto> CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompanies(bool trackChanges)
        {
            
                var companies=await _repository.Company.GetAllCompanies(trackChanges);

            //var companiesDto = companies.Select(e => new CompanyDto(e.Id, e.Name ?? "", string.Join(" ", e.Address, e.Country))).ToList();
            var companiesDto =  companies.Select(e => new CompanyDto { Id = e.Id, Name = e.Name, FullAddress = string.Join(" ", e.Address, e.Country) }).ToList();

                //var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
                
                return companiesDto;
           
        }

        public async Task<IEnumerable<CompanyDto>> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids == null)
                throw new IdParametersBadRequestException();

            var companyEntities =await _repository.Company.GetByIds(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }

        public async Task<CompanyDto> GetCompany(Guid id, bool trackChanges)
        {
            //var company =await _repository.Company.GetCompany(id, trackChanges);
            //if (company == null)
            //    throw new CompanyNotFoundException(id);
            var company = await GetCompanyAndCheckIfItExists(id, trackChanges);
            var companyDto = _mapper.Map<CompanyDto>(company);

            return companyDto;  
        }
        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
           await _repository.Save();
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);

        }

        public async Task DeleteCompany(Guid comapnyId, bool trackChanges)
        {
            //var company =await _repository.Company.GetCompany(comapnyId, trackChanges);
            //if (company == null)
            //    throw new CompanyNotFoundException(comapnyId);
            var company = await GetCompanyAndCheckIfItExists(comapnyId, trackChanges);
            _repository.Company.DeleteCompany(company);
           await _repository.Save();
        }

        public async Task UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            //var companyEntity = await _repository.Company.GetCompany(companyId, trackChanges);
            //if(companyEntity == null)
            //    throw new CompanyNotFoundException(companyId);
            var companyEntity = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

            _mapper.Map(companyForUpdate, companyEntity);
           await _repository.Save();
        }
        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id ,bool trackChanges)
        {
            var company = await _repository.Company.GetCompany(id, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(id);
            return company;
        }

        
    }
}
