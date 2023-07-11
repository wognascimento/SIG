using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HT.DataBase.Model
{
    [Keyless]
    [Table("qry_funcionario_ativos", Schema = "ht")]
    public class FuncionarioAtivoModel
    {
        public long? codfun { get; set; }
        public string? barcode { get; set; }
        public string? nome_apelido { get; set; }
        public string? setor { get; set; }
        public DateTime? data_admissao { get; set; }
        public DateTime? data_demissao { get; set; }
        public string? ativo { get; set; }
    }
}
