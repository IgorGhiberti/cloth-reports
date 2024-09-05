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
                    var sql = @"INSERT INTO GRUPO_VENDA (IDPRODUTOLOJA, QUANTIDADE_VENDIDO, DATA_VENDA, VALOR_UNITARIO_VENDA) 
                                VALUES (@IDPRODUTOLOJA, @QUANTIDADE_VENDIDO, @DATA_VENDA, @VALOR_UNITARIO_VENDA)";

                    var novoGpVenda = new
                    {
                        IdProdutoLoja = request.idprodutoloja,
                        Quantidade_Vendido = request.quantidade_vendido,
                        Data_venda = request.data_venda,
                        Valor_unitario_venda = request.valor_unitario_venda
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
                                    GV.VALOR_UNITARIO_VENDA AS VALOR_UNITARIO_VENDA,
                                    GV.DATA_VENDA AS DATA_VENDA,
                                    PRD_LJ.IDPRODUTOLOJA,
                                    LJ.NOME AS LOJA,
                                    GV.QUANTIDADE_VENDIDO AS QUANTIDADE_VENDIDO,
                                    GV.IDGRUPOVENDA,
                                    SUM(GV.VALOR_UNITARIO_VENDA * GV.QUANTIDADE_VENDIDO) AS TOTAL
                                FROM 
                                    GRUPO_VENDA GV
                                LEFT OUTER JOIN
                                    PRODUTOS_LOJA PRD_LJ ON GV.IDPRODUTOLOJA = PRD_LJ.IDPRODUTOLOJA
                                LEFT OUTER JOIN
                                    PRODUTO PRD ON PRD_LJ.IDPRODUTO = PRD.IDPRODUTO
                                LEFT OUTER JOIN
                                    LOJA LJ ON PRD_LJ.IDLOJA = LJ.IDLOJA
                                GROUP BY
                                    PRD.NOME, LJ.NOME, PRD_LJ.IDPRODUTOLOJA, GV.VALOR_UNITARIO_VENDA, PRD.VALOR_UNITARIO, GV.QUANTIDADE_VENDIDO, GV.IDGRUPOVENDA
                                ORDER BY
                                    PRD.NOME ASC, LJ.NOME ASC;";

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

            //Filtro de data
            gp_vendaEndpoint.MapGet("/filtro-data", async (DbContext connection, DateTime data_inicio, DateTime data_fim) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT
	                                PRD.NOME AS PRODUTO,
	                                GV.VALOR_UNITARIO_VENDA AS VALOR_UNITARIO_VENDA,
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
                                WHERE DATA_VENDA BETWEEN @DATA_INICIO AND @DATA_FIM";

                    var listagemVenda = await dbConnection.QueryAsync(sql, new { Data_Inicio = data_inicio, Data_Fim = data_fim });

                    if (listagemVenda != null)
                        return Results.Ok(listagemVenda);

                    return Results.NotFound();
                }
            });

            //Traz os produtos vendidos de determinada loja
            gp_vendaEndpoint.MapGet("/produtos-vendidos/{idloja}", async (DbContext connection ,int idloja) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"select
	                                prd.nome,
	                                gpv.valor_unitario_venda as VALOR_UNITARIO_VENDA,
	                                mc.nome as Marca,
	                                tm.nome as Tamanho,
	                                ct.nome as Categoria,
	                                prd_lj.idprodutoloja,
                                    gpv.idgrupovenda,
                                    gpv.quantidade_vendido
                                from
                                    grupo_venda gpv
                                left outer join
                                    produtos_loja prd_lj on gpv.idprodutoloja = prd_lj.idprodutoloja
                                left outer join
                                    produto prd on prd_lj.idproduto = prd.idproduto
                                left outer join
                                    marca mc on prd.idmarca = mc.idmarca
                                left outer join
                                    tamanho tm on prd.idtamanho = tm.idtamanho
                                left outer join
                                    categoria ct on prd.idcategoria = ct.idcategoria
                                where
                                    prd_lj.idloja = @idloja";

                    var listagemVendaLoja = await dbConnection.QueryAsync(sql, new { Idloja = idloja });

                    if (listagemVendaLoja.Count() > 0)
                        return Results.Ok(listagemVendaLoja);

                    return Results.NotFound();
                }
            });

            //Agrupa o resultado das vendas por loja
            gp_vendaEndpoint.MapGet("/vendaPorLoja", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT 
	                                LJ.NOME AS LOJA,
	                                SUM(GP.QUANTIDADE_VENDIDO * GP.VALOR_UNITARIO_VENDA) AS TOTAL
                                FROM
	                                GRUPO_VENDA GP
                                LEFT OUTER JOIN
	                                PRODUTOS_LOJA PRD_LJ ON GP.IDPRODUTOLOJA = PRD_LJ.IDPRODUTOLOJA
                                LEFT OUTER JOIN
	                                LOJA LJ ON PRD_LJ.IDLOJA = LJ.IDLOJA
                                GROUP BY LJ.NOME";

                    var listagemReports = await dbConnection.QueryAsync(sql);

                    if(listagemReports.Count() > 0)
                        return Results.Ok(listagemReports);

                    return Results.NotFound();
                }
            });

        }
    }
}
