﻿using System.Security.Cryptography.X509Certificates;
using APICatalogo.Context;
using APICatalogo.Model;
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

        private readonly IProdutoRepository _produtoRepository;
        private readonly IRepository<Produto> _repository;


        public ProdutosController(IRepository<Produto>? repository, IProdutoRepository produtoRepository)
        {
            _repository = repository;
            _produtoRepository = produtoRepository;
        }


        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);
            if (produtos is null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        // /produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
           var produtos = _repository.GetAll();
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
            var produto = _repository.Get(p => p.ProdutoId == id);
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

            var produtoAtualizado = _repository.Update(produto);
            return Ok(produtoAtualizado);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.Get(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound();
            }
           var produtoDeletado = _repository.Delete(produto);
            return Ok(produtoDeletado);

        }



    }
}
