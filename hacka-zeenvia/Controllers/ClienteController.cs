using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ClienteController : ControllerBase
    {

        private readonly Context _context;        

        private readonly ILogger<Cliente> _logger;

        public ClienteController(ILogger<Cliente> logger,
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
        public IActionResult Incluir([FromBody] Cliente model)
        {

            var json = JsonConvert.SerializeObject(model);
            _logger.LogInformation($"Acessando POST {nameof(ClienteController)} {nameof(model)}: {json}");
          
            _context.Cliente.Add(model);
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
            _logger.LogInformation($"Acessando DELETE Cliente/{id}");

            var Cliente =  _context.Cliente.Find(id);

            if (Cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(Cliente);
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult Get([FromQuery] String nome, int? clienteId)
        {
            _logger.LogInformation($"Acessando GET  Cliente {nameof(nome)}: {nome} , {nameof(clienteId)}: {clienteId}");

            var Clientes = _context.Cliente
                                      .Where(x => (clienteId == null || x.ClienteId == clienteId) &&
                                                  (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower()))
                                             ).ToList();

            if (Clientes == null || Clientes.Count() == 0)
            {
                return NotFound();
            }

            return Ok(Clientes);
        }


        [HttpPost("mensagem-enviada")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult MensagemEnviada([FromBody] EventHook eventHook)
        {

            var json = JsonConvert.SerializeObject(eventHook);
            _logger.LogInformation($"Acessando POST mensagem-enviada {nameof(ClienteController)} {nameof(eventHook)}: {json}");

            foreach (var conteudo in eventHook.Message.Contents.Where(x=>x.Type == "text"))
            {
                var mensagem = new MensagemZAP
                {
                    From = eventHook.Message.From,
                    Channel = eventHook.Message.Channel,
                    Direction = eventHook.Message.Direction,
                    To = eventHook.Message.To,
                    Conteudo = conteudo.Text,
                    VisitorFullName = eventHook.Message.Visitor.Name 

                };
                _context.MensagemZAP.Add(mensagem);
            }

            _context.SaveChanges();


            return Ok();


        }


    }
}
