namespace RelatorioRoupas.Models
{
    public class Loja
    {
        public int IdLoja { get; set; }
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }
    }
}
