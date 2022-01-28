using Bookstore.Domain.Abstractions.Repository;
using Bookstore.Domain.Entities;
using Bookstore.Infra.Data.Orm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBookstore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookstoreController : ControllerBase
    {
        private readonly TodoContext _context;

        private readonly IRepositoryProducts _repoProducts;

        public BookstoreController(TodoContext context, 
                                   IRepositoryProducts repoProducts)
        {
            _context = context;

            var listaProduct = new List<Product> {
                new Product{ Id = "1", Name = "Book1", Price = 26, Quantity = 1, Category = "action", Img = "Img1" },
                new Product{ Id = "2", Name = "Book2", Price = 52, Quantity = 1, Category = "action", Img = "Img1" },
                new Product{ Id = "3", Name = "Book3", Price = 20, Quantity = 2, Category = "action", Img = "Img1" },
                new Product{ Id = "4", Name = "Book4", Price = 10, Quantity = 1, Category = "action", Img = "Img1" },
                new Product{ Id = "5", Name = "Book5", Price = 15, Quantity = 5, Category = "action", Img = "Img1" }
            };

            _context.TodoProducts.AddRange(listaProduct);

            _context.SaveChanges();
            _repoProducts = repoProducts;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //_context.TodoProducts.Add(product);
            //await _context.SaveChangesAsync();

            if (!ModelState.IsValid) return BadRequest("A Model está inválida!");

            try
            {
                await _repoProducts.Add(product);
                await _repoProducts.Commit();

                return CreatedAtAction(nameof(GetProdut), new { id = product.Id }, product);
            }
            catch (System.Exception)
            {
                /* faça algo... 
                 * avise a alguém 
                 * ou não faça nada...
                 */
                await _repoProducts.Rollback();
                return BadRequest("Erro ao tentar adicionar Produto");
            }

        }

        // GET: api/
        [HttpGet]
        public async Task<IEnumerable<Product>> GetTodoItems()
        {
            //return await _context.TodoProducts.ToListAsync();
            return await _repoProducts.GetAll();
        }


        // GET: api/bookstore/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProdut(int id)
        {
            //var todoItem = await _context.TodoProducts.FindAsync(id.ToString());
            var todoItem = await _repoProducts.GetById(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

    }
}
