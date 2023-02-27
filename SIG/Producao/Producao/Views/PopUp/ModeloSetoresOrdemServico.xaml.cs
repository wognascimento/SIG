using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Producao.Views.PopUp
{
    /// <summary>
    /// Lógica interna para ModeloSetoresOrdemServico.xaml
    /// </summary>
    public partial class ModeloSetoresOrdemServico : Window
    {

        private long? codcompladicional;

        public ModeloSetoresOrdemServico(long? codcompladicional)
        {
            InitializeComponent();
            this.DataContext = new ModeloSetoresOrdemServicoViewModel();
            this.codcompladicional = codcompladicional;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ModeloSetoresOrdemServicoViewModel vm = (ModeloSetoresOrdemServicoViewModel)DataContext;
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Itens = await Task.Run(() => vm.GetSetoresAsync(this.codcompladicional));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }

        private void dgItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dgItens_AddNewRowInitiating(object sender, Syncfusion.UI.Xaml.Grid.AddNewRowInitiatingEventArgs e)
        {

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
        public HistoricoSetorModel Item
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged("Item"); }
        }
        private ObservableCollection<HistoricoSetorModel> _itens;
        public ObservableCollection<HistoricoSetorModel> Itens
        {
            get { return _itens; }
            set { _itens = value; RaisePropertyChanged("Itens"); }
        }

        public async Task<ObservableCollection<HistoricoSetorModel>> GetSetoresAsync(long? codcompladicional)
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

    }
}
