using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            if (categorias == null)
                return NotFound("Não foi possível retornar as categorias...");
           
           /* var categoriasDTO = new List<CategoriaDTO>();
            foreach (var categoria in categorias)
            {
                var categoriaDTO = new CategoriaDTO
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };
                categoriasDTO.Add(categoriaDTO);
            }*/
            var categoriasDTO = categorias.ToCategoriaDTOList();
            return Ok(categoriasDTO);

        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(p => p.CategoriaId == id);
            

                if (categoria == null)
                {
                    _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                    return NotFound($"Categoria com id= {id} não encontrada...");
                }

            /*   var categoriaDTO = new CategoriaDTO()
               {
                   CategoriaId = categoria.CategoriaId,
                   Nome = categoria.Nome,
                   ImagemUrl = categoria.ImagemUrl
               };*/
            var categoriaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaDTO);
          
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
        {

            if (categoriaDTO is null)
            {

                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos.");
            }

            /* var categoria = new Categoria()
             {
                 CategoriaId = categoriaDTO.CategoriaId,
                 Nome = categoriaDTO.Nome,
                 ImagemUrl = categoriaDTO.ImagemUrl
             };*/
            var categoria = categoriaDTO.ToCategoria();

            var CategoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            /*var NovacategoriaDTO = new CategoriaDTO()
            {
                CategoriaId = CategoriaCriada.CategoriaId,
                Nome = CategoriaCriada.Nome,
                ImagemUrl = CategoriaCriada.ImagemUrl
            };*/
            var NovacategoriaDTO = CategoriaCriada.ToCategoriaDTO();


            return new CreatedAtRouteResult("ObterCategoria",
                new { id = NovacategoriaDTO.CategoriaId }, NovacategoriaDTO);


        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO) // recebe DTO
        {
            if (id != categoriaDTO.CategoriaId)  //Validação
            {
                _logger.LogWarning($"Categoria com id={id} não corresponde ao corpo da requisição...");
                return BadRequest("Categoria com id não corresponde ao corpo da requisição.");
            }
            /* var categoria = new Categoria() //converção DTO para Model
             {
                 CategoriaId = categoriaDTO.CategoriaId,
                 Nome = categoriaDTO.Nome,
                 ImagemUrl = categoriaDTO.ImagemUrl
             };*/

            var categoria = categoriaDTO.ToCategoria(); //converção DTO para Model

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria); //Atualiza a categoria
            _uof.Commit();

            /* var categoriaAtualizadaDTO = new CategoriaDTO() //Converte Model para DTO
             {
                 CategoriaId = categoriaAtualizada.CategoriaId,
                 Nome = categoriaAtualizada.Nome,
                 ImagemUrl = categoriaAtualizada.ImagemUrl
             };*/

            var categoriaAtualizadaDTO = categoriaAtualizada.ToCategoriaDTO(); //Converte Model para DTO


            return Ok(categoriaAtualizadaDTO); //Retorna a categoria atualizada 
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
           
            var categoria = _uof.CategoriaRepository.Get(p => p.CategoriaId == id);
            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }
            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            /* var categoriaExcluidaDTO = new CategoriaDTO() //Converte Model para DTO
             {
                 CategoriaId = categoriaExcluida.CategoriaId,
                 Nome = categoriaExcluida.Nome,
                 ImagemUrl = categoriaExcluida.ImagemUrl
             };*/
            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO(); //Converte Model para DTO

            return Ok(categoriaExcluidaDTO);

        }
    }
}
