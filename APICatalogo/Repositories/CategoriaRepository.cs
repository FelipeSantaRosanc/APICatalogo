using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>,  ICategoriaRepository 
    {

        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {

        }

    }
}
