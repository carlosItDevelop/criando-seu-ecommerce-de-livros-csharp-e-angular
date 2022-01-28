using Bookstore.Domain.Entities;
using Bookstore.Infra.Data.Orm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIBookstore.Api.Configurations.Extensions
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context =
                new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Buscando qualquer Produto.
                if (context.Products.Any())
                {
                    return;   // Banco de Dados já foi semeado!
                }

                context.Products.AddRange(
                    new Product { Id = "1", Name = "Book1", Price = 26, Quantity = 1, Category = "action", Img = "Img1" },
                    new Product { Id = "2", Name = "Book2", Price = 52, Quantity = 1, Category = "action", Img = "Img1" },
                    new Product { Id = "3", Name = "Book3", Price = 20, Quantity = 2, Category = "action", Img = "Img1" },
                    new Product { Id = "4", Name = "Book4", Price = 10, Quantity = 1, Category = "action", Img = "Img1" },
                    new Product { Id = "5", Name = "Book5", Price = 15, Quantity = 5, Category = "action", Img = "Img1" }
                );
                context.SaveChanges();
            }
        }
    }
}
