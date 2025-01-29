using System.Security.Cryptography.X509Certificates;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")] //produtos
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly IProdutoRepository _repository;


        public ProdutosController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        // /produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
           var produtos = _repository.GetProdutos().ToList();
            if (produtos is null)
            {
                return NotFound();
            }
              return Ok(produtos);
           
        }


        // /produtos/id
        [HttpGet("{id:int:min(1)}", Name ="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _repository.GetProduto(id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return Ok(produto);
        }

        // /produtos
        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            if(produto is null)
            {
                return BadRequest();
            }

           var novoProduto =  _repository.Create(produto);
            
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
        }


        // /produtos/id
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }
            bool atualizado = _repository.Update(produto);
            if (atualizado)
            {
                return Ok(atualizado);
            }
            else
            {
                return StatusCode(500, $"Falha ao atualizar o produito de id = {id}");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool deletado = _repository.Delete(id);
            if (deletado)
            {
                return Ok($"Produto de id={id} foi excluido");
            }
            else
            {
                return StatusCode(500, "$Falha ao excluir o produto de id = {id}");
            }

        }



    }
}
