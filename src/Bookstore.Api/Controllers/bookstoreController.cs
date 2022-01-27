using APIBookstore.Api.Dtos;
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



        public BookstoreController(TodoContext context)
        {
            _context = context;

            _context.TodoProducts.Add(new ProductDTO { Id = "1", Name = "Book1", Price = 24, Quantity = 1, Category = "action", Img = "Img1" });
            _context.TodoProducts.Add(new ProductDTO { Id = "2", Name = "Book2", Price = 50, Quantity = 1, Category = "action", Img = "Img1" });
            _context.TodoProducts.Add(new ProductDTO { Id = "3", Name = "Book3", Price = 20, Quantity = 2, Category = "action", Img = "Img1" });
            _context.TodoProducts.Add(new ProductDTO { Id = "4", Name = "Book4", Price = 10, Quantity = 1, Category = "action", Img = "Img1" });
            _context.TodoProducts.Add(new ProductDTO { Id = "5", Name = "Book5", Price = 15, Quantity = 5, Category = "action", Img = "Img1" });



            _context.SaveChanges();



        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO product)
        {
            _context.TodoProducts.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProdut), new { id = product.Id }, product);
        }

        // GET: api/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetTodoItems()
        {
            return await _context.TodoProducts.ToListAsync();



        }

        // GET: api/bookstore/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProdut(int id)
        {
            var todoItem = await _context.TodoProducts.FindAsync(id.ToString());

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

    }
}
