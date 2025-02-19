using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Z.PagedList;



namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>,  ICategoriaRepository 
    {

        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParams)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

            /*var resultado = IPagedList<Categoria>.ToPagedList(categoriasOrdenadas,
                categoriaParams.PageNumber,
                categoriaParams.PageSize);*/

            var categoriasFiltradas = await categoriasOrdenadas.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);

            return categoriasFiltradas;

        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriaParams)
        {
            var categoria = await GetAllAsync();
       
            if (!string.IsNullOrEmpty(categoriaParams.Nome))
            {   
                categoria = categoria.Where(c => c.Nome.Contains(categoriaParams.Nome));
            }

            /*var categoriaFiltrada = PagedList<Categoria>.ToPagedList(categoria.AsQueryable(),
                categoriaParams.PageNumber,
                categoriaParams.PageSize);
            */

            var categoriaFiltrada = await categoria.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);

            return categoriaFiltrada;
        }
    }
}
