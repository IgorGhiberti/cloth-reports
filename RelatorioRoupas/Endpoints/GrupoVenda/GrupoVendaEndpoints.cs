using Dapper;
using RelatorioRoupas.Data;

namespace RelatorioRoupas.Endpoints.GrupoVenda
{
    public static class GrupoVendaEndpoints
    {
        public static void AddGrupoVendaEndpoints(this WebApplication app)
        {
            app.MapPost("grupovenda/produtoloja/{idprodutoloja}", async (DbContext connection, int idprodutoloja) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = "insert into grupo_venda (idprodutoloja) values (@idprodutoloja);";
                    var novoGrupoVenda = new {Idprodutoloja = idprodutoloja};
                    var rowsAffected = await dbConnection.ExecuteAsync(sql, novoGrupoVenda);
                }
            });
        }
    }
}
