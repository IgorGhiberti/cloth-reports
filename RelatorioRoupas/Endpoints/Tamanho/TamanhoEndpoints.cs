using Dapper;
using RelatorioRoupas.Data;
using RelatorioRoupas.Models;

namespace RelatorioRoupas.Endpoints.Tamanho
{
    public static class TamanhoEndpoints
    {
        public static void AddTamanhoEndpoints(this WebApplication app)
        {
            var endpointsTamanho = app.MapGroup("tamanho");

            //Cadastrar novo tamanho
            endpointsTamanho.MapPost("", async (AddTamanhoRequest request, DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = "INSERT INTO TAMANHO (NOME) VALUES (@NOME)";
                    {
                        var novoTamanho = new { Nome = request.nome };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, novoTamanho);
                        if (rowsAffected > 0)
                        {
                            return Results.Ok(rowsAffected);
                        }
                        else
                        {
                            return Results.BadRequest("No máximo 2 caracteres");
                        }
                    }
                }
            });

            //Editar tamanho
            endpointsTamanho.MapPut("{id}", async (UpdateTamanhoRequest update, DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE TAMANHO SET NOME = @NOME WHERE IDTAMANHO = @ID";
                    var tamanhoEdit = new { Nome = update.nome, Id = id };
                    var rowsAffected = await dbConnection.ExecuteAsync(sql, tamanhoEdit);
                    return Results.Ok(rowsAffected);
                }
            });

            //Excluir tamanho
            endpointsTamanho.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM TAMANHO WHERE IDTAMANHO = @ID";
                    {
                        var tamanhoId = new { Id = id };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, tamanhoId);
                        return Results.Ok(rowsAffected);
                    }
                }
            });

            //Listar tamanhos
            endpointsTamanho.MapGet("", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = "SELECT IDTAMANHO, NOME FROM TAMANHO";
                    var listagemTamanhos = await dbConnection.QueryAsync(sql);
                    return Results.Ok(listagemTamanhos);
                }
            });

            //Listar tamanho por id
            endpointsTamanho.MapGet("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = "SELECT IDTAMANHO, NOME FROM TAMANHO WHERE IDTAMANHO = @ID";
                    var tamanhoExibido = new {Id = id };
                    var tamanho = await dbConnection.QuerySingleOrDefaultAsync(sql, tamanhoExibido);

                    if (tamanho != null)
                    {
                        return Results.Ok(tamanho);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                }
            });
        }
    }
}
