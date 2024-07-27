namespace RelatorioRoupas.Endpoints.Produto
{
    public record AddProdutoRequest (int codigoProduto, string nomeProduto, float valorUnitario, int idLoja);

}
