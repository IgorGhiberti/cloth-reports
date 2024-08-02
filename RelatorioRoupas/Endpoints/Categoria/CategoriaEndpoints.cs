using Dapper;
using RelatorioRoupas.Data;
using System.Runtime.CompilerServices;

namespace RelatorioRoupas.Categoria
{
    public static class CategoriaEndpoints
    {
        public static void AddCategoriaEndpoints(this WebApplication app)
        {
            var categoriaEndpoints = app.MapGroup("categoria");

            //Criar uma nova categoria de produto
            categoriaEndpoints.MapPost("", async (AddCategoriaRequest request, DbContext context) =>
            {
                using (var dbConnection = context.CreateConnection())
                {
                    var sql = @"INSERT INTO CATEGORIA (NOME) VALUES (@NOME)";
                    {
                        var novaCategoria = new { Nome = request.nome };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, novaCategoria);
                        return Results.Ok(rowsAffected);
                    }
                }

            });

            //Editar uma categoria de produto
            categoriaEndpoints.MapPut("{id}", async (UpdateCategoriaRequest request, DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE CATEGORIA SET NOME = @NOME WHERE IDCATEGORIA = @ID";
                    var categoriaEditada = new { Nome = request.nome, Id = id };
                    var rowsAffected = await dbConnection.ExecuteAsync(sql, categoriaEditada);

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

            //Listar todos as categorias de produto
            categoriaEndpoints.MapGet("", async (DbContext connection) => 
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT IDCATEGORIA, NOME FROM CATEGORIA";
                    {
                        var listagemCategorias = await dbConnection.QueryAsync(sql);

                        return Results.Ok(listagemCategorias);
                    }
                }
            });

            //Listar categoria por id
            categoriaEndpoints.MapGet("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT IDCATEGORIA, NOME FROM CATEGORIA WHERE IDCATEGORIA = @ID";
                    {
                        var categoriaId = new { Id = id };
                        var categoria = await dbConnection.QueryAsync(sql, categoriaId);

                        if(categoria != null) 
                        { 
                            return Results.Ok(categoria);
                        }

                        return Results.NotFound();
                    }
                }
            });

            //Excluir categoria
            categoriaEndpoints.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM CATEGORIA WHERE IDCATEGORIA = @ID";
                    {
                        var categoriaId = new { Id = id };
                        var rowsAffected = await dbConnection.ExecuteAsync(sql, categoriaId);
                        return Results.Ok(rowsAffected);
                    }
                }
            });
        }
    }
}
