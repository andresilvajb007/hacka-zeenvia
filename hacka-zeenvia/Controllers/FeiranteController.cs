using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hacka_zeenvia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace hacka_zeenvia.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FeiranteController : ControllerBase
    {

        private readonly Context _context;        

        private readonly ILogger<Feirante> _logger;

        public FeiranteController(ILogger<Feirante> logger,
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
        public IActionResult Incluir([FromBody] Feirante model)
        {

            var json = JsonConvert.SerializeObject(model);
            _logger.LogInformation($"Acessando POST {nameof(FeiranteController)} {nameof(model)}: {json}");         

            _context.Feirante.Add(model);
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
            _logger.LogInformation($"Acessando DELETE Feirante/{id}");

            var Feirante = _context.Feirante.Find( id);

            if (Feirante == null)
            {
                return NotFound();
            }

            _context.Feirante.Remove(Feirante);
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult Get([FromQuery] string nome,string nomeProduto,int? feiranteId, int? produtoId)
        {
            _logger.LogInformation($"Acessando GET  Feirante {nameof(nome)}: {nome} , {nameof(feiranteId)}: {feiranteId}");

            var feirantes = _context.Feirante
                                      .Include(x => x.FeiranteProdutos)
                                      .ThenInclude(x => x.Produto)
                                      .Where(x => (feiranteId == null || x.FeiranteId == feiranteId) &&
                                                  (produtoId == null || x.FeiranteProdutos.Where(x => x.ProdutoId == produtoId).Any()) &&
                                                  (string.IsNullOrEmpty(nomeProduto) || x.FeiranteProdutos.Where(x => x.Produto.Nome.ToLower().Contains(nomeProduto.ToLower())).Any()) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).Select(x => new { x.FeiranteId, x.Nome, x.Celular }).ToList();
                                            

            if (feirantes == null || feirantes.Count() == 0)
            {
                return NotFound();
            }

            
            return Ok(feirantes);
        }

        [HttpGet("menu-feirantes")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MenuFeirantes([FromQuery] string nome,string nomeProduto, int? feiranteId, int? produtoId)
        {
            _logger.LogInformation($"Acessando GET  Feirante {nameof(nome)}: {nome} , {nameof(feiranteId)}: {feiranteId}");

            var feirantes = _context.Feirante
                                      .Include(x=>x.FeiranteProdutos)
                                      .ThenInclude(x=>x.Produto)
                                      .Where(x => (feiranteId == null || x.FeiranteId == feiranteId) &&
                                                  (produtoId == null || x.FeiranteProdutos.Where(x => x.ProdutoId == produtoId).Any()) &&
                                                  (string.IsNullOrEmpty(nomeProduto) || x.FeiranteProdutos.Where(x => x.Produto.Nome.ToLower().Contains(nomeProduto.ToLower())).Any()) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (feirantes == null || feirantes.Count() == 0)
            {
                return NotFound();
            }

            StringBuilder builder = new StringBuilder();
            feirantes.ForEach(x => builder.AppendLine($"{x.FeiranteId} - {x.Nome} - {x.Celular}"));
            

            return Ok(builder.ToString());
        }


    }
}
