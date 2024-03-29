﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

[Table("tbl_enderecamento_galpao", Schema = "expedicao")]
[Index("Endereco", Name = "tbl_enderecamento_galpao_endereco_key", IsUnique = true)]
public partial class TblEnderecamentoGalpao
{
    [Required]
    [Column("endereco")]
    [StringLength(30)]
    public string Endereco { get; set; }

    [Key]
    [Column("id_endereco")]
    public int IdEndereco { get; set; }

    public virtual ICollection<TblControleBaiaEnderecamento> TblControleBaiaEnderecamento { get; set; } = new List<TblControleBaiaEnderecamento>();
}