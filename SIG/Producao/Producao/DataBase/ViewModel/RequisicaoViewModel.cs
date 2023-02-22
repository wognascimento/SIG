using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Producao
{
    class RequisicaoViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<RelplanModel> _planilhas;
        public ObservableCollection<RelplanModel> Planilhas
        {
            get { return _planilhas; }
            set { _planilhas = value; RaisePropertyChanged("Planilhas"); }
        }
        private RelplanModel _planilha;
        public RelplanModel Planilha
        {
            get { return _planilha; }
            set { _planilha = value; RaisePropertyChanged("Planilha"); }
        }

        private ObservableCollection<ProdutoModel> _produtos;
        public ObservableCollection<ProdutoModel> Produtos
        {
            get { return _produtos; }
            set { _produtos = value; RaisePropertyChanged("Produtos"); }
        }
        private ProdutoModel _produto;
        public ProdutoModel Produto
        {
            get { return _produto; }
            set { _produto = value; RaisePropertyChanged("Produto"); }
        }

        private ObservableCollection<TabelaDescAdicionalModel> _descAdicionais;
        public ObservableCollection<TabelaDescAdicionalModel> DescAdicionais
        {
            get { return _descAdicionais; }
            set { _descAdicionais = value; RaisePropertyChanged("DescAdicionais"); }
        }
        private TabelaDescAdicionalModel _descAdicional;
        public TabelaDescAdicionalModel DescAdicional
        {
            get { return _descAdicional; }
            set { _descAdicional = value; RaisePropertyChanged("DescAdicional"); }
        }

        private ObservableCollection<TblComplementoAdicionalModel> _compleAdicionais;
        public ObservableCollection<TblComplementoAdicionalModel> CompleAdicionais
        {
            get { return _compleAdicionais; }
            set { _compleAdicionais = value; RaisePropertyChanged("CompleAdicionais"); }
        }
        private TblComplementoAdicionalModel _compledicional;
        public TblComplementoAdicionalModel Compledicional
        {
            get { return _compledicional; }
            set { _compledicional = value; RaisePropertyChanged("Compledicional"); }
        }

        private ProdutoServicoModel _produtoServico;
        public ProdutoServicoModel ProdutoServico
        {
            get { return _produtoServico; }
            set { _produtoServico = value; RaisePropertyChanged("ProdutoServico"); }
        }
        private ObservableCollection<ProdutoServicoModel> _produtoServicos;
        public ObservableCollection<ProdutoServicoModel> ProdutoServicos
        {
            get { return _produtoServicos; }
            set { _produtoServicos = value; RaisePropertyChanged("ProdutoServicos"); }
        }

        private RequisicaoModel _requisicao;
        public RequisicaoModel Requisicao
        {
            get { return _requisicao; }
            set { _requisicao = value; RaisePropertyChanged("Requisicao"); }
        }
        private ObservableCollection<RequisicaoModel> _requisicoes;
        public ObservableCollection<RequisicaoModel> Requisicoes
        {
            get { return _requisicoes; }
            set { _requisicoes = value; RaisePropertyChanged("Requisicoes"); }
        }

        private DetalheRequisicaoModel _requisicaoDetalhe;
        public DetalheRequisicaoModel RequisicaoDetalhe
        {
            get { return _requisicaoDetalhe; }
            set { _requisicaoDetalhe = value; RaisePropertyChanged("RequisicaoDetalhe"); }
        }
        private ObservableCollection<DetalheRequisicaoModel> _requisicaoDetalhes;
        public ObservableCollection<DetalheRequisicaoModel> RequisicaoDetalhes
        {
            get { return _requisicaoDetalhes; }
            set { _requisicaoDetalhes = value; RaisePropertyChanged("RequisicaoDetalhes"); }
        }

        private QryRequisicaoDetalheModel _qryRequisicaoDetalhe;
        public QryRequisicaoDetalheModel QryRequisicaoDetalhe
        {
            get { return _qryRequisicaoDetalhe; }
            set { _qryRequisicaoDetalhe = value; RaisePropertyChanged("QryRequisicaoDetalhe"); }
        }
        private ObservableCollection<QryRequisicaoDetalheModel> _qryRequisicaoDetalhes;
        public ObservableCollection<QryRequisicaoDetalheModel> QryRequisicaoDetalhes
        {
            get { return _qryRequisicaoDetalhes; }
            set { _qryRequisicaoDetalhes = value; RaisePropertyChanged("QryRequisicaoDetalhes"); }
        }

        private QryDescricao _descricao;
        public QryDescricao Descricao
        {
            get { return _descricao; }
            set { _descricao = value; RaisePropertyChanged("Descricao"); }
        }
        private ObservableCollection<QryDescricao> _descricoes;
        public ObservableCollection<QryDescricao> Descricoes
        {
            get { return _descricoes; }
            set { _descricoes = value; RaisePropertyChanged("Descricoes"); }
        }

        public RequisicaoViewModel()
        {
            try
            {
                Task.Run(async () => await GetPlanilhasAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task GetDescricaoAsync(long codcompladicional)
        {//e.FirstName.StartsWith(employeeName) || e.LastName.StartsWith(employeeName)
            try
            {
                using DatabaseContext db = new();
                Descricao = await db.Descricoes.Where(d => d.inativo.Equals("0") && d.codcompladicional == codcompladicional).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetPlanilhasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                Planilhas = new ObservableCollection<RelplanModel>(await db.Relplans.OrderBy(c => c.planilha).Where(c => c.ativo.Equals("1")).ToListAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetProdutosAsync()
        {
            try
            {
                Produtos = new ObservableCollection<ProdutoModel>();
                using DatabaseContext db = new();
                var data = await db.Produtos
                    .OrderBy(c => c.descricao)
                    .Where(c => c.planilha.Equals(Planilha.planilha))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                Produtos = new ObservableCollection<ProdutoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetDescAdicionaisAsync()
        {
            try
            {
                DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
                using DatabaseContext db = new();
                var data = await db.DescAdicionais
                    .OrderBy(c => c.descricao_adicional)
                    .Where(c => c.codigoproduto.Equals(Produto.codigo))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetCompleAdicionaisAsync()
        {
            try
            {
                CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>();
                using DatabaseContext db = new();
                var data = await db.ComplementoAdicionais
                    .OrderBy(c => c.complementoadicional)
                    .Where(c => c.coduniadicional.Equals(DescAdicional.coduniadicional))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetRequisicaoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                Requisicao = await db.Requisicoes.Where(r => r.num_os_servico == ProdutoServico.num_os_servico).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddProdutoRequisicaoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                db.Entry(RequisicaoDetalhe).State = RequisicaoDetalhe.cod_det_req == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetRequisicaoDetalhesAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.QryRequisicaoDetalhes.Where(r => r.num_requisicao == Requisicao.num_requisicao).ToListAsync();
                QryRequisicaoDetalhes = new ObservableCollection<QryRequisicaoDetalheModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
  
        }

    }
}
