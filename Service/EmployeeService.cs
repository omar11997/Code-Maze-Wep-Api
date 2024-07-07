
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeLinks _employeeLinks;


        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IEmployeeLinks employeeLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _employeeLinks = employeeLinks;
        }

        public async Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            

            await CheckIfCompanyExists(companyId, trackChanges);

           var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
           await _repository.Employee.CreateEmployeeForCompany(companyId,employeeEntity);
           await _repository.Save();

            var employeeToRetrun = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToRetrun;
        }

        public async Task DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            
            await CheckIfCompanyExists(companyId, trackChanges);

            
            var employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExists(companyId,id,trackChanges);
           await _repository.Employee.DeleteEmployee(employeeForCompany);
           await _repository.Save();
        }

        public async Task<EmployeeDto> GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            
            await CheckIfCompanyExists(companyId, trackChanges);
            

            var employeeFromDB = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);
            var employee = _mapper.Map<EmployeeDto>(employeeFromDB);
            return employee;
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid id, bool trackChanges, bool empTrackChanges)
        {
            

            await CheckIfCompanyExists(companyId, trackChanges);

           
            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch, employeeEntity);

        }

        public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(Guid companyId, LinkParameters linkParameters, bool trackChanges)
        {
            if(!linkParameters.EmployeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            await CheckIfCompanyExists(companyId, trackChanges);

            var employeesWithMetadata = await _repository.Employee.GetEmployeesAsync(companyId, linkParameters.EmployeeParameters, trackChanges);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetadata);
            var links = _employeeLinks.TryGenerateLinks(employeesDto,linkParameters.EmployeeParameters.Fields, companyId, linkParameters.Context);

           

            return (linkResponse: links, metaData: employeesWithMetadata.MetaData);
        }

        public void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            _repository.Save();
        }

        public async Task UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool trackChanges, bool empTrackChanges)
        {
            
            await CheckIfCompanyExists(companyId, trackChanges);

            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId,id, empTrackChanges);
            _mapper.Map(employeeForUpdate, employeeEntity);
           await _repository.Save();
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var comapny = await _repository.Company.GetCompany(companyId, trackChanges);
            if (comapny is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployee(companyId, id, trackChanges);
            if(employeeDb is null)
                throw new EmployeeNotFoundException(companyId);

            return employeeDb;
        }
    }
}
