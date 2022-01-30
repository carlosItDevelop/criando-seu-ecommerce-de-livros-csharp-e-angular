using Bookstore.Domain.Abstractions.Repository;
using Bookstore.Domain.Entities;
using Bookstore.Infra.Data.Orm;
using Bookstore.Infra.Repository.Base;
using System.Threading.Tasks;

namespace Bookstore.Infra.Repository.Entities
{
    public class RepositoryProducts : GenericRepository<Product, int>, IRepositoryProducts
    {
        public RepositoryProducts(ApplicationDbContext ctx) : base(ctx) => _context = ctx;

        public async Task<bool> Commit() 
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task Rollback()
        {
            /* To do any process... Call other implementation, etc */
            await Task.CompletedTask;
        }
        public async override Task<Product> GetById(int id)
        {
            var idStr = id.ToString();
            return await _context.Set<Product>().FindAsync(idStr);
        }
    }
}
