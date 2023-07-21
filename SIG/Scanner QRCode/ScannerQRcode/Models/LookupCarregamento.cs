using SQLite;

namespace ScannerQRcode.Models
{
    public class LookupCarregamento
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Caminhao { get; set; }
        public string PlacaCaminhao { get; set; }
    }
}
