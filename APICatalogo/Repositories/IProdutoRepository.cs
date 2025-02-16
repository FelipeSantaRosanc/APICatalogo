﻿using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutosPorPreco(ProdutosParameters produtosParams);
        PagedList<Produto> GetProdutosPorPreco(ProdutosParameters produtosParams);
        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPrecoParams);

        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
