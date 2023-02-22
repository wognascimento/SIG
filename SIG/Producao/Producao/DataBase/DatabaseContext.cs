using Microsoft.EntityFrameworkCore;
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
        public DbSet<SetorProducaoModel> SetorProducaos { get; set; }
        public DbSet<ProdutoOsModel> ProdutoOs { get; set; }
        public DbSet<ProdutoServicoModel> ProdutoServicos { get; set; }   
        public DbSet<RequisicaoModel> Requisicoes { get; set; }
        public DbSet<DetalheRequisicaoModel> RequisicaoDetalhes { get; set; }
        public DbSet<QryRequisicaoDetalheModel> QryRequisicaoDetalhes { get; set; }
        public DbSet<QryDescricao> Descricoes { get; set; }
        public DbSet<ControleMemorialModel> ControleMemorials { get; set; }
        public DbSet<RevisorModel> Revisores { get; set; }
        public DbSet<EtiquetaCheckListModel> EtiquetaCheckLists { get; set; }
        public DbSet<EtiquetaProducaoModel> EtiquetaProducaos { get; set; }
        public DbSet<EtiquetaEmitidaModel> EtiquetaEmitidas { get; set; }
        public DbSet<QryModeloModel> qryModelos { get; set; }
        public DbSet<ModeloModel> Modelos { get; set; }
        public DbSet<QryReceitaDetalheCriadoModel> qryReceitas  { get; set; }
        public DbSet<ModeloReceitaModel> ReceitaModelos  { get; set; }
        public DbSet<ModeloReceitaAnoAnterior> ModelosAnoAnterior { get; set; }
        public DbSet<ControleModeloBaixa> ModelosControle { get; set; }
        public DbSet<HistoricoModelo> HistoricosModelo { get; set; }
        public DbSet<TemaModel> Temas { get; set; }
        
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
