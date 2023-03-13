using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expedicao
{
    [Keyless]
    [Table("c_bd_produtos_baiados_geral_total_data", Schema = "expedicao")]
    public class ProdutosBaiadosGeralTotalDataModel
    {
        public string? sigla { get; set; }
        public DateTime? data { get; set; }
        public string? planilha { get; set; }
        public string? descricao_completa { get; set; }
        public double? cubagem { get; set; }
        public double? qtde_expedida {  get; set; }
    }
}
