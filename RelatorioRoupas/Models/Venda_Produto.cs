namespace RelatorioRoupas.Models
{
    public class Venda_Produto
    {
        public int Id_Venda { get; set; }
        public int Id_Produto { get; set; }
        public Produto Produto { get; set; }
        public Venda Venda { get; set; }
    }
}
