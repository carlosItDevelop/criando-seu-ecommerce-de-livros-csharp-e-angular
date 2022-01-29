using Bookstore.Domain.Abstractions.Repository.Base;
using Bookstore.Infra.Data.Orm;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bookstore.Infra.Repository.Base
{
    public abstract class GenericRepository<T, Key> : IGenericRepository<T, Key> where T : class, new()
    {
        protected ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context) => _context = context;

        public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return await _context.Set<T>().AsNoTracking().ToListAsync();
            
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual async Task<T> GetById(Key id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public virtual async Task Add(T obj)
        {
            _context.Set<T>().Add(obj);
            await Task.CompletedTask;
        }
        public virtual async Task Update(T obj)
        {
            _context.Set<T>().Update(obj);
            await Task.CompletedTask;
        }
        public virtual async Task Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
            await Task.CompletedTask;
        }
        public void Dispose()
        {
            _ = (_context?.DisposeAsync());
        }
    }
}
