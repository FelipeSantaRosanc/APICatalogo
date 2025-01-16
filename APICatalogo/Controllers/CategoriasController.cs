using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _contex;

        public CategoriasController(AppDbContext contex)
        {
            _contex = contex;
        }

        [HttpGet("Produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            // return _contex.Categorias.Include(p => p.Produtos).AsNoTracking().ToList(); //incluir produtos da categoria
            return _contex.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5). ToList(); //incluir produtos da categoria e restringir somente a a id's menores que 5
        }



        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {

            return _contex.Categorias.AsNoTracking().ToList();  //AsNoTracking() não rastreia as entidades e melhora a performance somente no Get
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _contex.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrada");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest("Categoria não encontrada");
            }

            _contex.Categorias.Add(categoria);
            _contex.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest($"Categoria com id={id} não encontrada");
            }

            _contex.Entry(categoria).State = EntityState.Modified;
            _contex.SaveChanges();
            return Ok(categoria);
        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _contex.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }
            else
            {
                _contex.Categorias.Remove(categoria);
                _contex.SaveChanges();
                return Ok(categoria);
            }

        }

    }
}
