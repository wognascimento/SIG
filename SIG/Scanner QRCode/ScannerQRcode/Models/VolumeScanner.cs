using SQLite;

namespace ScannerQRcode.Models
{
    //[Table("volume_scanner")]
    public class VolumeScanner
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Volume { get; set; }
        public string Tipo { get; set; }
        public DateTime? Created { get; set; }
    }
}
