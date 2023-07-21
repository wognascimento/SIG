namespace ScannerQRcode.Data.Api.Models
{
    public class ConfCargaGeral
    {
        public string Barcode { get; set; }
        public string DocaOrigem { get; set; }
        public DateOnly? Data { get; set; }
        public string Shopp { get; set; }
        public string Resp { get; set; }
        public string Caminhao { get; set; }
        public DateTime? Entradasistema { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataAltera { get; set; }
        public string InseridoPor { get; set; }
        public DateTime? InseridoEm { get; set; }
    }
}
