using Dapper;
using DRP.Infra.DataAccess.Core;
using Microsoft.Extensions.Configuration;

namespace Common;

public class Otro
{
    public IEnumerable<dynamic> Prueba(string query, IConfiguration config)
    {
        var servicio = new ConnectionService(config);
        return servicio.Sql.Query($"select * from test where otro = '{query}'");
    }
}