using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Model;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork uof)
        {

            _logger = logger;
            _uof = uof;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(p => p.CategoriaId == id);
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

            if (categoria is null)
            {

                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos.");
            }


            var CategoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();
            return new CreatedAtRouteResult("ObterCategoria",
                new { id = CategoriaCriada.CategoriaId }, CategoriaCriada);


        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Categoria com id={id} não corresponde ao corpo da requisição...");
                return BadRequest("Categoria com id não corresponde ao corpo da requisição.");
            }
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
           
            var categoria = _uof.CategoriaRepository.Get(p => p.CategoriaId == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }
            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return Ok();

        }
    }
}
