using Bookstore.Domain.Abstractions.Repository;
using Bookstore.Domain.Entities;
using Bookstore.Infra.Data.Orm;
using Microsoft.AspNetCore.Http;
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
        private readonly IRepositoryProducts _repoProducts;

        public BookstoreController(IRepositoryProducts repoProducts)
        {
            _repoProducts = repoProducts;
        }

        [Route("")]
        [HttpGet("obter-todos")]
        public async Task<IEnumerable<Product>> GetTodoItems()
        {
            //return await _context.TodoProducts.ToListAsync();
            return await _repoProducts.GetAll();
        }


        // GET: api/bookstore/5
        [HttpGet("obter-produto/{id}")]
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


        [HttpPost("adicionar-produto")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    }
}
