﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[PrimaryKey("Planilha", "RequisicaoExpedicao")]
[Table("tbl_planilha_controlados", Schema = "expedicao")]
public partial class TblPlanilhaControlados
{
    [Key]
    [Column("planilha")]
    [StringLength(30)]
    public string Planilha { get; set; }

    [Key]
    [Column("requisicao_expedicao")]
    [StringLength(1)]
    public string RequisicaoExpedicao { get; set; }
}