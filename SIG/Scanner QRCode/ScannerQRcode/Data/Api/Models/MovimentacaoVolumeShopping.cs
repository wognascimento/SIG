namespace ScannerQRcode.Data.Api.Models
{
    public class MovimentacaoVolumeShopping
    {
        public long? id_linha_inserida { get; set; }
        public string barcode_volume { get; set; }
        public string barcode_endereco { get; set; }
        public string inserido_por { get; set; }
        public DateTime? inserido_em { get; set; }
    }
}
