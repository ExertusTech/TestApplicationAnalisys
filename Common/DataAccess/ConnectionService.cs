using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Common.DataAccess;

public interface IConnectionService
{
    public IEnumerable<dynamic> ProcedureQuery(string procedureName, object parameters);

    public dynamic ProcedureQuerySingle(string procedureName, object parameters);
}

public class ConnectionService : IConnectionService
{
    public ConnectionService(IConfiguration config)
    {
        this.Sql = new SqlConnection(config.GetConnectionString("Main"));
    }

    public SqlConnection Sql { get; set; }

    public IEnumerable<dynamic> ProcedureQuery(string procedureName, object parameters)
    {
        return this.Sql.Query(procedureName, parameters, commandType: CommandType.StoredProcedure);
    }

    public dynamic ProcedureQuerySingle(string procedureName, object parameters)
    {
        return this.Sql.QuerySingle(procedureName, parameters, commandType: CommandType.StoredProcedure);
    }
}