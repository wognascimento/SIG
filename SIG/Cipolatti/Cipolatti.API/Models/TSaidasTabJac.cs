﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[Table("t_saidas_tab_jac", Schema = "expedicao")]
public partial class TSaidasTabJac
{
    [Key]
    [Column("cod")]
    public int Cod { get; set; }

    [Column("data")]
    public DateTime? Data { get; set; }

    [Column("resp")]
    [StringLength(50)]
    public string Resp { get; set; }

    [Column("qtd")]
    public double? Qtd { get; set; }

    [Column("codvol")]
    [StringLength(50)]
    public string Codvol { get; set; }

    [Column("processado")]
    [StringLength(5)]
    public string Processado { get; set; }

    [Column("modelo_cx")]
    [StringLength(50)]
    public string ModeloCx { get; set; }

    [Column("medidas")]
    [StringLength(50)]
    public string Medidas { get; set; }

    [Column("impresso")]
    [StringLength(5)]
    public string Impresso { get; set; }

    [Column("vol_exp")]
    public int? VolExp { get; set; }

    [Column("vol_tot_exp")]
    public int? VolTotExp { get; set; }

    [Column("l")]
    public float? L { get; set; }

    [Column("h")]
    public float? H { get; set; }

    [Column("p")]
    public float? P { get; set; }

    [Column("peso_bruto")]
    public float? PesoBruto { get; set; }

    [Column("peso_liquido")]
    public float? PesoLiquido { get; set; }

    [Column("codcompladicional")]
    public int Codcompladicional { get; set; }

    [Column("codtransf")]
    public int? Codtransf { get; set; }

    [Column("inserido_por")]
    [StringLength(30)]
    public string InseridoPor { get; set; }

    [Column("inserido_em", TypeName = "timestamp without time zone")]
    public DateTime? InseridoEm { get; set; }

    [Required]
    [Column("operacao")]
    [StringLength(50)]
    public string Operacao { get; set; }
}