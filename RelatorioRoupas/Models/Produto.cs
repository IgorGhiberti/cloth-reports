namespace RelatorioRoupas.Models
{
    public class Produto
    {
        public int IdProduto { get; init; }
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public string NomeMarca { get; set; }
        public string NomeCategoria { get; set; }
        public string NomeTamanho { get; set; }
        public int idCategoria { get; set; }
        public int idMarca { get; set; }
        public int idTamanho { get; set; }

    }
}
