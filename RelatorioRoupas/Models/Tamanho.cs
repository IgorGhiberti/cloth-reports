namespace RelatorioRoupas.Models
{
    public class Tamanho
    {
        public int IdTamanho { get; set; }
        public string Nome { get; set; }
        public int IdProduto { get; set; } //Fk
    }
}
