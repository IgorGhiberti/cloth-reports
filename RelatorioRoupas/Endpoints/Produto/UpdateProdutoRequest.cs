namespace RelatorioRoupas.Endpoints.Produto
{
    public record UpdateProdutoRequest (string nome, float valorunitario, int idcategoria, int idtamanho, int idmarca);
}
