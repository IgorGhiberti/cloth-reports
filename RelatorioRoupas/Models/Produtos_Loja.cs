namespace RelatorioRoupas.Models
{
    public class Produtos_Loja
    {
        public int idProdutoLoja { get; init; }
        public int idProduto { get; set; }
        public int idLoja { get; set; }
        public string vendido { get; set; }
    }
}
