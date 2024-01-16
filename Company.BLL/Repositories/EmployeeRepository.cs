using Company.BLL.Interfaces;
using Company.DAL.Context;
using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Company.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CompanyDbContext dbContext) : base(dbContext)  // unit of work will inject context
        {
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
            => _dbContext.Employees.Where(E => E.Address.ToLower().Contains(address.ToLower()));

        public IQueryable<Employee> SearchEmployeeByName(string name)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()));
    }
}
