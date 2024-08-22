namespace RelatorioRoupas.Models
{
    public class Grupo_Venda
    {
        public int idGrupoVenda { get; init; }
        public int idProdutoLoja { get; set; }
        public int quantidade_vendido { get; set; }
        public DateTime Data_Venda { get; set; }
        public string Loja { get; set; }
        public string Produto { get; set; }

    }
}
