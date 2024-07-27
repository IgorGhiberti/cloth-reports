namespace RelatorioRoupas.Models
{
    public class Marca
    {
        public int IdMarca { get; set; }
        public string Nome { get; set; }
        public int IdProduto { get; set; } //Fk
    }
}
