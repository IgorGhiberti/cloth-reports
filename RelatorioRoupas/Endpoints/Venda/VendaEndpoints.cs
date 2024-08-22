using Dapper;
using RelatorioRoupas.Data;
using RelatorioRoupas.Models;

namespace RelatorioRoupas.Endpoints.Venda
{
    public static class VendaEndpoints
    {
        public static void AddVendaEndpoints(this WebApplication app)
        {
            var vendaEndpoints = app.MapGroup("venda");

            //Adicionar nova venda
            vendaEndpoints.MapPost("", async (DbContext connection, AddVendasRequest request) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"INSERT INTO VENDA (DATAVENDA) VALUES (@DATAVENDA)";

                    var venda = new {DataVenda = request.datavenda};

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, venda);

                    if (rowsAffected > 0)
                        return Results.Ok();

                    return Results.BadRequest();
                }
            });

            //Editar data da venda
            vendaEndpoints.MapPut("{id}", async (DbContext connection, int id, UpdateVendaRequest request) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE VENDA SET DATAVENDA = @DATAVENDA WHERE IDVENDA = @ID";

                    var venda = new { Id = id, DataVenda = request.datavenda };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, venda);

                    if (rowsAffected > 0)
                        return Results.Ok();

                    return Results.BadRequest();
                }
            });

            //Excluir venda
            vendaEndpoints.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM VENDA WHERE IDVENDA = @ID";

                    var venda = new { Id = id };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, venda);

                    if (rowsAffected > 0)
                        return Results.Ok();

                    return Results.BadRequest();
                }
            });
        }
    }
}
