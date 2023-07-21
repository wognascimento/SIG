﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

/// <summary>
/// Transferencia entre galpões -&gt; scanner descarregado em taboão
/// </summary>
[Table("t_conf_carga_jac", Schema = "expedicao")]
public partial class TConfCargaJac
{
    [Key]
    [Column("barcode")]
    [StringLength(16)]
    public string Barcode { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Column("resp")]
    [StringLength(20)]
    public string Resp { get; set; }

    [Column("placa")]
    [StringLength(10)]
    public string Placa { get; set; }

    [Column("enviado")]
    [MaxLength(1)]
    public char? Enviado { get; set; }

    [Column("viagem")]
    public int? Viagem { get; set; }

    [Column("inserido_por")]
    [StringLength(30)]
    public string InseridoPor { get; set; }

    [Column("inserido_em", TypeName = "timestamp without time zone")]
    public DateTime? InseridoEm { get; set; }

    [Column("nf")]
    [StringLength(50)]
    public string Nf { get; set; }
}