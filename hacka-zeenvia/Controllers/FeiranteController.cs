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
using System.Net.Http;
using hacka_zeenvia.Models.SendMessageZenvia;
using System.Text.RegularExpressions;
using hacka_zeenvia.DTO;
using System.Globalization;

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
            foreach (var feirante in feirantes)
            {
                var celularMask =  long.Parse(feirante.Celular).ToString(@"00 (00) 00000-0000"); // (49) 98807-0405

                builder.AppendLine($"{feirante.FeiranteId} - {feirante.Nome}");
                builder.AppendLine($"{celularMask}");
                builder.AppendLine($"https://feirante/{feirante.FeiranteId}");
                builder.AppendLine(string.Empty);
            }
            
            

            return Ok(builder.ToString());
        }

        [HttpPost("enviar-pedido-zap")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EnviarPedido([FromBody] PedidoDTO pedidoDTO)
        {
            _logger.LogInformation($"Acessando GET  EnviarPedido ");

            var pedido = pedidoDTO.ToPedido();
            var feiranteProdutoIds = pedidoDTO.ItensPedidoDTO.Select(x => x.FeiranteProdutoId).ToList();

            var cliente = _context.Cliente.Find(pedido.ClienteId);
            var feirante = _context.Feirante.Find(pedido.FeiranteId);
            var feiranteProdutos = _context.FeiranteProduto
                                           .Include(x=>x.Produto)
                                           .Where(x => feiranteProdutoIds.Contains(x.FeiranteProdutoId))
                                           .ToList();


            _context.Pedido.Add(pedido);
            _context.SaveChanges();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Olá {feirante.Nome}");
            builder.AppendLine($"{cliente.Nome} tem interesse nos produtos:");
            builder.AppendLine(string.Empty);

            foreach (var feiranteProduto in feiranteProdutos)
            {
                builder.AppendLine($"{feiranteProduto.Produto.Nome}, QTD:{feiranteProduto.ProdutoId}");
            }

            var celularClienteMask = long.Parse(cliente.Celular).ToString(@"00 (00) 00000-0000");
            var valorTotalFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", pedido.Total);

            builder.AppendLine(string.Empty);
            builder.AppendLine($"Total do pedido: {valorTotalFormatado}");
            builder.AppendLine(string.Empty);            
            builder.AppendLine($"Entre em contato com o cliente através do número:{celularClienteMask}");

            var sender = new SenderMessageRequest();
            sender.From = "furry-time";
            sender.To = feirante.Celular;
            sender.Contents = new List<Models.SendMessageZenvia.Content>();
            sender.Contents.Add(new Models.SendMessageZenvia.Content { Type = "text", Text = builder.ToString(), Payload = string.Empty });


            var json = JsonConvert.SerializeObject(sender);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://api.zenvia.com/v1/channels/whatsapp/messages");
            httpRequestMessage.Headers.Add("X-API-TOKEN", "sxyGdagDRB3AFLl51p_y5gGzXnIyx2w4qmzR");
            httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            
            var response = httpClient.SendAsync(httpRequestMessage).Result;

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                return BadRequest();
            }
            

        }



    }
}
