namespace RelatorioRoupas.Endpoints.GrupoVenda
{
    public record AddGrupoVendaRequest(int idprodutoloja, int quantidade_vendido, DateTime data_venda, decimal valor_unitario_venda);
}
