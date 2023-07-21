﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[Table("tbl_os_exp", Schema = "expedicao")]
public partial class TblOsExp
{
    [Column("antigo")]
    public int? Antigo { get; set; }

    [Key]
    [Column("n_os_desbaiamento")]
    public int NOsDesbaiamento { get; set; }

    [Required]
    [Column("codvol")]
    [StringLength(100)]
    public string Codvol { get; set; }

    [Column("data")]
    public DateTime? Data { get; set; }

    [Column("resp")]
    [StringLength(30)]
    public string Resp { get; set; }

    [Required]
    [Column("setor")]
    [StringLength(30)]
    public string Setor { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Required]
    [Column("obs")]
    public string Obs { get; set; }

    [Column("coddetalhescompl")]
    public int Coddetalhescompl { get; set; }

    [Column("solicitante")]
    [StringLength(30)]
    public string Solicitante { get; set; }

    [Column("local_shopp")]
    [StringLength(30)]
    public string LocalShopp { get; set; }

    [Column("inserido_por")]
    [StringLength(30)]
    public string InseridoPor { get; set; }

    [Column("inserido_em", TypeName = "timestamp without time zone")]
    public DateTime? InseridoEm { get; set; }
}