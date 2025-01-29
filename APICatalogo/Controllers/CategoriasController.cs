using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            _logger.LogInformation("======= GET Categorias com Produtos ======= ");
            return await _context.Categorias.Include(x => x.Produtos).ToListAsync();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            try
            {
                return await _context.Categorias.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            try
            {

                if (categoria == null)
                {
                    _logger.LogWarning("=======================================");
                    _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                    _logger.LogWarning("=======================================");
                    return NotFound($"Categoria com id= {id} não encontrada...");
                }

                return Ok(categoria);
            }
            catch (Exception)
            {
                _logger.LogError("=======================================================================");
                _logger.LogError($"{StatusCodes.Status500InternalServerError} - Ocorreu um problema ao tratar a sua solicitação");
                _logger.LogError("=======================================================================");

                return StatusCode(StatusCodes.Status500InternalServerError,
                           "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria is null)

                    return BadRequest("Categoria está nula");

                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar criar uma nova categoria");
            }

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"Categoria com id={id} não encontrada...");
                }
                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar atualizar a categoria");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não encontrada...");
                }
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar deletar a categoria");
            }
        }
    }
}
