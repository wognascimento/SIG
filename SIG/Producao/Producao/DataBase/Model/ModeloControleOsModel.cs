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
    [Table("qry_modelo_controle_os", Schema = "modelos")]
    public class ModeloControleOsModel
    {
        public string? sigla { get; set; }
        public string? tema { get; set; }
        public string? planilha { get; set; }
        public string? descricao_completa { get; set; }
        public string? unidade { get; set; }
        public long? id_modelo { get; set; }
        public double? qtd_chk_list { get; set; }
        public double? qtd_os { get; set; }
        public long? num_os_produto {  get; set; }
    }
}
