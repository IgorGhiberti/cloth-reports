using Dapper;
using RelatorioRoupas.Data;
using System.Runtime.CompilerServices;

namespace RelatorioRoupas.Tipo_de_produto
{
    public static class TipoEndpoints
    {
        public static void AddTipoEndpoints(this WebApplication app)
        {
            var tipoEndpoints = app.MapGroup("tipo/produto/{idProduto}");

            //Criar um novo tipo de produto
            tipoEndpoints.MapPost("", async (AddTipoRequest request, DbContext context) =>
            {
                using (var dbConnection = context.CreateConnection())
                {
                    var sql = @"INSERT INTO TIPO (NOME, IDPRODUTO) VALUES (@NOME, @IDPRODUTO)";
                    {
                        var novoTipo = new { Nome = request.nome };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, novoTipo);
                        return Results.Ok(rowsAffected);
                    }
                }

            });

            //Editar um tipo de produto
            tipoEndpoints.MapPut("{id}", async (UpdateTipoRequest request, DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE TIPO SET NOME = @NOME WHERE IDTIPO = @ID";
                    var tipoEditado = new { Nome = request.nome, Id = id };
                    var rowsAffected = await dbConnection.ExecuteAsync(sql, tipoEditado );

                    if(rowsAffected >  0)
                    {
                        return Results.Ok();
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                }
            });

            //Listar todos os tipos de produto
            tipoEndpoints.MapGet("", async (DbContext connection) => 
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT * FROM TIPO";
                    {
                        var listagemTipos = await dbConnection.QueryAsync<Models.Tipo>(sql);

                        return Results.Ok(listagemTipos);
                    }
                }
            });

            //Excluir tipo
            tipoEndpoints.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM TIPO WHERE IDTIPO = @ID";
                    {
                        var tipoId = new { Id = id };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, tipoId);
                        return Results.Ok(rowsAffected);
                    }
                }
            });
        }
    }
}
