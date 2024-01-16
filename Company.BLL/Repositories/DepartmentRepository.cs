using Company.BLL.Interfaces;
using Company.DAL.Context;
using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext dbContext) : base(dbContext)  // unit of work will inject context
        {

        }

        public IQueryable<Department> SearchDepartmentByName(string name)
                    => _dbContext.Departments.Where(D => D.Name.ToLower().Contains(name.ToLower()));
    }
}
