using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bookstore.Infra.Data.Orm
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
             : base(option)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Onde não tiver setado varchar e a propriedade for do tipo string fica valendo varchar(valor)
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
            {
                //property.Relational().ColumnType = "varchar(80)"; Até o .Net 2.2
                property.SetColumnType("varchar(80)");
            }


            // Busca os Mapppings de uma vez só
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

    }
}
