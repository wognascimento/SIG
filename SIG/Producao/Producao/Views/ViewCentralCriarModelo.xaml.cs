using Microsoft.EntityFrameworkCore;
using Producao.Views.PopUp;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.ScrollAxis;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Producao.Views
{
    /// <summary>
    /// Interação lógica para ViewCentralCriarModelo.xam
    /// </summary>
    public partial class ViewCentralCriarModelo : UserControl
    {
        private  CentralCriarModeloViewModel? vm;
        private QryModeloModel? modelo;
        public ViewCentralCriarModelo()
        {
            InitializeComponent();
            
        }

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            this.DataContext = new CentralCriarModeloViewModel();
            vm = (CentralCriarModeloViewModel)DataContext;
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Planilhas = await Task.Run(vm.GetPlanilhasAsync);
                vm.Temas = await Task.Run(vm.GetTemasAsync);
                vm.QryModelos = await Task.Run(vm.GetModelos);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtPlanilha.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }
        private async void OnBuscaProduto(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    string text = ((TextBox)sender).Text;
                    vm.Descricao = await Task.Run(() => vm.GetDescricaoAsync(long.Parse(text)));
                    if (vm.Descricao == null)
                    {
                        MessageBox.Show("Produto não encontrado", "Busca de produto");
                        Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                        return;
                    }
                    txtCodigoProduto.Text = vm.Descricao.codcompladicional.ToString();
                    txtPlanilha.Text = vm.Descricao.planilha;
                    txtDescricao.Text = vm.Descricao.descricao;
                    txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                    txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                    txtTema.Focus();

                    dgModelos.Columns["codcompladicional"].FilteredFrom = FilteredFrom.FilterRow;
                    dgModelos.Columns["codcompladicional"].FilterPredicates.Add(new FilterPredicate()
                    {
                        FilterType = FilterType.Equals,
                        FilterValue = vm.Descricao.codcompladicional
                    });
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }
                
                
            }
        }
        private void OnOpenDescricoes(object sender, RoutedEventArgs e)
        {
            var window = new BuscaProduto();
            window.Owner = App.Current.MainWindow;
            //window.ShowDialog();
            if (window.ShowDialog() == true)
            {
                vm.Descricao = window.descricao;

                txtCodigoProduto.Text = vm.Descricao.codcompladicional.ToString();
                txtPlanilha.Text = vm.Descricao.planilha;
                txtDescricao.Text = vm.Descricao.descricao;
                txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                txtTema.Focus();
                dgModelos.Columns["codcompladicional"].FilteredFrom = FilteredFrom.FilterRow;
                dgModelos.Columns["codcompladicional"].FilterPredicates.Add(new FilterPredicate()
                {
                    FilterType = FilterType.Equals,
                    FilterValue = vm.Descricao.codcompladicional
                });
            }  
        }
        private async void OnSelectedPlanilha(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                RelplanModel? planilha = e.NewValue as RelplanModel;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Produtos = await Task.Run(()=> vm.GetProdutosAsync(planilha?.planilha));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtDescricao.Focus();
                dgModelos.Columns["planilha"].FilteredFrom = FilteredFrom.FilterRow;
                dgModelos.Columns["planilha"].FilterPredicates.Add(new FilterPredicate()
                {
                    FilterType = FilterType.Equals,
                    FilterValue = planilha?.planilha
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnSelectedDescricao(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ProdutoModel? produto = e.NewValue as ProdutoModel;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(produto?.codigo));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtDescricaoAdicional.Focus();
                dgModelos.Columns["descricao_completa"].FilteredFrom = FilteredFrom.FilterRow;
                dgModelos.Columns["descricao_completa"].FilterPredicates.Add(new FilterPredicate()
                {
                    FilterBehavior = FilterBehavior.StringTyped,
                    FilterType = FilterType.Contains,
                    FilterValue = produto?.descricao
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void OnSelectedDescricaoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                TabelaDescAdicionalModel? adicional = e.NewValue as TabelaDescAdicionalModel;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(adicional?.coduniadicional));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtComplementoAdicional.Focus();
                dgModelos.Columns["descricao_completa"].FilteredFrom = FilteredFrom.FilterRow;
                dgModelos.Columns["descricao_completa"].FilterPredicates.Add(new FilterPredicate()
                {
                    FilterBehavior = FilterBehavior.StringTyped,
                    FilterType = FilterType.Contains,
                    FilterValue = adicional?.descricao_adicional
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSelectedComplementoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TblComplementoAdicionalModel? complemento = e.NewValue as TblComplementoAdicionalModel;
            vm.Compledicional = complemento;
            txtCodigoProduto.Text = complemento?.codcompladicional.ToString();
            txtTema.Focus();
            dgModelos.Columns["descricao_completa"].FilteredFrom = FilteredFrom.FilterRow;
            dgModelos.Columns["descricao_completa"].FilterPredicates.Add(new FilterPredicate()
            {
                FilterBehavior = FilterBehavior.StringTyped,
                FilterType = FilterType.Contains,
                FilterValue = complemento?.complementoadicional
            });
        }

        private void OnSelectedTema(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TemaModel? tema = e.NewValue as TemaModel;
            vm.Tema = tema;
            dgModelos.Columns["tema"].FilteredFrom = FilteredFrom.FilterRow;
            dgModelos.Columns["tema"].FilterPredicates.Add(new FilterPredicate()
            {
                FilterType = FilterType.Equals,
                FilterValue = tema?.temas
            });
        }

        private void Limpar()
        {
            txtCodigoProduto.Text = string.Empty;
            txtPlanilha.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtDescricaoAdicional.Text = string.Empty;
            txtComplementoAdicional.Text = string.Empty;
            txtTema.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            txtPlanilha.Focus();
            dgModelos.ClearFilters();
            this.dgModelos.SelectedItems.Clear();
            modelo = null;
        }

        private void OnLimparClick(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private async void OnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                var dados = new ModeloModel
                {
                    id_modelo = modelo?.id_modelo,
                    codcompladicional = long.Parse(txtCodigoProduto.Text), 
                    tema = txtTema.Text, 
                    obs_modelo = txtObservacao.Text,
                    cadastrado_por = Environment.UserName,
                    data_cadastro = DateTime.Now
                };

                var tema = txtTema.SelectedItem as TemaModel;
                
                vm.Modelo = await Task.Run(() => vm.AddModeloAsync(dados, tema));
                if (modelo == null)
                {
                    dgModelos.ClearFilters();
                    modelo = await Task.Run(() => vm.GetModelo(vm.Modelo.id_modelo));
                    vm?.QryModelos.Add(modelo);
                    RowColumnIndex rowColumnIndex = new RowColumnIndex();
                    this.dgModelos.SearchHelper.Search(vm?.Modelo.id_modelo.ToString());
                    this.dgModelos.SearchHelper.FindNext(vm?.Modelo.id_modelo.ToString());
                    rowColumnIndex.RowIndex = this.dgModelos.SearchHelper.CurrentRowColumnIndex.RowIndex;
                    dgModelos.ScrollInView(rowColumnIndex);
                    this.dgModelos.SelectedItem = modelo;

                    /*
                    var modelo = (QryModeloModel)dgModelos.SelectedItem;
                    if (modelo == null)
                    {
                        MessageBox.Show("Precisa selecionar um modelo para ir na Receita", "Receita");
                        return;
                    }
                    */

                    var window = new ModeloReceita(modelo);
                    window.Owner = App.Current.MainWindow;
                    window.ShowDialog();
                }
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }


        private void OnReceitaClick(object sender, RoutedEventArgs e)
        {
            var modelo = (QryModeloModel)dgModelos.SelectedItem;

            if(modelo == null)
            {
                MessageBox.Show("Precisa selecionar um modelo para ir na Receita","Receita");
                return;
            }

            var window = new ModeloReceita(modelo);
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
        }

        private void OnControleClick(object sender, RoutedEventArgs e)
        {
            modelo = (QryModeloModel)dgModelos.SelectedItem;

            if (modelo == null)
            {
                MessageBox.Show("Precisa selecionar um modelo para abrir o controle", "Controle");
                return;
            }

            var window = new ModeloControleChecklist(modelo);
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
        }

        private void OnModelosFiadaClick(object sender, RoutedEventArgs e)
        {
            var window = new ModeloFiada(modelo);
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
        }

        private async void dgModelos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            modelo = (QryModeloModel)dgModelos.SelectedItem;

            CentralCriarModeloViewModel? vm = (CentralCriarModeloViewModel)DataContext;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.Descricao = await Task.Run(() => vm.GetDescricaoAsync(modelo.codcompladicional));
                txtCodigoProduto.Text = vm.Descricao.codcompladicional.ToString();
                txtPlanilha.Text = vm.Descricao.planilha;
                txtDescricao.Text = vm.Descricao.descricao;
                txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                txtTema.Text = modelo.tema;
                txtObservacao.Text = modelo.obs_modelo;

                vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(vm.Descricao.planilha));
                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(vm.Descricao.codigo));
                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(vm.Descricao.coduniadicional));

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private void dgModelos_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //var SelectedItem = ((DetailsViewDataGrid)e.OriginalSender).SelectedItem;
        }

    }

    public class CentralCriarModeloViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private QryModeloModel qrymodelo;
        public QryModeloModel QryModelo
        {
            get { return qrymodelo; }
            set { qrymodelo = value; RaisePropertyChanged("QryModelo"); }
        }

        private ObservableCollection<QryModeloModel>? qrymodelos;
        public ObservableCollection<QryModeloModel> QryModelos
        {
            get { return qrymodelos; }
            set { qrymodelos = value; RaisePropertyChanged("QryModelos"); }
        }

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

        private ObservableCollection<TemaModel> _temas;
        public ObservableCollection<TemaModel> Temas
        {
            get { return _temas; }
            set { _temas = value; RaisePropertyChanged("Temas"); }
        }
        private TemaModel _tema;
        public TemaModel Tema
        {
            get { return _tema; }
            set { _tema = value; RaisePropertyChanged("Tema"); }
        }
        private QryDescricao _descricao;
        public QryDescricao Descricao
        {
            get { return _descricao; }
            set { _descricao = value; RaisePropertyChanged("Descricao"); }
        }

        private ModeloModel modelo;
        public ModeloModel Modelo
        {
            get { return modelo; }
            set { modelo = value; RaisePropertyChanged("Modelo"); }
        }

        public async Task<QryModeloModel> GetModelo(long? id_modelo)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.qryModelos.Where(m => m.id_modelo == id_modelo).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<QryModeloModel>> GetModelos()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.qryModelos.ToListAsync();
                return new ObservableCollection<QryModeloModel>(data);
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
                var data = await db.Relplans
                    .OrderBy(c => c.planilha)
                    .Where(c => c.ativo.Equals("1"))
                    .Where(c => c.agrupamento.Contains("CENTRAL DE MODELOS"))
                    .ToListAsync();
                return new ObservableCollection<RelplanModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<ProdutoModel>> GetProdutosAsync(string? planilha)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Produtos
                    .OrderBy(c => c.descricao)
                    .Where(c => c.planilha.Equals(planilha))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                return new ObservableCollection<ProdutoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<TabelaDescAdicionalModel>> GetDescAdicionaisAsync(long? codigo)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.DescAdicionais
                    .OrderBy(c => c.descricao_adicional)
                    .Where(c => c.codigoproduto.Equals(codigo))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                return new ObservableCollection<TabelaDescAdicionalModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<TblComplementoAdicionalModel>> GetCompleAdicionaisAsync(long? coduniadicional)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ComplementoAdicionais
                    .OrderBy(c => c.complementoadicional)
                    .Where(c => c.coduniadicional.Equals(coduniadicional))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                return new ObservableCollection<TblComplementoAdicionalModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GravarAsync()
        {
            try
            {
                using DatabaseContext db = new();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<TemaModel>> GetTemasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Temas
                    .OrderBy(c => c.temas)
                    .Where(c => c.ativo == "S")
                    .ToListAsync();
                return new ObservableCollection<TemaModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<QryDescricao> GetDescricaoAsync(long? codcompladicional)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Descricoes.Where(c => c.codcompladicional == codcompladicional).FirstOrDefaultAsync();
                    
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ModeloModel> AddModeloAsync(ModeloModel modelo, TemaModel tema)
        {
            using DatabaseContext db = new();
            var transaction = db.Database.BeginTransaction();
            try
            {
                await db.Modelos.SingleMergeAsync(modelo);
                await db.SaveChangesAsync();

                var historico =  await db.HistoricosModelo.Where(c =>  c.codcompladicional_modelo == modelo.codcompladicional && c.idtema == tema.idtema ).ToListAsync();
                foreach (HistoricoModelo item in historico)
                {
                    var receita = new ModeloReceitaModel
                    {
                        id_modelo = modelo.id_modelo,
                        codcompladicional = item.codcompladicional_receita,
                        qtd_modelo = item.media_qtd_modelo,
                        qtd_producao = item.media_qtd_producao,
                        cadastrado_por = Environment.UserName,
                        data_cadastro = DateTime.Now,
                    };
                    await db.ReceitaModelos.SingleMergeAsync(receita);
                    await db.SaveChangesAsync();
                }
                //<ObservableCollection<TemaModel>>
                transaction.Commit();

                return modelo;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

    }

}
