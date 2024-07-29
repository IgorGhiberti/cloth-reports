using Dapper;
using RelatorioRoupas.Data;
using RelatorioRoupas.Models;

namespace RelatorioRoupas.Endpoints.Venda
{
    public static class VendaEndpoints
    {
        public static void AddVendaEndpoints(this WebApplication app)
        {
            var vendaEndpoints = app.MapGroup("venda");

            //Criar nova venda
            vendaEndpoints.MapPost("grupovenda/{idgrupovenda}", async (DbContext connection, AddVendasRequest request) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"insert into venda (idgrupovenda, datavenda) values (@idgrupovenda, @datavenda);";
                    var novaVenda = new { Idgrupovenda = request.idgrupovenda, DataVenda = request.datavenda };
                    var rowsAffected = await dbConnection.ExecuteAsync(sql, novaVenda);
                    return Results.Ok(rowsAffected);
                }
            });

            //Listar as vendas
            vendaEndpoints.MapGet("", async (DbContext connection) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT 
                                    vnd.datavenda as Data_Venda,
                                    produto.nome AS Produto_Nome,
                                    produto.valor_unitario as Valor_Unitario,
                                    marca.nome AS Marca_Nome,
                                    tamanho.nome AS Tamanho_Nome,
                                    categoria.nome AS Categoria_Nome,
                                    loja.nome AS Loja_nome,
                                    loja.cnpj AS CNPJ,
	                                prd_lj.vendido as Vendido
                                FROM 
                                    venda vnd
                                LEFT JOIN 
                                    grupo_venda gpn_vnd ON vnd.idgrupovenda = gpn_vnd.idgrupovenda
                                LEFT JOIN 
                                    produtos_loja prd_lj ON gpn_vnd.idprodutoloja = prd_lj.idprodutoloja
                                LEFT JOIN 
                                    produto ON prd_lj.idproduto = produto.idproduto
                                LEFT JOIN 
                                    loja ON prd_lj.idloja = loja.idloja
                                LEFT JOIN 
                                    marca ON produto.idmarca = marca.idmarca
                                LEFT JOIN 
                                    tamanho ON produto.idtamanho = tamanho.idtamanho
                                LEFT JOIN 
                                    categoria ON produto.idcategoria = categoria.idcategoria;";
                    var vendas = await dbConnection.QueryAsync(sql);

                    return Results.Ok(vendas);
                }
            });

            //Listar uma venda por id
            vendaEndpoints.MapGet("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"SELECT 
                                    vnd.datavenda as Data_Venda,
                                    produto.nome AS Produto_Nome,
                                    produto.valor_unitario as Valor_Unitario,
                                    marca.nome AS Marca_Nome,
                                    tamanho.nome AS Tamanho_Nome,
                                    categoria.nome AS Categoria_Nome,
                                    loja.nome AS Loja_nome,
                                    loja.cnpj AS CNPJ,
	                                prd_lj.vendido as Vendido
                                FROM 
                                    venda vnd
                                LEFT JOIN 
                                    grupo_venda gpn_vnd ON vnd.idgrupovenda = gpn_vnd.idgrupovenda
                                LEFT JOIN 
                                    produtos_loja prd_lj ON gpn_vnd.idprodutoloja = prd_lj.idprodutoloja
                                LEFT JOIN 
                                    produto ON prd_lj.idproduto = produto.idproduto
                                LEFT JOIN 
                                    loja ON prd_lj.idloja = loja.idloja
                                LEFT JOIN 
                                    marca ON produto.idmarca = marca.idmarca
                                LEFT JOIN 
                                    tamanho ON produto.idtamanho = tamanho.idtamanho
                                LEFT JOIN 
                                    categoria ON produto.idcategoria = categoria.idcategoria
                                WHERE IDVENDA = @ID";
                                
                    var idVenda = new {Id = id};
                    var vendas = await dbConnection.QueryAsync(sql, idVenda);

                    return Results.Ok(vendas);
                }
            });

            //Editar uma venda
            vendaEndpoints.MapPut("{id}/grupovenda/{idgrupovenda}", async (DbContext connection, int id, UpdateVendaRequest update) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"UPDATE VENDA SET IDGRUPOVENDA = @IDGRUPOVENDA, DATAVENDA = @DATAVENDA";
                    var vendaEditada = new { Id = id, DataVenda = update.datavenda, Idgrupovenda = update.idgrupovenda };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, vendaEditada);
                    return Results.Ok(rowsAffected);
                }
            });

            //Deletar uma venda
            vendaEndpoints.MapDelete("{id}", async (DbContext connection, int id) =>
            {
                using (var dbConnection = connection.CreateConnection())
                {
                    var sql = @"DELETE FROM VENDA WHERE IDVENDA = @ID";
                    var idVenda = new { Id = id };

                    var rowsAffected = await dbConnection.ExecuteAsync(sql, idVenda);
                    return Results.Ok(rowsAffected);
                }
            });
        }
    }
}
