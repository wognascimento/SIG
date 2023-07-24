﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[Table("tbl_controle_baia_enderecamento", Schema = "expedicao")]
[Index("SiglaServ", "ItemMemorial", "Endereco", Name = "tbl_controle_baia_enderecamen_sigla_serv_item_memorial_ende_key", IsUnique = true)]
public partial class TblControleBaiaEnderecamento
{
    [Column("sigla_serv")]
    [StringLength(30)]
    public string SiglaServ { get; set; }

    [Column("baia_caminhao")]
    [StringLength(30)]
    public string BaiaCaminhao { get; set; }

    [Column("endereco")]
    [StringLength(30)]
    public string Endereco { get; set; }

    [Key]
    [Column("id_controle")]
    public int IdControle { get; set; }

    [Column("item_memorial")]
    [StringLength(30)]
    public string ItemMemorial { get; set; }

    [Column("id_aprovado")]
    public int? IdAprovado { get; set; }

    [Column("inserido_por")]
    [StringLength(30)]
    public string InseridoPor { get; set; }

    [Column("inserido_em", TypeName = "timestamp without time zone")]
    public DateTime? InseridoEm { get; set; }

    [Column("alterado_por")]
    [StringLength(30)]
    public string AlteradoPor { get; set; }

    [Column("alterado_em", TypeName = "timestamp without time zone")]
    public DateTime? AlteradoEm { get; set; }

    public virtual TblEnderecamentoGalpao EnderecoNavigation { get; set; }
}