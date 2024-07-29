using Dapper;
using RelatorioRoupas.Data;

namespace RelatorioRoupas.Endpoints.ProdutosLoja
{
    public static class ProdutoLojaEndpoints
    {
        public static void AddProdutoLojaEndpoints(this WebApplication app)
        {
            var produtoLojaEndpoints = app.MapGroup("produtoLoja/produto/{idproduto}/loja/{idloja}");

            //Inserção de um novo produto loja
            produtoLojaEndpoints.MapPost("", async (DbContext connection, AddProdutoLojaRequest request) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"insert into produtos_loja (idproduto, idloja, vendido) 
                                values (@idproduto, @idloja, @vendido);";

                    var novoProdutoLoja = new {
                        IdProduto = request.idproduto, 
                        IdLoja = request.idloja, 
                        Vendido = request.vendido
                    };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, novoProdutoLoja);
                }
            });
        }
    }
}
