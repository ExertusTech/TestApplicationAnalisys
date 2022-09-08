using Common;
using Dapper;
using DRP.Infra.DataAccess.Core;
using Microsoft.AspNetCore.Mvc;

namespace TestApplicationAnalisys
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TestController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Get()
        {
            return Ok("prueba");
        }

        [Route("bd/{query}")]
        public IActionResult Get(string query)
        {
            var servicio = new Otro();
            var result = servicio.Prueba($"select * from test where otro = '{query}'", _config);
            return Ok(result);
        }

    }
}
