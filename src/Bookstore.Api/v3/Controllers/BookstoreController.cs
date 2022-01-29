using APIBookstore.Api.Controllers;
using APIBookstore.Api.Dtos;
using AutoMapper;
using Bookstore.Domain.Abstractions.Repository;
using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIBookstore.Api.v3.Controllers
{
    [ApiVersion("3.0")]
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


        [HttpPost("adicionar-produto")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> PostProduct(ProductDTO productDto)
        {
            //_context.TodoProducts.Add(product);
            //await _context.SaveChangesAsync();

            if (!ModelState.IsValid) return BadRequest("A Model está inválida!");

            try
            {
                await _repoProducts.Add(_mapper.Map<Product>(productDto));
                await _repoProducts.Commit();

                return CreatedAtAction(nameof(GetProdut), new { id = productDto.Id }, productDto);
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


        /// <summary>
        /// Aqui poderíamos retornar 204 (NoContent), mas para 
        /// efeito de demonstração, utilizei o 200 para exibir
        /// o novo conteúdo já alterado!
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDTO"></param>
        /// <returns></returns>
        [HttpPut("atualizar-produto/{id:int}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            if (id.ToString() != productDTO.Id) return BadRequest("Ids não correspondentes.");

            if(!ModelState.IsValid) return BadRequest("ModelState está inválida");

            var product = _mapper.Map<Product>(productDTO);

            try
            {
                await _repoProducts.Update(product);
                await _repoProducts.Commit();

                return Ok(product);
            }
            catch (System.Exception)
            {
                await _repoProducts.Rollback();
                return BadRequest("Erro ao tentar Atualizar Produto.");
            }
        }

        [HttpDelete("excluir-produto/{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productDTO = await _repoProducts.GetById(id);

            if (productDTO == null) return NotFound();

            try
            {
                await _repoProducts.Delete(_mapper.Map<Product>(productDTO));
                await _repoProducts.Commit();
                return NoContent();
            }
            catch (System.Exception)
            {
                await _repoProducts.Rollback();
                return BadRequest("Erro ao tentar Excluir Produto.");
            }
        }

    }
}
