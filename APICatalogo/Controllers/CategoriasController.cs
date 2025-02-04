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
        private readonly IRepository<Categoria> _repository;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IRepository<Categoria> repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetAll();
            return Ok(categorias);

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _repository.Get(p => p.CategoriaId == id);
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


            var CategoriaCriada = _repository.Create(categoria);
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
            _repository.Update(categoria);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
           
            var categoria = _repository.Get(p => p.CategoriaId == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }
            _repository.Delete(categoria);
            return Ok();

        }
    }
}
