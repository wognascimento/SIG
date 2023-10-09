using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HT.DataBase.Model
{
    [Table("view_apontamento", Schema = "ht")]
    public class ViewApontamentoModel
    {
        [Key]
        public long? cod { get; set; }
        public long? codfun { get; set; }
        public string? nome_apelido { get; set; }
        public DateTime data { get; set; }
        public double? semana { get; set; }
        public long? num_os { get; set; }
        public double? ht { get; set; }
        public string? cadastrado_por { get; set; }
        public DateTime? inclusao { get; set; }
        public string? alterado_por { get; set; }
        public DateTime? alteracao { get; set; }
        public string? barcode { get; set; }
    }
}
