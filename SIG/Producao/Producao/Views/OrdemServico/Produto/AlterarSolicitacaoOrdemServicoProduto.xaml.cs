using Microsoft.EntityFrameworkCore;
using Producao.Views.PopUp;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Producao.Views.OrdemServico.Produto
{
    /// <summary>
    /// Interação lógica para SolicitacaoOrdemServicoProduto.xam
    /// </summary>
    public partial class AlterarSolicitacaoOrdemServicoProduto : UserControl
    {
        public AlterarSolicitacaoOrdemServicoProduto()
        {
            InitializeComponent();
            DataContext = new AlterarSolicitacaoOrdemServicoProdutoViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //ObsOSs

            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                AlterarSolicitacaoOrdemServicoProdutoViewModel vm = (AlterarSolicitacaoOrdemServicoProdutoViewModel)DataContext;
                vm.ObsOSs = new ObservableCollection<ObsOsModel>();
                vm.Setores = await Task.Run(vm.GetSetorsAsync);
                vm.Siglas = await Task.Run(vm.GetSiglasAsync);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnBuscaProduto(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                try
                {
                    AlterarSolicitacaoOrdemServicoProdutoViewModel vm = (AlterarSolicitacaoOrdemServicoProdutoViewModel)DataContext;
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    string text = ((TextBox)sender).Text;
                    vm.OrdemServico = await Task.Run(() => vm.GetOrdemServicoAsync(long.Parse(text)));
                    if (vm.OrdemServico == null)
                    {
                        MessageBox.Show("O.S não encontrada", "Busca de produto");
                        Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                        return;
                    }
                    tbCodproduto.Text = vm.OrdemServico.cod_compl_adicional.ToString();
                    txtPlanilha.Text = vm.OrdemServico.planilha;
                    txtDescricao.Text = vm.OrdemServico.descricao;
                    txtDescricaoAdicional.Text = vm.OrdemServico.descricao_adicional;
                    txtComplementoAdicional.Text = vm.OrdemServico.complementoadicional;
                    txtQuantidade.Focus();
                    vm.ObsOSs = await Task.Run(() => vm.GetCaminhosOSAsync(long.Parse(text)));
                    

                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }
            }
        }

        private void caminhos_AddNewRowInitiating(object sender, AddNewRowInitiatingEventArgs e)
        {
            AlterarSolicitacaoOrdemServicoProdutoViewModel vm = (AlterarSolicitacaoOrdemServicoProdutoViewModel)DataContext;
            ((ObsOsModel)e.NewObject).num_os_produto = vm.OrdemServico?.num_os_produto;
            ((ObsOsModel)e.NewObject).cod_compl_adicional = vm.OrdemServico?.cod_compl_adicional;
        }

        private async void caminhos_RowValidated(object sender, RowValidatedEventArgs e)
        {
            var sfdatagrid = sender as SfDataGrid;
            AlterarSolicitacaoOrdemServicoProdutoViewModel vm = (AlterarSolicitacaoOrdemServicoProdutoViewModel)DataContext;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ObsOsModel data = (ObsOsModel)e.RowData;
                data.solicitado_por = Environment.UserName;
                data.solicitado_data = DateTime.Now;
                vm.ObsOs = await Task.Run(() => vm.SaveProdutoOsAsync(data));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                var toRemove = vm.ObsOSs.Where(x => x.cod_obs == null).ToList();
                foreach (var item in toRemove)
                    vm.ObsOSs.Remove(item);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private void caminhos_RowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {
            ObsOsModel rowData = (ObsOsModel)e.RowData;
            if (!rowData.num_os_produto.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("num_caminho", "Não foi criado O.S para incluir o(s) caminho(s).");
                e.ErrorMessages.Add("codigo_setor", "Não foi criado O.S para incluir o(s) caminho(s).");
                e.ErrorMessages.Add("orientacao_caminho", "Não foi criado O.S para incluir o(s) caminho(s).");
                e.ErrorMessages.Add("distribuir_os", "Não foi criado O.S para incluir o(s) caminho(s).");
                e.ErrorMessages.Add("cliente", "Não foi criado O.S para incluir o(s) caminho(s).");
            }
            else if (!rowData.num_caminho.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("num_caminho", "Informe a ordem do caminho da O.S.");
            }
            else if (!rowData.codigo_setor.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("codigo_setor", "Seleciona o Setor da O.S.");
            }
            else if (rowData.orientacao_caminho == "")
            {
                e.IsValid = false;
                e.ErrorMessages.Add("orientacao_caminho", "Informe uma orientação para o Setor.");
            }
            else if (rowData.orientacao_caminho == "")
            {
                e.IsValid = false;
                e.ErrorMessages.Add("distribuir_os", "Informe como será distribuida a O.S.");
            }
            else if (rowData.cliente == "")
            {
                e.IsValid = false;
                e.ErrorMessages.Add("cliente", "Informe o cliente da O.S.");
            }
        }

        private void caminhos_CurrentCellDropDownSelectionChanged(object sender, CurrentCellDropDownSelectionChangedEventArgs e)
        {
            int rowIndex = caminhos.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
            ObsOsModel record;

            if (rowIndex == -1)
                record = (ObsOsModel)caminhos.View.CurrentAddItem;
            //record = new();
            else
                record = (ObsOsModel)(caminhos.View.Records[rowIndex] as RecordEntry).Data;

            if (e.RowColumnIndex.ColumnIndex == 2)
            {
                record.setor_caminho = ((SetorModel)e.SelectedItem).setor; 
            }
        }

    }

    class AlterarSolicitacaoOrdemServicoProdutoViewModel : INotifyPropertyChanged
    {

        private ObsOsModel _obsOs;
        public ObsOsModel ObsOs
        {
            get { return _obsOs; }
            set { _obsOs = value; RaisePropertyChanged("ObsOs"); }
        }
        private ObservableCollection<ObsOsModel> _obsOSs;
        public ObservableCollection<ObsOsModel> ObsOSs
        {
            get { return _obsOSs; }
            set { _obsOSs = value; RaisePropertyChanged("ObsOSs"); }
        }

        private SetorModel _setor;
        public SetorModel Setor
        {
            get { return _setor; }
            set { _setor = value; RaisePropertyChanged("Setor"); }
        }
        private ObservableCollection<SetorModel> _setores;
        public ObservableCollection<SetorModel> Setores
        {
            get { return _setores; }
            set { _setores = value; RaisePropertyChanged("Setores"); }
        }

        private List<string> _distribuirOS = new List<string> { "No setor", "No Solicitante", "No Encarregado"};
        public List<string> DistribuirOS
        {
            get { return _distribuirOS; }
            set { _distribuirOS = value; RaisePropertyChanged("DistribuirOS"); }
        }

        private List<string> _ipoOS = new List<string> { "PEÇA NOVA", "RECUPERAÇÃO", "RETRABALHO", "KIT"};
        public List<string> TpoOS
        {
            get { return _ipoOS; }
            set { _ipoOS = value; RaisePropertyChanged("TpoOS"); }
        }

        private SiglaChkListModel _sigla;
        public SiglaChkListModel Sigla
        {
            get { return _sigla; }
            set { _sigla = value; RaisePropertyChanged("Sigla"); }
        }

        private ObservableCollection<SiglaChkListModel> _siglas;
        public ObservableCollection<SiglaChkListModel> Siglas
        {
            get { return _siglas; }
            set { _siglas = value; RaisePropertyChanged("Siglas"); }
        }

        private AlteraSolicitacaoOsProducao _ordemServico;
        public AlteraSolicitacaoOsProducao OrdemServico
        {
            get { return _ordemServico; }
            set { _ordemServico = value; RaisePropertyChanged("OrdemServico"); }
        }
        private ObservableCollection<AlteraSolicitacaoOsProducao> _ordemServicos;
        public ObservableCollection<AlteraSolicitacaoOsProducao> OrdemServicos
        {
            get { return _ordemServicos; }
            set { _ordemServicos = value; RaisePropertyChanged("OrdemServicos"); }
        }

        private ProdutoOsModel _produtoOs;
        public ProdutoOsModel ProdutoOs
        {
            get { return _produtoOs; }
            set { _produtoOs = value; RaisePropertyChanged("ProdutoOs"); }
        }
        private ObservableCollection<ProdutoOsModel> _produtoOSs;
        public ObservableCollection<ProdutoOsModel> ProdutoOSs
        {
            get { return _produtoOSs; }
            set { _produtoOSs = value; RaisePropertyChanged("ProdutoOSs"); }
        }

        public async Task<ObservableCollection<SetorModel>> GetSetorsAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await (from s in db.SetorProducaos where s.inativo == "0    " select new SetorModel { setor = s.setor + " - " + s.galpao, codigo_setor = s.codigo_setor }).ToListAsync();
                return new ObservableCollection<SetorModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<SiglaChkListModel>> GetSiglasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Siglas.OrderBy(c => c.sigla_serv).ToListAsync();
                return new ObservableCollection<SiglaChkListModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<RelplanModel>> GetPlanilhasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                return new ObservableCollection<RelplanModel>(await db.Relplans.OrderBy(c => c.planilha).Where(c => c.ativo.Equals("1")).ToListAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AlteraSolicitacaoOsProducao> GetOrdemServicoAsync(long num_os_produto)
        {
            try
            {
                using DatabaseContext db = new();
                return await db.AlteraSolicitacaoOsProducaos.Where(c => c.num_os_produto == num_os_produto).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProdutoOsModel> AddProdutoOsAsync(ProdutoOsModel produtoOs)
        {
            try
            {
                using DatabaseContext db = new();
                await db.ProdutoOs.SingleMergeAsync(produtoOs);
                await db.SaveChangesAsync();
                return produtoOs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObsOsModel> SaveProdutoOsAsync(ObsOsModel obsOs)
        {
            try
            {
                using DatabaseContext db = new();
                await db.ObsOs.SingleMergeAsync(obsOs);
                await db.SaveChangesAsync();
                return obsOs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<ObsOsModel>> GetCaminhosOSAsync(long num_os_produto)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ObsOs.OrderBy(c => c.num_caminho).Where(c => c.num_os_produto == num_os_produto).ToListAsync();
                return new ObservableCollection<ObsOsModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
