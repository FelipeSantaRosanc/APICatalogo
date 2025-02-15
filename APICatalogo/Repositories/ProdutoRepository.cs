using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }
        /* public IEnumerable<Produto> GetProdutosPorPreco(ProdutosParameters produtosParams)
        {
            return GetAll()
                .OrderBy(p => p.Preco)
                .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
                .Take(produtosParams.PageSize).ToList();
        }*/

        public PagedList<Produto> GetProdutosPorPreco(ProdutosParameters produtosParams)
        {
            var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);
            return produtosOrdenados;
        }


        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(p => p.CategoriaId == id);
        }

     
    }
}
