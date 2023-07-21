﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[Table("t_caixas", Schema = "expedicao")]
[Index("NomeCaixa", Name = "t_caixas_nome_caixa_key", IsUnique = true)]
public partial class TCaixas
{
    [Key]
    [Column("cod_caixa")]
    public int CodCaixa { get; set; }

    [Required]
    [Column("nome_caixa")]
    [StringLength(22)]
    public string NomeCaixa { get; set; }

    [Column("setor")]
    [StringLength(30)]
    public string Setor { get; set; }

    [Column("sigla")]
    [StringLength(15)]
    public string Sigla { get; set; }

    [Column("sequencia")]
    public int? Sequencia { get; set; }

    [Column("inserido_por")]
    [StringLength(30)]
    public string InseridoPor { get; set; }

    [Column("inserido_em", TypeName = "timestamp without time zone")]
    public DateTime? InseridoEm { get; set; }

    [Column("impresso")]
    [MaxLength(1)]
    public char? Impresso { get; set; }

    [Column("producao")]
    [StringLength(7)]
    public string Producao { get; set; }

    public virtual ICollection<TExped> TExped { get; set; } = new List<TExped>();
}