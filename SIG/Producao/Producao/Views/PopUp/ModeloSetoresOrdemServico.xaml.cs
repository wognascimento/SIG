using Microsoft.EntityFrameworkCore;
using Syncfusion.Data;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Producao.Views.PopUp
{
    /// <summary>
    /// Lógica interna para ModeloSetoresOrdemServico.xaml
    /// </summary>
    public partial class ModeloSetoresOrdemServico : Window
    {

        //private long? codcompladicional;
        private ModeloControleOsModel modeloControle;

        public ModeloSetoresOrdemServico(ModeloControleOsModel modeloControle)
        {
            InitializeComponent();
            this.DataContext = new ModeloSetoresOrdemServicoViewModel();
            this.modeloControle = modeloControle;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ModeloSetoresOrdemServicoViewModel vm = (ModeloSetoresOrdemServicoViewModel)DataContext;
            
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Setores = await Task.Run(vm.GetSetorsAsync);
                vm.Itens = await Task.Run(() => vm.GetSetoresProdutoAsync(modeloControle.codcompladicional));
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });

                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }

        private void dgItens_CurrentCellDropDownSelectionChanged(object sender, CurrentCellDropDownSelectionChangedEventArgs e)
        {
            var sfdatagrid = sender as SfDataGrid;
            int rowIndex = sfdatagrid.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
            var record = sfdatagrid.View.Records[rowIndex].Data as HistoricoSetorModel;
            record.selesao = true;
            record.setor = ((SetorModel)e.SelectedItem).setor;
        }

        private void dgItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dgItens_AddNewRowInitiating(object sender, AddNewRowInitiatingEventArgs e)
        {

        }

        private void OnAddSetor(object sender, RoutedEventArgs e)
        {
            adicionarSetor();
        }

        private async void adicionarSetor()
        {
            Window window = new()
            {
                Owner = this,
                Title = "ADICIONAR SETOR",
                WindowStyle = WindowStyle.ToolWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Content = new AddSetorOrdemServico(this.DataContext),
                Width = 300,
                Height = 300,
            };
            window.ShowDialog();
        }

        private async void OnCreateOrdemServico(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                ModeloSetoresOrdemServicoViewModel vm = (ModeloSetoresOrdemServicoViewModel)DataContext;
                await Task.Run(() => vm.CreateOrdenServicoAsync(modeloControle));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

    }

    public class ModeloSetoresOrdemServicoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private HistoricoSetorModel _item;
        public HistoricoSetorModel Iteme
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged("Iteme"); }
        }
        private ObservableCollection<HistoricoSetorModel> _itens;
        public ObservableCollection<HistoricoSetorModel> Itens
        {
            get { return _itens; }
            set { _itens = value; RaisePropertyChanged("Itens"); }
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

        public async Task<ObservableCollection<HistoricoSetorModel>> GetSetoresProdutoAsync(long? codcompladicional)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.HistoricosSetor.Where(c => c.codcompladicional == codcompladicional).ToListAsync();
                return new ObservableCollection<HistoricoSetorModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //ObservableCollection<SetorProducaoModel>
        public async Task<ObservableCollection<SetorModel>> GetSetorsAsync()
        {
            try
            {
                using DatabaseContext db = new();
                //var data = await db.SetorProducaos.Where(c => c.inativo == "0    ").OrderBy(c => c.setor).ToListAsync();
                var data = await (from s in db.SetorProducaos where s.inativo == "0    " select new SetorModel { setor =  s.setor + " - " + s.galpao, codigo_setor = s.codigo_setor}).ToListAsync();
                return new ObservableCollection<SetorModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateOrdenServicoAsync(ModeloControleOsModel modeloControle)
        {
            using DatabaseContext db = new();
            using var transaction = db.Database.BeginTransaction();

            var Setores = (from e in Itens where e.selesao == true select e).ToList();

            if (Setores.Count == 0)
                throw new InvalidOperationException("Não existe setor para criar ordem de serviço.");

            var quantidade = (modeloControle?.qtd_chk_list - (int)(modeloControle?.qtd_os ?? 0));

            try
            {
                var produto = await db.Descricoes.Where(d => d.codcompladicional == modeloControle.codcompladicional).FirstOrDefaultAsync();
                var produtoOsModel = new ProdutoOsModel
                {
                    tipo = "KIT",
                    planilha = produto.planilha,
                    quantidade = (float?)quantidade,
                    responsavel_emissao = Environment.UserName,
                    data_emissao = DateTime.Now,
                    cod_produto = produto.codigo,
                    cod_desc_adicional = produto.coduniadicional,
                    cod_compl_adicional = produto.codcompladicional,
                    id_modelo = modeloControle.id_modelo
                };
                await db.ProdutoOs.AddAsync(produtoOsModel);
                await db.SaveChangesAsync();

                for (int i = 0; i < Setores.Count; i++)
                {
                    var item = Setores[i];
                    var Obs = new ObsOsModel
                    {
                        num_os_produto = produtoOsModel.num_os_produto,
                        cod_compl_adicional = produto.codcompladicional,
                        num_caminho = i + 1,
                        codigo_setor = item.codigo_setor,
                        setor_caminho = item.setor,
                        orientacao_caminho = item.observacao,
                        distribuir_os = "No setor",
                        cliente = modeloControle.sigla,
                        solicitado_por = Environment.UserName,
                        solicitado_data = DateTime.Now
                    };

                    await db.ObsOs.AddAsync(Obs);
                    await db.SaveChangesAsync();
                }
                
                var solictAberta = await db.OrdemServicoEmissaoAbertas.Where(o => o.num_os_produto == produtoOsModel.num_os_produto).ToListAsync();
                for (int i = 0; i < solictAberta.Count; i++)
                {
                    var item = solictAberta[i];
                    var teste = ((i + 1) < solictAberta.Count);
                    var produtoServicoModel = new ProdutoServicoModel
                    {
                        num_os_produto = item.num_os_produto,
                        tipo = item.tipo,
                        codigo_setor = item.codigo_setor,
                        setor_caminho = item.setor_caminho,
                        quantidade = item.quantidade,
                        data_inicio = DateTime.Now,
                        data_fim = DateTime.Now.AddDays(15),
                        cliente = item.cliente,
                        tema = item.tema,
                        orientacao_caminho = item.orientacao_caminho,
                        codigo_setor_proximo = ((i+1) < solictAberta.Count) ? solictAberta[i+1].codigo_setor : 39,
                        setor_caminho_proximo = ((i+1) < solictAberta.Count) ? solictAberta[i+1].setor_caminho : "FINAL - TODOS",
                        fase = "PRODUÇÃO",
                        responsavel_emissao_os = Environment.UserName,
                        emitida_por = Environment.UserName,
                        emitida_data = DateTime.Now,
                        turno = "DIURNO",
                        id_modelo = item.id_modelo
                    };

                    await db.ProdutoServicos.AddAsync(produtoServicoModel);
                    await db.SaveChangesAsync();

                }
                
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

    }
}
