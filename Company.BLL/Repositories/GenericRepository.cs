using Company.BLL.Interfaces;
using Company.DAL.Context;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected readonly CompanyDbContext _dbContext;

        public GenericRepository(CompanyDbContext dbContext) // repo which inhirate from GenericRepository in his ctor will inject context to generic repo
        {
            _dbContext = dbContext;
        }

        public async Task Add(T item)
            => await _dbContext.Set<T>().AddAsync(item);


        public void Delete(T item)
            => _dbContext.Set<T>().Remove(item);


        public async Task<T> GetByID(int id)
            => await _dbContext.Set<T>().FindAsync(id); // use find to search local if not found then search remot


        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Department).ToListAsync();
            else
                return await _dbContext.Set<T>().ToListAsync();
        }

        public void Update(T item)
            => _dbContext.Set<T>().Update(item);

    }
}
