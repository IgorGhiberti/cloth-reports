namespace RelatorioRoupas.Models
{
    public class Venda
    {
        public int IdVenda { get; set; }
        public DateTime DataAtual { get; set; }
        public List<Venda_Produto> Vendas_Produtos { get; set; }
    }
}
