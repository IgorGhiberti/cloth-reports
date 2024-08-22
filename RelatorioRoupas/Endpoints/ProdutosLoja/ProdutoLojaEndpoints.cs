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
                    var produto = @"SELECT * FROM PRODUTOS_LOJA WHERE IDPRODUTO = @IDPRODUTO AND IDLOJA = @IDLOJA";

                    var produtoLojaSelecionado = await dbConnection.QueryFirstOrDefaultAsync<Models.Produtos_Loja>(produto, new { IDPRODUTO = request.idproduto, IDLOJA = idloja }); 

                    if (produtoLojaSelecionado == null) 
                    {
                        var sql = @"insert into produtos_loja (idproduto, idloja, quantidade_produto) 
                                values (@idproduto, @idloja, @quantidade_produto);";

                        var novoProdutoLoja = new
                        {
                            IdProduto = request.idproduto,
                            IdLoja = idloja,
                            Quantidade_produto = request.quantidade_produto
                        };

                        var rowsAffected = await dbConnection.ExecuteAsync(sql, novoProdutoLoja);

                        if (rowsAffected > 0)
                        {
                            return Results.Ok(rowsAffected);
                        }

                        return Results.NotFound();
                    }

                    return Results.BadRequest("Este produto já está nesta loja");
                    
                }
            });

            //Listar os produtos pela loja
            produtoLojaEndpoints.MapGet("produtoLoja/loja/{idloja}", async (DbContext connection, int idloja) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"select
	                                prd.idproduto,
	                                prd.nome,
	                                prd.valor_unitario,
	                                mc.nome as Marca,
	                                tm.nome as Tamanho,
	                                ct.nome as Categoria,
	                                prd_lj.quantidade_produto as Quantidade_produto,
	                                prd_lj.idprodutoloja
                                from
	                                produtos_loja prd_lj
                                left outer join
                                    produto prd on prd_lj.idproduto = prd.idproduto
                                left outer join
                                    marca mc on prd.idmarca = mc.idmarca
                                left outer join
                                    tamanho tm on prd.idtamanho = tm.idtamanho
                                left outer join
                                    categoria ct on prd.idcategoria = ct.idcategoria
                                where prd_lj.idloja = @idloja";

                    var produtosPorLoja = await dbConnection.QueryAsync(sql, new {  IDLOJA = idloja });

                    if (produtosPorLoja.Count() > 0)
                    {
                        return Results.Ok(produtosPorLoja);
                    }

                    return Results.NotFound();
                    
                }
            });

            //Aumenta a quantidade
            produtoLojaEndpoints.MapPut("{id}/aumentar", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE PRODUTOS_LOJA SET QUANTIDADE_PRODUTO = QUANTIDADE_PRODUTO + 1 WHERE IDPRODUTOLOJA = @ID";

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, new {Id = id});

                    
                    if (rowsAffected > 0)
                        return Results.Ok(rowsAffected);

                    return Results.NotFound();

                }
            });

            //Diminui a quantidade
            produtoLojaEndpoints.MapPut("{id}/diminuir", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE PRODUTOS_LOJA SET QUANTIDADE_PRODUTO = QUANTIDADE_PRODUTO - 1 WHERE IDPRODUTOLOJA = @ID";

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, new { Id = id });


                    if (rowsAffected > 0)
                        return Results.Ok(rowsAffected);

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
