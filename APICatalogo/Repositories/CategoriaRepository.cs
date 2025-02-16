using System.Security.Cryptography.X509Certificates;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>,  ICategoriaRepository 
    {

        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParams)
        {
            var categoria = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

            var categoriaOrdenados = PagedList<Categoria>.ToPagedList(categoria,
                categoriaParams.PageNumber,
                categoriaParams.PageSize);

            return categoriaOrdenados;

        }
  
    }
}
