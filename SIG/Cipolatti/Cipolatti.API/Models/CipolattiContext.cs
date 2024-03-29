﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cipolatti.API.Models;

public partial class CipolattiContext : DbContext
{
    public CipolattiContext(DbContextOptions<CipolattiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<QryAprovados> QryAprovados { get; set; }

    public virtual DbSet<QryEnderecamentoGalpao> QryEnderecamentoGalpao { get; set; }

    public virtual DbSet<QryLookup> QryLookup { get; set; }

    public virtual DbSet<TCaixas> TCaixas { get; set; }

    public virtual DbSet<TCaixasJactab> TCaixasJactab { get; set; }

    public virtual DbSet<TCaixasTabjac> TCaixasTabjac { get; set; }

    public virtual DbSet<TConfCargaGeral> TConfCargaGeral { get; set; }

    public virtual DbSet<TConfCargaGeralColetor> TConfCargaGeralColetor { get; set; }

    public virtual DbSet<TConfCargaJac> TConfCargaJac { get; set; }

    public virtual DbSet<TConfCargaTab> TConfCargaTab { get; set; }

    public virtual DbSet<TConfEnvioGalpoesJac> TConfEnvioGalpoesJac { get; set; }

    public virtual DbSet<TConfEnvioGalpoesTab> TConfEnvioGalpoesTab { get; set; }

    public virtual DbSet<TConferentes> TConferentes { get; set; }

    public virtual DbSet<TConfirmaBaiaLocal> TConfirmaBaiaLocal { get; set; }

    public virtual DbSet<TControladosRecebidos> TControladosRecebidos { get; set; }

    public virtual DbSet<TControleBaia> TControleBaia { get; set; }

    public virtual DbSet<TControleBaiaLocal> TControleBaiaLocal { get; set; }

    public virtual DbSet<TEnvioBaiaGalpoesJac> TEnvioBaiaGalpoesJac { get; set; }

    public virtual DbSet<TEnvioBaiaGalpoesTab> TEnvioBaiaGalpoesTab { get; set; }

    public virtual DbSet<TExped> TExped { get; set; }

    public virtual DbSet<TPreConferencia> TPreConferencia { get; set; }

    public virtual DbSet<TRomaneio> TRomaneio { get; set; }

    public virtual DbSet<TSaidasJacTab> TSaidasJacTab { get; set; }

    public virtual DbSet<TSaidasTabJac> TSaidasTabJac { get; set; }

    public virtual DbSet<TTransfLookupConfereJac> TTransfLookupConfereJac { get; set; }

    public virtual DbSet<TTransfLookupConfereTab> TTransfLookupConfereTab { get; set; }

    public virtual DbSet<TTransfLookupJac> TTransfLookupJac { get; set; }

    public virtual DbSet<TTransfLookupTab> TTransfLookupTab { get; set; }

    public virtual DbSet<TblControleBaiaEnderecamento> TblControleBaiaEnderecamento { get; set; }

    public virtual DbSet<TblControleCaixaSetor> TblControleCaixaSetor { get; set; }

    public virtual DbSet<TblEnderecamentoGalpao> TblEnderecamentoGalpao { get; set; }

    public virtual DbSet<TblHistoricoCubagemPonderada> TblHistoricoCubagemPonderada { get; set; }

    public virtual DbSet<TblMateriaisControladosRetorno> TblMateriaisControladosRetorno { get; set; }

    public virtual DbSet<TblMediaCubagemProduto> TblMediaCubagemProduto { get; set; }

    public virtual DbSet<TblMovimentacaoVolumeShopping> TblMovimentacaoVolumeShopping { get; set; }

    public virtual DbSet<TblOsExp> TblOsExp { get; set; }

    public virtual DbSet<TblPlanilhaControlados> TblPlanilhaControlados { get; set; }

    public virtual DbSet<TblRespControlado> TblRespControlado { get; set; }

    public virtual DbSet<TblResumoAnoAnteriorAtual> TblResumoAnoAnteriorAtual { get; set; }

    public virtual DbSet<TblRetornoCliente> TblRetornoCliente { get; set; }

    public virtual DbSet<TblVolumeControlado> TblVolumeControlado { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pgagent", "pgagent")
            .HasPostgresExtension("postgres_fdw")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<QryAprovados>(entity =>
        {
            entity.ToView("qry_aprovados", "producao");
        });

        modelBuilder.Entity<QryEnderecamentoGalpao>(entity =>
        {
            entity.ToView("qry_enderecamento_galpao", "expedicao");
        });

        modelBuilder.Entity<QryLookup>(entity =>
        {
            entity.ToView("qry_lookup", "expedicao");
        });

        modelBuilder.Entity<TCaixas>(entity =>
        {
            entity.HasKey(e => e.CodCaixa).HasName("t_caixas_pkey");
        });

        modelBuilder.Entity<TCaixasJactab>(entity =>
        {
            entity.HasKey(e => e.CodCaixa).HasName("t_caixas_jactab_pkey");
        });

        modelBuilder.Entity<TCaixasTabjac>(entity =>
        {
            entity.HasKey(e => e.CodCaixa).HasName("t_caixas_tabjac_pkey");
        });

        modelBuilder.Entity<TConfCargaGeral>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_conf_carga_geral_pkey");

            entity.Property(e => e.Entradasistema).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TConfCargaGeralColetor>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_conf_carga_geral_coletor_pkey");

            entity.Property(e => e.Entradasistema).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TConfCargaJac>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_conf_carga_jac_pkey");

            entity.ToTable("t_conf_carga_jac", "expedicao", tb => tb.HasComment("Transferencia entre galpões -> scanner descarregado em taboão"));
        });

        modelBuilder.Entity<TConfCargaTab>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_conf_carga_tab_pkey");

            entity.ToTable("t_conf_carga_tab", "expedicao", tb => tb.HasComment("Transferencia entre galpões -> scanner descarregado em jacareí"));
        });

        modelBuilder.Entity<TConfEnvioGalpoesJac>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_conf_envio_galpoes_jac_pkey");

            entity.ToTable("t_conf_envio_galpoes_jac", "expedicao", tb => tb.HasComment("Conferencia envio entre galpões"));

            entity.Property(e => e.Dataconfere).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TConfEnvioGalpoesTab>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_conf_envio_galpoes_tab_pkey");

            entity.ToTable("t_conf_envio_galpoes_tab", "expedicao", tb => tb.HasComment("Conferencia envio entre galpões"));

            entity.Property(e => e.Dataconfere).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TConferentes>(entity =>
        {
            entity.HasKey(e => e.IdConferente).HasName("t_conferentes_pkey");
        });

        modelBuilder.Entity<TConfirmaBaiaLocal>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_confirma_baia_local_pkey");
        });

        modelBuilder.Entity<TControladosRecebidos>(entity =>
        {
            entity.HasKey(e => new { e.IdAprovado, e.Codcompladicional }).HasName("t_controlados_recebidos_pkey");

            entity.Property(e => e.CancelarCobraca).HasDefaultValueSql("0");
            entity.Property(e => e.EntradaEstoque).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<TControleBaia>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_controle_baia_pkey");

            entity.Property(e => e.Data).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TControleBaiaLocal>(entity =>
        {
            entity.HasKey(e => e.Sigla).HasName("t_controle_baia_local_pkey");
        });

        modelBuilder.Entity<TEnvioBaiaGalpoesJac>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_envio_baia_galpoes_jac_pkey");

            entity.ToTable("t_envio_baia_galpoes_jac", "expedicao", tb => tb.HasComment("Transferencia shooping"));

            entity.Property(e => e.Enviado).IsFixedLength();
        });

        modelBuilder.Entity<TEnvioBaiaGalpoesTab>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_envio_baia_galpoes_tab_pkey");

            entity.ToTable("t_envio_baia_galpoes_tab", "expedicao", tb => tb.HasComment("Transferencia shooping"));

            entity.Property(e => e.Enviado).IsFixedLength();
        });

        modelBuilder.Entity<TExped>(entity =>
        {
            entity.HasKey(e => e.Codexped).HasName("t_exped_pkey");

            entity.Property(e => e.BaiaVirtual)
                .HasDefaultValueSql("0")
                .IsFixedLength();
            entity.Property(e => e.NfEmitida).HasDefaultValueSql("false");
            entity.Property(e => e.Pb).HasDefaultValueSql("0");
            entity.Property(e => e.Pl).HasDefaultValueSql("0");

            entity.HasOne(d => d.CodvolNavigation).WithMany(p => p.TExped)
                .HasPrincipalKey(p => p.NomeCaixa)
                .HasForeignKey(d => d.Codvol)
                .HasConstraintName("t_exped_codvol_fkey");
        });

        modelBuilder.Entity<TPreConferencia>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("tbl_preconf_pkey");

            entity.Property(e => e.Entradasistema).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TRomaneio>(entity =>
        {
            entity.HasKey(e => e.CodRomaneiro).HasName("t_romaneio_pkey");

            entity.Property(e => e.BauAltura).HasDefaultValueSql("0");
            entity.Property(e => e.BauLargura).HasDefaultValueSql("0");
            entity.Property(e => e.BauProfundidade).HasDefaultValueSql("0");
            entity.Property(e => e.BauSoba).HasDefaultValueSql("0");
            entity.Property(e => e.IncluidoData).HasDefaultValueSql("(now())::timestamp(0) with time zone");
            entity.Property(e => e.IncluidoPor).HasDefaultValueSql("CURRENT_USER");
            entity.Property(e => e.M3Carregado).HasDefaultValueSql("0");
            entity.Property(e => e.M3Portaria).HasDefaultValueSql("0");
            entity.Property(e => e.PlacaEstado).IsFixedLength();
        });

        modelBuilder.Entity<TSaidasJacTab>(entity =>
        {
            entity.HasKey(e => e.Cod).HasName("t_saidas_jac_tab_pkey");

            entity.Property(e => e.Impresso).IsFixedLength();
            entity.Property(e => e.Processado).IsFixedLength();
        });

        modelBuilder.Entity<TSaidasTabJac>(entity =>
        {
            entity.HasKey(e => e.Cod).HasName("t_saidas_tab_jac_pkey");

            entity.Property(e => e.Impresso).IsFixedLength();
            entity.Property(e => e.Processado).IsFixedLength();
        });

        modelBuilder.Entity<TTransfLookupConfereJac>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_transf_lookup_confere_jac_pkey");
        });

        modelBuilder.Entity<TTransfLookupConfereTab>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_transf_lookup_confere_tab_pkey");
        });

        modelBuilder.Entity<TTransfLookupJac>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_transf_lookup_jac_pkey");
        });

        modelBuilder.Entity<TTransfLookupTab>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("t_transf_lookup_tab_pkey");
        });

        modelBuilder.Entity<TblControleBaiaEnderecamento>(entity =>
        {
            entity.HasKey(e => e.IdControle).HasName("tbl_controle_baia_enderecamento_pkey");

            entity.Property(e => e.BaiaCaminhao).HasDefaultValueSql("1");
            entity.Property(e => e.InseridoEm).HasDefaultValueSql("(now())::timestamp(0) with time zone");
            entity.Property(e => e.InseridoPor).HasDefaultValueSql("\"current_user\"()");

            entity.HasOne(d => d.EnderecoNavigation).WithMany(p => p.TblControleBaiaEnderecamento)
                .HasPrincipalKey(p => p.Endereco)
                .HasForeignKey(d => d.Endereco)
                .HasConstraintName("tbl_controle_baia_enderecamento_endereco_fkey");
        });

        modelBuilder.Entity<TblControleCaixaSetor>(entity =>
        {
            entity.HasKey(e => e.CodLinhaCaixaSetor).HasName("tbl_controle_caixa_setor_pkey");
        });

        modelBuilder.Entity<TblEnderecamentoGalpao>(entity =>
        {
            entity.HasKey(e => e.IdEndereco).HasName("tbl_enderecamento_galpao_pkey");
        });

        modelBuilder.Entity<TblHistoricoCubagemPonderada>(entity =>
        {
            entity.HasKey(e => e.IdLinhaPonderada).HasName("tbl_historico_cubagem_ponderada_pkey");
        });

        modelBuilder.Entity<TblMateriaisControladosRetorno>(entity =>
        {
            entity.HasKey(e => new { e.Sigla, e.Codcompladicional, e.DetalheReqCompl, e.Origem }).HasName("tbl_materiais_controlados_retorno_pkey");
        });

        modelBuilder.Entity<TblMediaCubagemProduto>(entity =>
        {
            entity.HasKey(e => e.Codcompladicional).HasName("tbl_media_cubagem_produto_pkey");

            entity.Property(e => e.Codcompladicional).ValueGeneratedNever();
            entity.Property(e => e.M3).HasDefaultValueSql("0");
        });

        modelBuilder.Entity<TblMovimentacaoVolumeShopping>(entity =>
        {
            entity.HasKey(e => e.IdLinhaInserida).HasName("tbl_movimentacao_volume_shopping_pkey");

            entity.Property(e => e.InseridoEm).HasDefaultValueSql("(now())::timestamp(0) with time zone");
        });

        modelBuilder.Entity<TblOsExp>(entity =>
        {
            entity.HasKey(e => e.NOsDesbaiamento).HasName("tbl_os_exp_pkey");

            entity.Property(e => e.Solicitante).IsFixedLength();
        });

        modelBuilder.Entity<TblPlanilhaControlados>(entity =>
        {
            entity.HasKey(e => new { e.Planilha, e.RequisicaoExpedicao }).HasName("tbl_planilha_controlados_pkey");
        });

        modelBuilder.Entity<TblRespControlado>(entity =>
        {
            entity.HasKey(e => new { e.Sigla, e.Nome }).HasName("tbl_resp_controlado_pkey");
        });

        modelBuilder.Entity<TblResumoAnoAnteriorAtual>(entity =>
        {
            entity.HasKey(e => e.CodLinha).HasName("tbl_resumo_ano_anterior_atual_pkey");
        });

        modelBuilder.Entity<TblRetornoCliente>(entity =>
        {
            entity.HasKey(e => e.RetornoClienteId).HasName("tbl_retorno_cliente_pkey");

            entity.Property(e => e.InseridoEm).HasDefaultValueSql("(now())::timestamp(0) with time zone");
            entity.Property(e => e.InseridoPor).HasDefaultValueSql("\"current_user\"()");
        });

        modelBuilder.Entity<TblVolumeControlado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tbl_volume_controlado_pkey");

            entity.Property(e => e.Recebido).HasDefaultValueSql("CURRENT_DATE");
        });
        modelBuilder.HasSequence("carregamento_itens_shopp_seq", "expedicao");
        modelBuilder.HasSequence("conceito_idconceito_seq", "comercial");
        modelBuilder.HasSequence("fluxo_id", "financeiro");
        modelBuilder.HasSequence("fluxo_linha_fluxo_seq", "financeiro");
        modelBuilder.HasSequence("fornecedores_idfornecedor_seq", "compras");
        modelBuilder.HasSequence("funcionarios_codfun_seq", "ht");
        modelBuilder.HasSequence("hibernate_sequence");
        modelBuilder.HasSequence("id_temporario", "equipe_externa").HasMin(0L);
        modelBuilder.HasSequence("idviewchklist", "equipe_externa").HasMin(0L);
        modelBuilder.HasSequence("idviewchklist", "producao").HasMin(0L);
        modelBuilder.HasSequence("itens_faltantes_id_seq", "expedicao");
        modelBuilder.HasSequence("pedidos_idpedido_seq", "compras").HasMin(0L);
        modelBuilder.HasSequence("pedidosdet_iddetpedido_seq", "compras").HasMin(0L);
        modelBuilder.HasSequence("produtos_codigo_seq", "producao");
        modelBuilder.HasSequence("proposta_base_preco_zefe_codbase_preco_seq", "comercial");
        modelBuilder.HasSequence("proposta_descricaocomercial_coddesccoml_seq", "comercial");
        modelBuilder.HasSequence("proposta_dimensaodescricaocomercial_coddimensao_seq", "comercial");
        modelBuilder.HasSequence("seq_gen_sequence")
            .StartsAt(50L)
            .IncrementsBy(50);
        modelBuilder.HasSequence("solicitacao_material_cod_solicitacao_seq", "compras")
            .StartsAt(4381L)
            .HasMin(0L);
        modelBuilder.HasSequence("solicitacao_material_itens_cod_item_seq", "compras").HasMin(0L);
        modelBuilder.HasSequence("solicitacao_solicitantes_cod_solicitante_seq", "compras");
        modelBuilder.HasSequence("t_caixas_cod_caixa_seq", "expedicao");
        modelBuilder.HasSequence("t_desp_funcionario_cod_func_seq", "operacional");
        modelBuilder.HasSequence("t_detalhes_req_cod_det_req_seq", "producao").HasMin(0L);
        modelBuilder.HasSequence("tabela_desc_adicional_coduniadicional_seq", "producao");
        modelBuilder.HasSequence("tbl_aderecamento_id_aderecamento_seq", "projetos");
        modelBuilder.HasSequence("tbl_adereco_id_adereco_seq", "projetos").StartsAt(6243L);
        modelBuilder.HasSequence("tbl_agendavt_id_seq", "projetos");
        modelBuilder.HasSequence("tbl_banco_hora_cod_linha_banco_horas_seq", "rh");
        modelBuilder.HasSequence("tbl_condicoes_pagto_id_cond_pagamento_seq", "compras");
        modelBuilder.HasSequence("tbl_produto_os_num_os_produto_seq", "producao").HasMin(0L);
        modelBuilder.HasSequence("tbl_produtos_servico_num_os_servico_seq", "producao").HasMin(0L);
        modelBuilder.HasSequence("tbl_retencao_clientes_id_cliente_retencao_seq", "financeiro").StartsAt(304L);
        modelBuilder.HasSequence("tbl_retencao_faturamento_id_retencao_faturamento_seq", "financeiro").StartsAt(1763L);
        modelBuilder.HasSequence("tbl_retencoes_pagamentos_id_retencao_seq", "financeiro").StartsAt(395L);
        modelBuilder.HasSequence("tbl_setor_codigo_setor_seq", "producao");
        modelBuilder.HasSequence("tbl_turno_catraca_cod_linha_turno_seq", "ht");
        modelBuilder.HasSequence("tblcomplementoadicional_codcompladicional_seq", "producao");
        modelBuilder.HasSequence("tblCorrigirCompras_Controle_seq", "compras");
        modelBuilder.HasSequence("tblcotacao_codigo_cotacao_seq", "compras");
        modelBuilder.HasSequence("tblcotacao_detalhes_codigo_detalhes_cotacao_seq", "compras");
        modelBuilder.HasSequence("tblcotacao_fornecedor_codigo_cotacao_fornecedor_seq", "compras");
        modelBuilder.HasSequence("tbldetalhesolicit_coddetalhe_seq", "compras");
        modelBuilder.HasSequence("tblDetPedido_Cod_Det_Pedido_seq", "compras");
        modelBuilder.HasSequence("tblEmpresa_codigo_seq", "compras");
        modelBuilder.HasSequence("tblequipesext_id_seq", "equipe_externa");
        modelBuilder.HasSequence("tblFamiliaProd_CodigoFamilia_seq", "compras");
        modelBuilder.HasSequence("tblFornecedor_Controle_seq", "compras");
        modelBuilder.HasSequence("tblfornecedor_forn_codigo_seq", "compras").HasMin(0L);
        modelBuilder.HasSequence("tblPedidos_Cod_Pedido_seq", "compras");
        modelBuilder.HasSequence<int>("tblprodutofornecedor_id_seq", "compras");
        modelBuilder.HasSequence("tblProdutos_Cod_Produto_seq", "compras");
        modelBuilder.HasSequence("tblRecebeAlmox_Cod_Receb_seq", "compras");
        modelBuilder.HasSequence<int>("tblsetores_codigo_setor_seq", "rh");
        modelBuilder.HasSequence<int>("tblsetorescidades_codigosetorcidade_seq", "rh");
        modelBuilder.HasSequence<int>("tblsetoreslocais_codigosetorlocal_seq", "rh");
        modelBuilder.HasSequence("tblSolicitacao_CodSolicitacao_seq", "compras");
        modelBuilder.HasSequence("tblsolicitacoes_codsolicit_seq", "compras");
        modelBuilder.HasSequence<int>("tblsub_setores_codigo_sub_setor_seq", "rh");
        modelBuilder.HasSequence("temas_idtema_seq", "comercial");
        modelBuilder.HasSequence("temporario", "comercial").HasMin(0L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}