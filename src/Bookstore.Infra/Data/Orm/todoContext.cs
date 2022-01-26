using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infra.Data.Orm
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> option)
             : base(option)
        {
        }

        public DbSet<Product> TodoProducts { get; set; }
    }
}
