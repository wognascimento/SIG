﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

/// <summary>
/// Conferencia envio entre galpões
/// </summary>
[Table("t_conf_envio_galpoes_tab", Schema = "expedicao")]
public partial class TConfEnvioGalpoesTab
{
    [Key]
    [Column("barcode")]
    [StringLength(15)]
    public string Barcode { get; set; }

    [Required]
    [Column("resp")]
    [StringLength(30)]
    public string Resp { get; set; }

    [Required]
    [Column("local")]
    [StringLength(15)]
    public string Local { get; set; }

    [Column("dataconfere")]
    public DateTime? Dataconfere { get; set; }

    [Column("inserido_por")]
    [StringLength(30)]
    public string InseridoPor { get; set; }

    [Column("inserido_em", TypeName = "timestamp without time zone")]
    public DateTime? InseridoEm { get; set; }
}