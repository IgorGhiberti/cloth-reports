namespace RelatorioRoupas.Endpoints.Produto
{
    public record AddProdutoRequest (string nome, float valor_unitario, int idcategoria, int idtamanho, int idmarca);

}
