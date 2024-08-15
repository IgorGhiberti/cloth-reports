using Dapper;
using RelatorioRoupas.Data;

namespace RelatorioRoupas.Endpoints.ProdutosLoja
{
    public static class ProdutoLojaEndpoints
    {
        public static void AddProdutoLojaEndpoints(this WebApplication app)
        {
            var produtoLojaEndpoints = app.MapGroup("");

            //Inserção de um novo produto loja
            produtoLojaEndpoints.MapPost("produtoLoja/loja/{idloja}", async (DbContext connection, AddProdutoLojaRequest request, int idloja) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"insert into produtos_loja (idproduto, idloja, vendido) 
                                values (@idproduto, @idloja, false);";

                    var novoProdutoLoja = new {
                        IdProduto = request.idproduto, 
                        IdLoja = idloja
                    };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, novoProdutoLoja);

                    if(rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
                    }

                    return Results.NotFound();
                }
            });

            //Listar os produtos pela loja associada que estão vendidos
            produtoLojaEndpoints.MapGet("produtoLoja/loja/{idloja}/vendido", async (DbContext connection, int idloja) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT
	                                PRD.*,
                                    COUNT(PRD_LJ.IDPRODUTO) AS QUANTIDADE,
                                    PRD_LJ.VENDIDO AS VENDIDO
                                FROM
	                                PRODUTOS_LOJA PRD_LJ
                                LEFT OUTER JOIN
	                                PRODUTO PRD ON PRD_LJ.IDPRODUTO = PRD.IDPRODUTO
                                LEFT OUTER JOIN
	                                LOJA LJ ON PRD_LJ.IDLOJA = LJ.IDLOJA
                                WHERE PRD_LJ.IDLOJA = @IDLOJA AND PRD_LJ.VENDIDO = TRUE
                                GROUP BY 
                                    PRD.IDPRODUTO, PRD_LJ.VENDIDO";
                 
                    var produtosPorLoja = await dbConnection.QueryAsync(sql, new {  IDLOJA = idloja });

                    if (produtosPorLoja.Count() > 0)
                    {
                        return Results.Ok(produtosPorLoja);
                    }

                    return Results.NotFound();
                    
                }
            });

            //Listar os produtos pela loja associada que não estão vendidos
            produtoLojaEndpoints.MapGet("produtoLoja/loja/{idloja}/nao-vendido", async (DbContext connection, int idloja) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT
	                                PRD.*,
                                    COUNT(PRD_LJ.IDPRODUTO) AS QUANTIDADE,
                                    PRD_LJ.VENDIDO AS VENDIDO
                                FROM
	                                PRODUTOS_LOJA PRD_LJ
                                LEFT OUTER JOIN
	                                PRODUTO PRD ON PRD_LJ.IDPRODUTO = PRD.IDPRODUTO
                                LEFT OUTER JOIN
	                                LOJA LJ ON PRD_LJ.IDLOJA = LJ.IDLOJA
                                WHERE PRD_LJ.IDLOJA = @IDLOJA AND PRD_LJ.VENDIDO = FALSE
                                GROUP BY PRD.IDPRODUTO, PRD_LJ.VENDIDO";

                    var produtosPorLoja = await dbConnection.QueryAsync(sql, new { IDLOJA = idloja });

                    if (produtosPorLoja.Count() > 0)
                    {
                        return Results.Ok(produtosPorLoja);
                    }

                    return Results.NotFound();

                }
            });

            //Edita de vendido para não vendido
            produtoLojaEndpoints.MapPut("{id}/nao-vendido", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE PRODUTOS_LOJA SET VENDIDO = FALSE WHERE IDPRODUTOLOJA = @ID";

                    var statusAlterado = new { Id = id };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, statusAlterado);

                    if(rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
                    }

                    return Results.NotFound();
                    
                }
            });

            //Edita de não vendido para vendido
            produtoLojaEndpoints.MapPut("{id}/vendido", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE PRODUTOS_LOJA SET VENDIDO = TRUE WHERE IDPRODUTOLOJA = @ID";

                    var statusAlterado = new { Id = id };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, statusAlterado);

                    if (rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
                    }

                    return Results.NotFound();

                }
            });

            //Exclui a relação de produto - este produto, não está mais nesta loja
            produtoLojaEndpoints.MapDelete("produtoLoja/{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection()) 
                { 
                    var sql = @"DELETE FROM PRODUTOS_LOJA WHERE IDPRODUTOLOJA = @ID";

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, new {Id = id});

                    if (rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
                    }

                    return Results.NotFound();

                }
            });


        }
    }
}
