namespace RelatorioRoupas.Models
{
    public class Loja
    {
        public int IdLoja { get; init; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public List<Produto> Produtos { get; set; }
    }
}
