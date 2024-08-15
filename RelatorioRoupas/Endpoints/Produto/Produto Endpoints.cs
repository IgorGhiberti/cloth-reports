using Dapper;
using RelatorioRoupas.Data;
using RelatorioRoupas.Models;

namespace RelatorioRoupas.Endpoints.Produto
{
    public static class Produto_Endpoints
    {
        public static void AddProdutoEndpoints (this WebApplication app)
        {
            var produtoEndpoints = app.MapGroup("produto");

            //Criar novo produto
            produtoEndpoints.MapPost("", async (AddProdutoRequest request, DbContext connection) => 
            { 
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"INSERT INTO PRODUTO (nome, valor_unitario, idcategoria, idtamanho, idmarca) VALUES
                    (@nome, @valor_unitario, @idcategoria, @idtamanho, @idmarca)";

                    var novoProduto = new { 
                        Nome = request.nome,
                        valor_unitario = request.valor_unitario,
                        IdCategoria = request.idcategoria,
                        IdTamanho = request.idtamanho,
                        IdMarca = request.idmarca
                    };

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
                    var sql = @"UPDATE PRODUTO SET NOME = @NOME, VALOR_UNITARIO = @VALOR_UNITARIO,
                        IDCATEGORIA = @IDCATEGORIA, IDMARCA = @IDMARCA, IDTAMANHO = @IDTAMANHO WHERE IDPRODUTO = @ID";
                    var produtoEditado = new
                    {
                        Nome = request.nome,
                        VALOR_UNITARIO = request.valor_unitario,
                        IDCATEGORIA = request.idcategoria,
                        IDMARCA = request.idmarca,
                        IDTAMANHO = request.idtamanho,
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

            //Selecionar todos os produtos
            produtoEndpoints.MapGet("", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"select 
                                    prd.idproduto as IdProduto,
	                                prd.nome as NomeProduto, 
	                                valor_unitario as ValorUnitario, 
	                                mrc.nome as NomeMarca, 
	                                ctr.nome as NomeCategoria, 
	                                tam.nome as NomeTamanho
                                from 
	                                produto prd
                                left outer join 
	                                marca mrc on prd.idmarca = mrc.idmarca
                                left outer join 
	                                categoria ctr on prd.idcategoria = ctr.idcategoria
                                left outer join 
	                                tamanho tam on prd.idtamanho = tam.idtamanho";

                    var produtos = await dbConnection.QueryAsync(sql);

                    return Results.Ok(produtos);
                }
            });

            //Selecionar apenas um produto
            produtoEndpoints.MapGet("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var idProduto = new { Id = id };
                    var sql = @"select 
                                    prd.idproduto as IdProduto,
	                                prd.nome as Nome, 
	                                valor_unitario as Valor_Unitario,
                                    mrc.idmarca as IdMarca,
	                                mrc.nome as NomeMarca,
                                    ctr.idcategoria as IdCategoria,
	                                ctr.nome as NomeCategoria, 
                                    tam.idtamanho as IdTamanho,
	                                tam.nome as NomeTamanho
                                from 
	                                produto prd
                                left outer join 
	                                marca mrc on prd.idmarca = mrc.idmarca
                                left outer join 
	                                categoria ctr on prd.idcategoria = ctr.idcategoria
                                left outer join 
	                                tamanho tam on prd.idtamanho = tam.idtamanho
                                where idproduto = @id";
                    var produto = await dbConnection.QuerySingleOrDefaultAsync(sql, idProduto);
                    return Results.Ok(produto);
                }
            });

            //Deletar um produto
            produtoEndpoints.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var idProduto = new { Id = id };
                    var sql = @"DELETE FROM PRODUTO WHERE IDPRODUTO = @ID";
                    var rowsAffected = await dbConnection.ExecuteAsync(sql, idProduto);
                    return Results.Ok(rowsAffected);
                }
            });
        }
    }
}
