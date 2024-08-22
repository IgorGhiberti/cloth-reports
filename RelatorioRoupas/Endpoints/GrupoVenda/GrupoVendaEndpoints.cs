using Dapper;
using RelatorioRoupas.Data;

namespace RelatorioRoupas.Endpoints.GrupoVenda
{
    public static class GrupoVendaEndpoints
    {
        public static void AddGrupoVendaEndpoints(this WebApplication app)
        {
            var gp_vendaEndpoint = app.MapGroup("grupo_venda");

            //Cria um novo grupo de vendas
            gp_vendaEndpoint.MapPost("", async (DbContext connection, AddGrupoVendaRequest request) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"INSERT INTO GRUPO_VENDA (IDPRODUTOLOJA, QUANTIDADE_VENDIDO, DATA_VENDA) 
                                VALUES (@IDPRODUTOLOJA, @QUANTIDADE_VENDIDO, @DATA_VENDA)";

                    var novoGpVenda = new
                    {
                        IdProdutoLoja = request.idprodutoloja,
                        Quantidade_Vendido = request.quantidade_vendido,
                        Data_venda = request.data_venda
                    };

                    var executeQuery = await dbConnection.ExecuteAsync(sql, novoGpVenda);

                    var updateQtdSql = @"UPDATE PRODUTOS_LOJA SET QUANTIDADE_PRODUTO = QUANTIDADE_PRODUTO -
                                        @QuantidadeVendido WHERE IDPRODUTOLOJA = @IDPRODUTOLOJA";

                    var novoProdutoLoja = new
                    {
                        IdProdutoLoja = request.idprodutoloja,
                        QuantidadeVendido = request.quantidade_vendido
                    };

                    var rowsAffected = await dbConnection.ExecuteAsync(updateQtdSql, novoProdutoLoja);

                    if (rowsAffected > 0)
                    {
                        return Results.Ok(rowsAffected);
                    }
                    else
                    {
                        return Results.BadRequest();
                    }


                }
            });

            //Exibe os agrupamentos das vendas
            gp_vendaEndpoint.MapGet("", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT
	                                PRD.NOME AS PRODUTO,
	                                PRD.VALOR_UNITARIO AS VALOR_UNITARIO,
	                                GV.DATA_VENDA AS DATA_VENDA,
	                                PRD_LJ.IDPRODUTOLOJA,
	                                LJ.NOME AS LOJA,
	                                GV.QUANTIDADE_VENDIDO AS QUANTIDADE_VENDIDO,
	                                GV.IDGRUPOVENDA
                                FROM 
	                                GRUPO_VENDA GV
                                LEFT OUTER JOIN
	                                PRODUTOS_LOJA PRD_LJ ON GV.IDPRODUTOLOJA = PRD_LJ.IDPRODUTOLOJA
                                LEFT OUTER JOIN
	                                PRODUTO PRD ON PRD_LJ.IDPRODUTO = PRD.IDPRODUTO
                                LEFT OUTER JOIN
	                                LOJA LJ ON PRD_LJ.IDLOJA = LJ.IDLOJA
                                GROUP BY
	                                PRD.NOME, LJ.NOME, PRD_LJ.IDPRODUTOLOJA, PRD.VALOR_UNITARIO, GV.QUANTIDADE_VENDIDO, GV.IDGRUPOVENDA";

                    var listagemVendas = await dbConnection.QueryAsync(sql);

                    if (listagemVendas != null)
                        return Results.Ok(listagemVendas);

                    return Results.NotFound("Não foi achado o registro de vendas");
                }
            });

            //Aumenta a quantidade de produtos vendidos
            gp_vendaEndpoint.MapPut("{id}/aumentar", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE GRUPO_VENDA SET QUANTIDADE_VENDIDO = QUANTIDADE_VENDIDO + 1 WHERE IDGRUPOVENDA = @ID";

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, new {Id = id});

                    if (rowsAffected > 0)
                        return Results.Ok(rowsAffected);

                    return Results.BadRequest("Não foi possível diminuir");
                }
            });

            //Diminui a quantidade de produtos vendidos
            gp_vendaEndpoint.MapPut("{id}/diminuir", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE GRUPO_VENDA SET QUANTIDADE_VENDIDO = QUANTIDADE_VENDIDO - 1 WHERE IDGRUPOVENDA = @ID";

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, new { Id = id });

                    if (rowsAffected > 0)
                        return Results.Ok();

                    return Results.BadRequest("Não foi possível diminuir");
                }
            });

            //Excluir a relação entre a venda e o produto_loja
            gp_vendaEndpoint.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection()) 
                {
                    var sql = @"DELETE FROM GRUPO_VENDA WHERE IDGRUPOVENDA = @ID";

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, new {Id = id});

                    if (rowsAffected > 0)
                        return Results.Ok();

                    return Results.BadRequest("Não foi possível excluir");
                }
            });

        }
    }
}
