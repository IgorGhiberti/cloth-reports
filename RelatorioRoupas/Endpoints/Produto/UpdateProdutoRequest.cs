namespace RelatorioRoupas.Endpoints.Produto
{
    public record UpdateProdutoRequest (string nome, float valor_unitario, int idcategoria, int idtamanho, int idmarca);
}
