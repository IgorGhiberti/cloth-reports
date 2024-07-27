namespace RelatorioRoupas.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public int CodigoProduto { get; set; }
        public string NomeProduto { get; set; }
        public float ValorUnitario { get; set; }
        public Marca Marca { get; set; }
        public Tipo Tipo { get; set; }
        public Tamanho Tamanho { get; set; }
        public int IdLoja { get; set; } //Fk


    }
}
