﻿using System.Security.Cryptography.X509Certificates;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _contex;

        public ProdutosController(AppDbContext contex)
        {
            _contex = contex;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
           var produtos = _contex.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound("Produtos Não encontrados");
            }
            else
            {
                return produtos;
            }
           
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _contex.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            if(produto is null)
            {
                return BadRequest("Produto não encontrado");
            }

            _contex.Produtos.Add(produto);
            _contex.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest("Produto não encontrado");
            }
            _contex.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _contex.SaveChanges();

            return Ok(produto);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _contex.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            _contex.Produtos.Remove(produto);
            _contex.SaveChanges();
            return Ok(produto);
        }



    }
}
