namespace LeitorControladoShopping.Data.Api.Model
{
    public class ApiVolumeControlado
    {
        public int? id { get; set; }
        public string sigla { get; set; }
        public long volume { get; set; }
        public DateTime? conferido { get; set; }
        public DateTime? recebido { get; set; }
    }
}
