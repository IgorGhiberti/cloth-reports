using Dapper;
using RelatorioRoupas.Data;

namespace RelatorioRoupas.Endpoints.Produto
{
    public static class Produto_Endpoints
    {
        public static void AddProdutoEndpoints (this WebApplication app)
        {
            var produtoEndpoints = app.MapGroup("produto/loja/{idLoja}");

            //Criar novo produto
            produtoEndpoints.MapPost("", async (AddProdutoRequest request, DbContext connection) => 
            { 
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"INSERT INTO PRODUTO (CODIGOPRODUTO, NOMEPRODUTO, VALORUNITARIO, IDLOJA) VALUES
                    (@CODIGOPRODUTO, @NOMEPRODUTO, @VALORUNITARIO, @IDLOJA)";

                    var novoProduto = new { CodigoProduto = request.codigoProduto, NomeProduto = request.nomeProduto,
                        ValorUnitario = request.valorUnitario, IdLoja = request.idLoja};

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, novoProduto);

                    if (rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
                    }
                    else
                    {
                        return Results.NotFound();
                    }
                }
            });

            //Editar um produto
            produtoEndpoints.MapPut("{id}", async (UpdateProdutoRequest request, DbContext connection, int id) => 
            { 
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE PRODUTO SET CODIGOPRODUTO = @CODIGOPRODUTO, NOMEPRODUTO = @NOMEPRODUTO
                        VALORUNITARIO = @VALORUNITARIO, IDLOJA = @IDLOJA";
                    var produtoEditado = new
                    {
                        CodigoProduto = request.codigoProduto,
                        NomeProduto = request.nomeProduto,
                        ValorUnitario = request.valorUnitario,
                        IdLoja = request.idLoja,
                        Id = id
                    };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, produtoEditado);

                    if (rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
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
