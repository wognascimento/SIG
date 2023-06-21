using Microsoft.EntityFrameworkCore;
using Producao.Views.Planilha;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Producao.Views.OrdemServico.Produto
{
    /// <summary>
    /// Interação lógica para BaixaOrdemServicoProduto.xam
    /// </summary>
    public partial class BaixaOrdemServicoProduto : UserControl
    {
        public BaixaOrdemServicoProduto()
        {
            InitializeComponent();
            this.DataContext = new BaixaOrdemServicoProdutoViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                BaixaOrdemServicoProdutoViewModel vm = (BaixaOrdemServicoProdutoViewModel)DataContext;
                vm.Itens = await Task.Run(vm.GetItensAsync);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }


        private async void SfDataGrid_RowValidated(object sender, Syncfusion.UI.Xaml.Grid.RowValidatedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                BaixaOrdemServicoProdutoViewModel vm = (BaixaOrdemServicoProdutoViewModel)DataContext;
                BaixaOsProducaoModel data = (BaixaOsProducaoModel)e.RowData;
                await Task.Run(() => vm.BaixaAsync(data));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class BaixaOrdemServicoProdutoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BaixaOsProducaoModel>? _itens;
        public ObservableCollection<BaixaOsProducaoModel> Itens
        {
            get { return _itens; }
            set { _itens = value; RaisePropertyChanged("Itens"); }
        }

        private BaixaOsProducaoModel? _item;
        public BaixaOsProducaoModel Item
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged("Item"); }
        }

        public async Task<ObservableCollection<BaixaOsProducaoModel>> GetItensAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.BaixaOsProducoes
                    .ToListAsync();
                return new ObservableCollection<BaixaOsProducaoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task BaixaAsync(BaixaOsProducaoModel baixa)
        {
            try
            {
                using DatabaseContext db = new();
                var os = await db.ProdutoServicos.FindAsync(baixa.num_os_servico);
                os.recebido_setor_data = baixa.recebido_setor_data;
                os.concluida_os_data = baixa.concluida_os_data;
                await db.ProdutoServicos.SingleMergeAsync(os);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

    }
}
