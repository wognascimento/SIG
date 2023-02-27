using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producao
{
    [Keyless]
    [Table("view_historico_setor", Schema = "modelos")]
    public class HistoricoSetorModel
    {
        public long? codcompladicional { get; set; }
        public long? codigo_setor { get; set; }
        public string? setor { get; set; }
        public bool? selesao { get; set; }
        public string? observacao {  get; set; }
    }
}
