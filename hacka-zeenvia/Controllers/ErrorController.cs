using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace hacka_zeenvia.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/error-exception")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorException()
        {

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var json = JsonConvert.SerializeObject(exceptionHandlerPathFeature.Error);
            _logger.LogCritical($"Ocorreu uma Exception {json}");

            return Problem(exceptionHandlerPathFeature.Error.StackTrace,
                           exceptionHandlerPathFeature.Path,
                           StatusCodes.Status500InternalServerError,
                           exceptionHandlerPathFeature.Error.Message,
                           "https://httpstatuses.com/500");
        }
    }
}
