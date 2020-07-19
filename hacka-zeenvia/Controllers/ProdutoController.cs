using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hacka_zeenvia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult Get([FromQuery] String nome, int? produtoId, int? feiranteId)
        {
            _logger.LogInformation($"Acessando GET  Produto {nameof(nome)}: {nome} , {nameof(feiranteId)}: {feiranteId}, {nameof(produtoId)}: {produtoId}");

            var produtos = _context.Produto
                                      .Where(x => 
                                                  (produtoId == null || x.ProdutoId == produtoId) &&
                                                   (feiranteId == null || x.FeiranteProdutos.Where(x => x.FeiranteId == feiranteId).Any()) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (produtos == null || produtos.Count() == 0)
            {
                return NotFound();
            }

            return Ok(produtos);
        }

        [HttpGet("busca-feirante-produtos")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BuscaFeiranteProdutos([FromQuery]  int? produtoId, int? feiranteId)
        {
            _logger.LogInformation($"Acessando GET  BuscaFeiranteProdutos");

            var produtos = _context.FeiranteProduto
                                      .Include(x=>x.Produto)
                                      .Where(x => (produtoId == null || x.ProdutoId == produtoId) &&
                                                  (feiranteId == null || x.FeiranteId == feiranteId)
                                                  
                                             )
                                      .Select(x => new
                                      {
                                          x.FeiranteId,
                                          x.FeiranteProdutoId,
                                          x.Produto.Nome,
                                          x.Produto.Unidade,
                                          x.Produto.UrlImagem,
                                          x.Preco
                                      }).ToList();


            if (produtos == null || produtos.Count() == 0)
            {
                return NotFound();
            }
            

            return Ok(produtos);
        }

        [HttpGet("menu-produtos")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MenuProduto([FromQuery] String nome, int? produtoId, int? feiranteId)
        {
            _logger.LogInformation($"Acessando GET  Produto {nameof(nome)}: {nome} , {nameof(produtoId)}: {produtoId}");

            var feirantes = _context.Produto
                                    .Include(x=>x.FeiranteProdutos)
                                      .Where(x => (produtoId == null || x.ProdutoId == produtoId) &&
                                                   (feiranteId == null || x.FeiranteProdutos.Where(x=>x.FeiranteId == feiranteId).Any()) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (feirantes == null || feirantes.Count() == 0)
            {
                return NotFound();
            }

            StringBuilder builder = new StringBuilder();
            feirantes.ForEach(x => builder.AppendLine($"{x.ProdutoId} - {x.Nome}"));


            return Ok(builder.ToString());
        }


    }
}
