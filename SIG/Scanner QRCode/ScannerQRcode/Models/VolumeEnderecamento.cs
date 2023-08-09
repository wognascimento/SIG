using SQLite;

namespace ScannerQRcode.Models
{
    public class VolumeEnderecamento
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Endereco { get; set; }
        public string Volume { get; set; }
        public DateTime? Created { get; set; }
    }
}
