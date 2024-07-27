namespace RelatorioRoupas.Endpoints.Produto
{
    public record UpdateProdutoRequest (int codigoProduto, string nomeProduto, float valorUnitario, int idLoja);
}
