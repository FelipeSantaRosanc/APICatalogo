using APICatalogo.Models;
using APICatalogo.Pagination;
using Z.PagedList;


namespace APICatalogo.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task <IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParams);
        Task <IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriaParams);

    }
}
