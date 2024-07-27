namespace RelatorioRoupas.Models
{
    public class Tipo
    {
        public int IdTipo { get; set; }
        public string Nome { get; set; }
        public int IdProduto { get; set; } //Fk
    }
}
