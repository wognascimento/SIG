using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Producao
{
    [Keyless]
    [Table("view_controle_planilha_grupo", Schema = "producao")]
    public class ControlePlanilhaGrupo
    {
        public string? data_de_expedicao { get; set; }
        public string? id { get; set; }
        public string? fechamento_shopp { get; set; }
        public string? revisado { get; set; }
        public string? confirmado { get; set; }
        public string? justificativa { get; set; }
        public string? produto_novo { get; set; }
        public string? local_producao { get; set; }
        public string? planilha { get; set; }
        public string? sigla { get; set; }
        public string? tema { get; set; }
        public string? local_shoppings { get; set; }
        public string? descricao { get; set; }
        public string? descricao_adicional { get; set; }
        public string? qtd_complemento { get; set; }
        public string? qtd_detalhe { get; set; }
        public string? qtd_exp { get; set; }
        public string? complementoadicional { get; set; }
        public string? unidade { get; set; }
        public string? producao { get; set; }
        public string? supermercado { get; set; }
        public string? enviado_baia { get; set; }
        public string? obs_planilheiro { get; set; }
        public string? req { get; set; }
        public string? os { get; set; }
        public string? transf { get; set; }
        public string? orient_montagem { get; set; }
        public string? obs { get; set; }
        public string? codcompl { get; set; }
        public string? coddetalhescompl { get; set; }
        public string? item { get; set; }
        public string? resp_cenas { get; set; }
        public string? resp_trilha { get; set; }
        public string? num_os_produto { get; set; }
        public string? caminhao { get; set; }
        public string? agrupamento { get; set; }
        public string? resp_estruturas { get; set; }
        public string? tamanho_construcao { get; set; }
        public string? resp_prod { get; set; }
    }
}
