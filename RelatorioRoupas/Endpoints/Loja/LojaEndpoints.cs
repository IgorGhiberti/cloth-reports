﻿using Dapper;
using RelatorioRoupas.Data;

namespace RelatorioRoupas.Endpoints.Loja
{
    public static class LojaEndpoints
    {
        public static void AddLojaEndpoints(this WebApplication app)
        {
            var lojaEndpoints = app.MapGroup("loja");

            //Criar o registro de uma nova loja
            lojaEndpoints.MapPost("", async (AddLojaRequest request, DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"INSERT INTO LOJA (NOME, CNPJ) VALUES (@NOME, @CNPJ)";
                    {
                        var novaLoja = new { Nome = request.nome, CNPJ = request.cnpj };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, novaLoja);
                        if (rowsAffected > 0)
                        {
                            return Results.Ok(rowsAffected);
                        }
                        else
                        {
                            return Results.BadRequest();
                        }
                    }
                }
            });

            //Editar loja existente
            lojaEndpoints.MapPut("{id}", async (UpdateLojaRequest update, DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE LOJA SET NOME = @NOME, CNPJ = @CNPJ WHERE IDLOJA = @ID";
                    {
                        var lojaEditada = new { Nome = update.nome, CNPJ = update.cnpj, Id = id };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, lojaEditada);
                        if (rowsAffected > 0)
                        {
                            return Results.Ok(rowsAffected);
                        }
                        else
                        {
                            return Results.BadRequest();
                        }
                    }
                }
            });

            //Deletar loja
            lojaEndpoints.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM LOJA WHERE IDLOJA = @ID";
                    {
                        var lojaExcluida = new { Id = id };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, lojaExcluida);
                        if (rowsAffected > 0)
                        {
                            return Results.Ok(rowsAffected);
                        }
                        else
                        {
                            return Results.BadRequest();
                        }
                    }
                }
            });

            //Listar todas as lojas
            lojaEndpoints.MapGet("", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT IDLOJA, NOME, CNPJ FROM LOJA";
                    {
                        var listagemLoja = await dbConnection.QueryAsync(sql);
                        return Results.Ok(listagemLoja);
                    }
                }
            });

            //Listar loja
            lojaEndpoints.MapGet("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT IDLOJA, NOME, CNPJ FROM LOJA WHERE IDLOJA = @ID";
                    {
                        var idLoja = new { Id = id };
                        var listagemLoja = await dbConnection.QuerySingleOrDefaultAsync(sql, idLoja);

                        if (listagemLoja != null)
                        {
                            return Results.Ok(listagemLoja);
                        }

                        return Results.NotFound();
                    }
                }
            });

        }
    }
}
