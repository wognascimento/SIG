﻿using Microsoft.EntityFrameworkCore;
using Producao.DataBase.Model;
using System;

namespace Producao
{
    public class DatabaseContext : DbContext
    {
        private DataBaseSettings BaseSettings  = DataBaseSettings.Instance;
        public DbSet<RelplanModel> Relplans { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<TabelaDescAdicionalModel> DescAdicionais { get; set; }
        public DbSet<TblComplementoAdicionalModel> ComplementoAdicionais { get; set; }
        public DbSet<AprovadoModel> Aprovados { get; set; }
        public DbSet<SiglaChkListModel> Siglas { get; set; }
        public DbSet<ComplementoCheckListModel> ComplementoCheckLists { get; set; }
        public DbSet<QryCheckListGeralModel> CheckListGerals { get; set; }
        public DbSet<DetalhesComplemento> DetalhesComplementos { get; set; }
        public DbSet<QryCheckListGeralComplementoModel> CheckListGeralComplementos { get; set; }
        public DbSet<ChkGeralRelatorioModel> ChkGeralRelatorios { get; set; }
        public DbSet<PendenciaProducaoModel> PendenciaProducaos { get; set; }
        public DbSet<ControlePlanilhaGrupoModel> ControlePlanilhaGrupos { get; set; }
        public DbSet<SetorProducaoModel> SetorProducaos { get; set; }
        public DbSet<ObsOsModel> ObsOs { get; set; }
        public DbSet<TblServicoModel> tblServicos { get; set; }
        public DbSet<TblTipoOs> tblTipoOs { get; set; }
        public DbSet<ProdutoOsModel> ProdutoOs { get; set; }
        public DbSet<ProdutoServicoModel> ProdutoServicos { get; set; }
        public DbSet<OrdemServicoAbertaEmissaoAgrupadoModel> OrdemServicoAbertas { get; set; }
        public DbSet<OrdemServicoEmissaoAbertaForm> OrdemServicoEmissaoAbertas { get; set; }
        public DbSet<AlteraSolicitacaoOsProducao> AlteraSolicitacaoOsProducaos { get; set; }
        public DbSet<RequisicaoModel> Requisicoes { get; set; }
        public DbSet<DetalheRequisicaoModel> RequisicaoDetalhes { get; set; }
        public DbSet<RequisicaoReceitaModel> RequisicaoReceitas { get; set; }
        public DbSet<GeralRequisicaoProducaoModel> RequisicoesProducao { get; set; }
        public DbSet<QryRequisicaoDetalheModel> QryRequisicaoDetalhes { get; set; }
        public DbSet<ReqDetalhesModel> ReqDetalhes { get; set; }
        public DbSet<QryDescricao> Descricoes { get; set; }
        public DbSet<ControleMemorialModel> ControleMemorials { get; set; }
        public DbSet<RevisorModel> Revisores { get; set; }
        public DbSet<EtiquetaCheckListModel> EtiquetaCheckLists { get; set; }
        public DbSet<EtiquetaProducaoModel> EtiquetaProducaos { get; set; }
        public DbSet<EtiquetaEmitidaModel> EtiquetaEmitidas { get; set; }
        public DbSet<QryModeloModel> qryModelos { get; set; }
        public DbSet<ModeloModel> Modelos { get; set; }
        public DbSet<ModeloFiadaModel> ModelosFiada { get; set; }
        public DbSet<QryReceitaDetalheCriadoModel> qryReceitas  { get; set; }
        public DbSet<ModeloReceitaModel> ReceitaModelos  { get; set; }
        public DbSet<ModeloReceitaAnoAnterior> ModelosAnoAnterior { get; set; }
        public DbSet<ControleModeloBaixa> ModelosControle { get; set; }
        public DbSet<ExportEnfeitesInfClienteModel> exportEnfeitesInfClientes { get; set; }
        public DbSet<HistoricoModelo> HistoricosModelo { get; set; }
        public DbSet<HistoricoSetorModel> HistoricosSetor { get; set; }
        public DbSet<DistribuicaoPAModel> DistribuicaoPAs { get; set; }
        public DbSet<OsEmissaoProducaoImprimirModel> ImprimirOsS { get; set; }
        public DbSet<DetalhesModeloModel> DetalhesModelo { get; set; }
        public DbSet<StatusChkGeralCentralModel> statusChkGeralCentrals { get; set; }
        public DbSet<ModeloTabelaPAModel> TabelaPAs { get; set; }
        public DbSet<ModeloTabelaConversaoModel> TabelaConversoes { get; set; }
        public DbSet<ModeloControleOsModel> modeloControleOs { get; set; }
        public DbSet<ClasseSolicitCompra> ClasseSolicitCompras { get; set; }
        public DbSet<UnidadeModel> Unidades { get; set; }
        public DbSet<TemaModel> Temas { get; set; }
        public DbSet<FamiliaProdModel> FamiliaProds { get; set; }
        public DbSet<HistoricoModeloCompletaModel> HistoricoModeloCompletas { get; set; }
        
        static DatabaseContext() => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                $"host={BaseSettings.Host};" +
                $"user id={BaseSettings.Username};" +
                $"password={BaseSettings.Password};" +
                $"database={BaseSettings.Database};" +
                $"Pooling=false;" +
                $"Timeout=300;" +
                $"CommandTimeout=300;"
                );
        }
    }
}
