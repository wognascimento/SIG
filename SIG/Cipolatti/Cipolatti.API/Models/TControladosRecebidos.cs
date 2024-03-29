﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[PrimaryKey("IdAprovado", "Codcompladicional")]
[Table("t_controlados_recebidos", Schema = "expedicao")]
public partial class TControladosRecebidos
{
    [Key]
    [Column("id_aprovado")]
    public int IdAprovado { get; set; }

    [Key]
    [Column("codcompladicional")]
    public int Codcompladicional { get; set; }

    [Column("qtd")]
    public float? Qtd { get; set; }

    [Column("atualizado_por")]
    [StringLength(30)]
    public string AtualizadoPor { get; set; }

    [Column("atualizado_em", TypeName = "timestamp without time zone")]
    public DateTime? AtualizadoEm { get; set; }

    [Column("cancelar_cobraca")]
    [StringLength(5)]
    public string CancelarCobraca { get; set; }

    [Column("justificativa")]
    public string Justificativa { get; set; }

    [Column("entrada_estoque")]
    [StringLength(5)]
    public string EntradaEstoque { get; set; }

    [Column("entrada_estoque_por")]
    [StringLength(30)]
    public string EntradaEstoquePor { get; set; }

    [Column("entrada_estoque_em", TypeName = "timestamp without time zone")]
    public DateTime? EntradaEstoqueEm { get; set; }
}