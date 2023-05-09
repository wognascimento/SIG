using Microsoft.EntityFrameworkCore;
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
    /// Interação lógica para ViewEtiquetaCheckList.xam
    /// </summary>
    public partial class ViewEtiquetaCheckList : UserControl
    {
        private int etiqueta = 1;
        public ViewEtiquetaCheckList()
        {
            InitializeComponent();
            this.DataContext = new EtiquetaViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                EtiquetaViewModel vm = (EtiquetaViewModel)DataContext;
                await Task.Run(async () => await vm.GetSiglasAsync());
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }

        private async void OnSiglaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                EtiquetaViewModel vm = (EtiquetaViewModel)DataContext;
                await Task.Run(async () => await vm.GetItensAsync());
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }

        private async void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                EtiquetaViewModel vm = (EtiquetaViewModel)DataContext;
                await Task.Run(async () => await vm.GetEtiquetasAsync());
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }

        private async void dgEtiqueta_RowValidated(object sender, Syncfusion.UI.Xaml.Grid.RowValidatedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                EtiquetaViewModel vm = (EtiquetaViewModel)DataContext;
                vm.Etiqueta = (EtiquetaProducaoModel)e.RowData;
                //EtiquetaProducaoModel data = (EtiquetaProducaoModel)e.RowData;
                await Task.Run(async () => await vm.AddEtiquetaAsync());
                await Task.Run(async () => await vm.GetEtiquetasAsync());
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgEtiqueta_RowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {
            EtiquetaViewModel vm = (EtiquetaViewModel)DataContext;
            EtiquetaProducaoModel rowData = (EtiquetaProducaoModel)e.RowData;
            if (!rowData.coddetalhescompl.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("codvol", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("volumes", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("volumes_total", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("qtd", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("largura", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("altura", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("profundidade", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("peso_bruto", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("peso_liquido", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("impresso", "Erro ao selecionar a linha.");
            }
            else if(rowData.codvol.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("codvol", "Este campo precisar está em branco.");
            }
            else if (!rowData.volumes.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("volumes", "Informe o volume.");
            }
            else if (rowData.volumes == 0)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("volumes", "Volume não pode ser Zero(0).");
            }
            else if (!rowData.volumes_total.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("volumes_total", "Informe o total de volume.");
            }
            else if (rowData.volumes_total < rowData.volumes)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("volumes_total", "Total de volumes não pode ser menor que volume");
            }
            else if (!rowData.qtd.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("qtd", "Informe a quantidade em cada volume.");
            }
            else if (rowData.qtd > vm.Item.qtd_nao_expedida)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("qtd", "a quantidade da etiqueta não pode ser menor que a do checklist");
            }
            /*if ((e.RowData as EtiquetaProducaoModel).volumes == null)
            {
                e.IsValid = false;

            }*/
        }

        private void dgEtiqueta_AddNewRowInitiating(object sender, Syncfusion.UI.Xaml.Grid.AddNewRowInitiatingEventArgs e)
        {
            EtiquetaViewModel vm = (EtiquetaViewModel)DataContext;
            ((EtiquetaProducaoModel)e.NewObject).coddetalhescompl = vm.Item.coddetalhescompl; // = new long?(ProdutoExpedido.CodDetalhesCompl);
        }

    }

    public class EtiquetaViewModel : INotifyPropertyChanged
    {
        public DataBaseSettings BaseSettings = DataBaseSettings.Instance;

        private ObservableCollection<SiglaChkListModel> _siglas;
        public ObservableCollection<SiglaChkListModel> Siglas
        {
            get { return _siglas; }
            set { _siglas = value; RaisePropertyChanged("Siglas"); }
        }
        private SiglaChkListModel _sigla;
        public SiglaChkListModel Sigla
        {
            get { return _sigla; }
            set { _sigla = value; RaisePropertyChanged("Sigla"); }
        }

        private ObservableCollection<EtiquetaCheckListModel> _itens;
        public ObservableCollection<EtiquetaCheckListModel> Itens
        {
            get { return _itens; }
            set { _itens = value; RaisePropertyChanged("Itens"); }
        }
        private EtiquetaCheckListModel _item;
        public EtiquetaCheckListModel Item
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged("Item"); }
        }

        private ObservableCollection<EtiquetaProducaoModel> _etiquetas;
        public ObservableCollection<EtiquetaProducaoModel> Etiquetas
        {
            get { return _etiquetas; }
            set { _etiquetas = value; RaisePropertyChanged("Etiquetas"); }
        }
        private EtiquetaProducaoModel _etiqueta;
        public EtiquetaProducaoModel Etiqueta
        {
            get { return _etiqueta; }
            set { _etiqueta = value; RaisePropertyChanged("Etiqueta"); }
        }

        public EtiquetaViewModel()
        {
           
        }

        public async Task GetSiglasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Siglas.OrderBy(c => c.sigla_serv).ToListAsync();
                Siglas = new ObservableCollection<SiglaChkListModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetItensAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.EtiquetaCheckLists.Where(e => e.sigla == Sigla.sigla_serv && e.qtd_detalhe > 0 && e.qtd_nao_expedida > 0).OrderBy(c => c.item_memorial).ToListAsync();
                Itens = new ObservableCollection<EtiquetaCheckListModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetEtiquetasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.EtiquetaProducaos.Where(e => e.coddetalhescompl == Item.coddetalhescompl ).OrderBy(c => c.codvol).ToListAsync();
                Etiquetas = new ObservableCollection<EtiquetaProducaoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddEtiquetaAsync()
        {
            try
            {
                using DatabaseContext db = new();
                db.Entry(Etiqueta).State = Etiqueta.codvol == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
