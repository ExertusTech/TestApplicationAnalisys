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
            var servicio = new ConnectionService(_config);
            var result = servicio.Sql.Query($"select * from test where otro = '{query}'");
            return Ok(result);
        }

    }
}
