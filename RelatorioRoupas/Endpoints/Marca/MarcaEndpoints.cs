using Dapper;
using Npgsql;
using RelatorioRoupas.Data;
using RelatorioRoupas.Models;
using System.Data.Common;

namespace RelatorioRoupas.Endpoints.Marca
{
    public static class MarcaEndpoints
    {
        public static void AddEndpointsMarcas(this WebApplication app)
        {
            var endpointsMarcas = app.MapGroup("marcas");

            //Criar novas marcas
            endpointsMarcas.MapPost("", async (AddMarcaRequest request, DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = "INSERT INTO MARCA (NOME) VALUES (@NOME)";
                    {
                        var novaMarca = new { Nome = request.nome };
                        var novaMarcaCriada = await dbConnection.ExecuteScalarAsync(sql, novaMarca);
                        return Results.Ok("Inserção feita com sucesso ");
                    }

                }
            });

            //Listar as marcas existentes
            endpointsMarcas.MapGet("", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = "SELECT NOME FROM MARCA";
                    var marcas = await dbConnection.QueryAsync<Models.Marca>(sql);

                    return Results.Ok(marcas);
                }
            });

            //Editar marca
            endpointsMarcas.MapPut("{id}", async (DbContext connection, int id, UpdateMarcaRequest update) =>
            {

                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE MARCA SET NOME = @NOME WHERE IDMARCA = @Id";
                    {
                        var marcaEditada = new { Nome = update.nome, Id = id };
                        var marca = await dbConnection.ExecuteAsync(sql, marcaEditada);
                        return Results.Ok(marca);
                    }
                }
            });

            //Excluir marca
            endpointsMarcas.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM MARCA WHERE IDMARCA = @ID";
                    {
                        var marcaId = new { Id = id };
                        var affectedRows = await dbConnection.ExecuteAsync(sql, marcaId);
                        if (affectedRows > 0)
                        {
                            return Results.Ok(affectedRows);
                        }
                        else
                        {
                            return Results.NotFound();
                        }

                    }
                }
            });
        }
    }
}
