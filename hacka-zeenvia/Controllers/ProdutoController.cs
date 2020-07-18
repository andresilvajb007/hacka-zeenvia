using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hacka_zeenvia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace hacka_zeenvia.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutoController : ControllerBase
    {

        private readonly Context _context;        

        private readonly ILogger<Produto> _logger;

        public ProdutoController(ILogger<Produto> logger,
                                 Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Incluir([FromBody] Produto model)
        {

            var json = JsonConvert.SerializeObject(model);
            _logger.LogInformation($"Acessando POST {nameof(ProdutoController)} {nameof(model)}: {json}");
          
            _context.Produto.Add(model);
            _context.SaveChanges();


            return Ok();
           

        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Excluir(int id)
        {
            _logger.LogInformation($"Acessando DELETE Produto/{id}");

            var produto =  _context.Produto.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produto.Remove(produto);
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Find(int id)
        {
            _logger.LogInformation($"Acessando Find Produto/{id}");

            var produto = _context.Produto.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult Get([FromQuery] String nome, int? feiranteId, int? produtoId)
        {
            _logger.LogInformation($"Acessando GET  Produto {nameof(nome)}: {nome} , {nameof(feiranteId)}: {feiranteId}, {nameof(produtoId)}: {produtoId}");

            var produtos = _context.Produto
                                      .Where(x => (feiranteId == null || x.FeiranteId == feiranteId) &&
                                                  (produtoId == null || x.ProdutoId == produtoId) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (produtos == null || produtos.Count() == 0)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpGet("produtos-disponiveis")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ProdutosDisponiveis([FromQuery] String nome, int? feiranteId, int? produtoId)
        {
            _logger.LogInformation($"Acessando GET  Produto {nameof(nome)}: {nome} , {nameof(feiranteId)}: {feiranteId}, {nameof(produtoId)}: {produtoId}");

            var produtos = _context.Produto
                                      .Where(x => (feiranteId == null || x.FeiranteId == feiranteId) &&
                                                  (produtoId == null || x.ProdutoId == produtoId) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (produtos == null || produtos.Count() == 0)
            {
                return NotFound();
            }

            StringBuilder stringBuilder = new StringBuilder();
            produtos.ForEach(x => stringBuilder.AppendLine($"{x.ProdutoId} - {x.Nome}"));
            

            return Ok(stringBuilder.ToString());
        }


    }
}
