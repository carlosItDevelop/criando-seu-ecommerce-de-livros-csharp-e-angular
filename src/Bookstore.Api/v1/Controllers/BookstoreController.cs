using APIBookstore.Api.Controllers;
using APIBookstore.Api.Dtos;
using AutoMapper;
using Bookstore.Domain.Abstractions.Repository;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBookstore.Api.v1.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
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


    }
}
