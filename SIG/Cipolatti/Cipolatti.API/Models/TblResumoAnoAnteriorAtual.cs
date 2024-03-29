﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[Table("tbl_resumo_ano_anterior_atual", Schema = "expedicao")]
public partial class TblResumoAnoAnteriorAtual
{
    [Required]
    [Column("sigla_serv")]
    [StringLength(30)]
    public string SiglaServ { get; set; }

    [Column("data_estimativa")]
    public DateOnly DataEstimativa { get; set; }

    [Column("m3_estimado")]
    public float? M3Estimado { get; set; }

    [Key]
    [Column("cod_linha")]
    public int CodLinha { get; set; }

    [Column("observacao")]
    public string Observacao { get; set; }

    [Column("inserido_por")]
    [StringLength(20)]
    public string InseridoPor { get; set; }

    [Column("inserido_data", TypeName = "timestamp without time zone")]
    public DateTime? InseridoData { get; set; }
}