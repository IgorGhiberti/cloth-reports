namespace RelatorioRoupas.Models
{
    public class Categoria
    {
        public int IdTipo { get; init; }
        public string Nome { get; set; }
        public List<Produto> Produtos { get; }
    }
}
