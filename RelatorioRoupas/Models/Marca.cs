namespace RelatorioRoupas.Models
{
    public class Marca
    {
        public int IdMarca { get; init; }
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }
    }
}
