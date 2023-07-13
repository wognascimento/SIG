
using SQLite;

namespace ScannerQRcode.Models
{
    public class VolumeLookup
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Volume { get; set; }
    }
}
