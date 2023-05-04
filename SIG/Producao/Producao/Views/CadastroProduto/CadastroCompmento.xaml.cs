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
    /// Interação lógica para CadastroCompmento.xam
    /// </summary>
    public partial class CadastroCompmento : Window
    {
        TabelaDescAdicionalModel produtoAdicional;

        public CadastroCompmento(TabelaDescAdicionalModel produtoAdicional)
        {
            DataContext = new CadastroCompmentoViewModel();
            InitializeComponent();
            this.produtoAdicional = produtoAdicional;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                CadastroCompmentoViewModel vm = (CadastroCompmentoViewModel)DataContext;
                vm.Unidades = await Task.Run(vm.GetUnidadesAsync);
                vm.ComplementoAdicionais = await Task.Run(() => vm.GetComplementoAdicionaisAsync(produtoAdicional.coduniadicional));
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

    public class CadastroCompmentoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private TblComplementoAdicionalModel _complementoAdicional;
        public TblComplementoAdicionalModel ComplementoAdicional
        {
            get { return _complementoAdicional; }
            set { _complementoAdicional = value; RaisePropertyChanged("ComplementoAdicional"); }
        }

        private ObservableCollection<TblComplementoAdicionalModel> _complementoAdicionais;
        public ObservableCollection<TblComplementoAdicionalModel> ComplementoAdicionais
        {
            get { return _complementoAdicionais; }
            set { _complementoAdicionais = value; RaisePropertyChanged("ComplementoAdicionais"); }
        }

        private UnidadeModel _unidade;
        public UnidadeModel Unidade
        {
            get { return _unidade; }
            set { _unidade = value; RaisePropertyChanged("Unidade"); }
        }

        private ObservableCollection<UnidadeModel> _unidades;
        public ObservableCollection<UnidadeModel> Unidades
        {
            get { return _unidades; }
            set { _unidades = value; RaisePropertyChanged("Unidades"); }
        }

        public async Task<ObservableCollection<TblComplementoAdicionalModel>> GetComplementoAdicionaisAsync(long? coduniadicional)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ComplementoAdicionais
                    .OrderBy(c => c.acompanhamento)
                    .Where(c => c.coduniadicional == coduniadicional)
                    .ToListAsync();
                return new ObservableCollection<TblComplementoAdicionalModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<UnidadeModel>> GetUnidadesAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Unidades
                    .OrderBy(c => c.unidade)
                    .ToListAsync();
                return new ObservableCollection<UnidadeModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
