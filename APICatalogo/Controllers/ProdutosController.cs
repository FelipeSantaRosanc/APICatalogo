﻿using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Z.PagedList;

namespace APICatalogo.Controllers
{
    [Route("[controller]")] //produtos
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
        {
            var metaData = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageCount,
                produtos.PageNumber,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        // /produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _uof.ProdutoRepository.GetAllAsync();
            await _uof.CommitAsync();
            if (produtos is null)
                return NotFound();

            // var destino = _mapper.Map<Destino>(origem);
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtos);

        }

        [HttpGet("produtos/{id}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);
            await _uof.CommitAsync();
            if (produtos is null)
                return NotFound();

            // var destino = _mapper.Map<Destino>(origem);
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        // /produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPrecoAsync(produtosParameters);
            return ObterProdutos(produtos);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco produtosFilterParams)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFilterParams);
            return ObterProdutos(produtos);
        }

        // /produtos
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            await _uof.CommitAsync();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id,
         JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            //valida input 
            if (patchProdutoDto == null || id <= 0)
                return BadRequest();

            //obtem o produto pelo Id
            var produto = await _uof.ProdutoRepository.GetAsync(c => c.ProdutoId == id);

            //se não econtrou retorna
            if (produto == null)
                return NotFound();

            //mapeia produto para ProdutoDTOUpdateRequest
            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            //aplica as alterações definidas no documento JSON Patch ao objeto ProdutoDTOUpdateRequest
            patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
                return BadRequest(ModelState);

            // Mapeia as alterações de volta para a entidade Produto
            _mapper.Map(produtoUpdateRequest, produto);

            // Atualiza a entidade no repositório
            _uof.ProdutoRepository.Update(produto);
            // Salve as alterações no banco de dados
            await _uof.CommitAsync();

            //retorna ProdutoDTOUpdateResponse
            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        // /produtos/id
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDto);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            await _uof.CommitAsync();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoAtualizadoDto);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
            if (produto is null)
                return NotFound();

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            await _uof.CommitAsync();

            var produtoDtoDeletado = _mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDtoDeletado);

        }



    }
}
