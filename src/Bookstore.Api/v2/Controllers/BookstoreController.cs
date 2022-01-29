using APIBookstore.Api.Controllers;
using APIBookstore.Api.Dtos;
using AutoMapper;
using Bookstore.Domain.Abstractions.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBookstore.Api.v2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/bookstore")]
    public class BookstoreController : MainController
    {
        private readonly IRepositoryProducts _repoProducts;
        private readonly IMapper _mapper;

        public BookstoreController(IRepositoryProducts repoProducts,
                                   IMapper mapper)
        {
            _repoProducts = repoProducts;
            _mapper = mapper;
        }

        [HttpGet("obter-todos")]
        public async Task<IEnumerable<ProductDTO>> GetTodoItems()
        {
            //return await _context.TodoProducts.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(await _repoProducts.GetAll());
        }


        [HttpGet("obter-produto/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProdut(int id)
        {
            //var todoItem = await _context.TodoProducts.FindAsync(id.ToString());
            var todoItem = _mapper.Map<ProductDTO>(await _repoProducts.GetById(id));

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

    }
}
