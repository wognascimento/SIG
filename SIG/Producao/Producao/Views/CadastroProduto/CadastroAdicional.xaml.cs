using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Producao.Views.CadastroProduto
{
    /// <summary>
    /// Interação lógica para CadastroAdicional.xam
    /// </summary>
    public partial class CadastroAdicional : Window
    {
        ProdutoModel produto;
        public CadastroAdicional(ProdutoModel produto)
        {
            DataContext = new CadastroAdicionalViewModel();
            InitializeComponent();
            this.produto = produto;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                CadastroAdicionalViewModel vm = (CadastroAdicionalViewModel)DataContext;
                vm.ProdutosAdicionais = await Task.Run(()=> vm.GetDescricaoAdicionaisAsync(produto.codigo));
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

        private void OnAddNewRowInitiating(object sender, Syncfusion.UI.Xaml.Grid.AddNewRowInitiatingEventArgs e)
        {

        }

        private void OnCurrentCellDropDownSelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellDropDownSelectionChangedEventArgs e)
        {

        }

        private void OnCurrentCellValueChanged(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellValueChangedEventArgs e)
        {

        }

        private void OnRowValidated(object sender, Syncfusion.UI.Xaml.Grid.RowValidatedEventArgs e)
        {

        }

        private void OnRowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {

        }

    }

    public class CadastroAdicionalViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private TabelaDescAdicionalModel _produtoAdicional;
        public TabelaDescAdicionalModel ProdutoAdicional
        {
            get { return _produtoAdicional; }
            set { _produtoAdicional = value; RaisePropertyChanged("ProdutoAdicional"); }
        }

        private ObservableCollection<TabelaDescAdicionalModel> _produtosAdicionais;
        public ObservableCollection<TabelaDescAdicionalModel> ProdutosAdicionais
        {
            get { return _produtosAdicionais; }
            set { _produtosAdicionais = value; RaisePropertyChanged("ProdutosAdicionais"); }
        }

        private ICommand rowDataCommand { get; set; }
        public ICommand RowDataCommand
        {
            get { return rowDataCommand; }
            set { rowDataCommand = value; }
        }

        public CadastroAdicionalViewModel()
        {
            rowDataCommand = new RelayCommand(ChangeCanExecute);
        }


        public async void ChangeCanExecute(object obj)
        {
            try
            {
                //obj = {Syncfusion.UI.Xaml.Grid.SfDataGrid}
                var window = new CadastroCompmento(ProdutoAdicional);
                window.Owner = App.Current.MainWindow;
                window.Height = 450;
                window.Width = 900;
                if (window.ShowDialog() == true) { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        public async Task<ObservableCollection<TabelaDescAdicionalModel>> GetDescricaoAdicionaisAsync(long? codigoproduto)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.DescAdicionais
                    .OrderBy(c => c.descricao_adicional)
                    .Where(c => c.codigoproduto == codigoproduto)
                    .ToListAsync();
                return new ObservableCollection<TabelaDescAdicionalModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
