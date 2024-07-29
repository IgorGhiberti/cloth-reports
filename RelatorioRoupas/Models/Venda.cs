namespace RelatorioRoupas.Models
{
    public class Venda
    {
        public int IdVenda { get; init; }
        public DateTime DataAtual { get; set; }
        public int idGrupoVenda { get; set; }
        public string NomeProduto { get; }
        public float Valor_Unitario { get;}
        public string Marca_Nome { get; }
        public string Tamanho_Nome { get; }
        public string Categoria_Nome { get; }
        public string Loja_nome { get; }
        public string Loja_cnpj { get; }
        public char Vendido { get; }
    }
}
