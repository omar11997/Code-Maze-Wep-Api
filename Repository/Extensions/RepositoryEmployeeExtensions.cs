using Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
        {
            return employees.Where(e => e.Age <= maxAge && e.Age >= minAge);
        }
        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if(string.IsNullOrEmpty(searchTerm))
                return employees;
            return employees.Where(e => e.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }
        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            if(string.IsNullOrEmpty(orderByQueryString))
                return employees.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreatOrderQuery<Employee>(orderByQueryString);


            if (string.IsNullOrEmpty(orderQuery))
                return employees.OrderBy(e => e.Name);

            return employees.OrderBy(orderQuery);
        }

        
    }
}
