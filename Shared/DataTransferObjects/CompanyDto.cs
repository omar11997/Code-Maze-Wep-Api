using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }

        public string? FullAddress { get; init; }

        //public CompanyDto(Guid id, string name, string fullAddress) 
        //{ 
        //    Id = id;
        //    Name = name;
        //    FullAddress = fullAddress;
        
        
        //}
    }
    //[Serializable]
    //public record CompanyDto(Guid Id, string Name, string FullAddress);
}
