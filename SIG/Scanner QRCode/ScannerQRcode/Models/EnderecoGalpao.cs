using SQLite;

namespace ScannerQRcode.Models
{
    public class EnderecoGalpao
    {
        [PrimaryKey]
        public int? IdEndereco { get; set; }
        public string Endereco { get; set; }
        public string Barcode { get; set; }
    }
}
