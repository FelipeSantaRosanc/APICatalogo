using APICatalogo.Models;
using APICatalogo.Pagination;
using Z.PagedList;

namespace APICatalogo.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutosPorPreco(ProdutosParameters produtosParams);
        Task<IPagedList<Produto>> GetProdutosPorPrecoAsync(ProdutosParameters produtosParams);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPrecoParams);

        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
