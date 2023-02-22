using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
using System.Windows.Shapes;

namespace Producao.Views.PopUp
{
    /// <summary>
    /// Lógica interna para ModeloControleChecklist.xaml
    /// </summary>
    public partial class ModeloControleChecklist : Window
    {

        private QryModeloModel modelo;

        public ModeloControleChecklist(QryModeloModel modelo)
        {
            InitializeComponent();
            this.DataContext = new ModeloControleChecklistViewModel();
            this.modelo = modelo;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            ModeloControleChecklistViewModel vm = (ModeloControleChecklistViewModel)DataContext;
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.ItensChkList = await Task.Run(() => vm.GetControlesAsync(modelo));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;

                MessageBox.Show("Precisona a tecla F3 para dar baixa na linha selecionada.","Info Baixa", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                var confirm = MessageBox.Show($"Deseja confirmar a inclusão do modelo {modelo.id_modelo} na linha selecionada?", "Baixa modelo", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            }
        }
    }

    public class ModeloControleChecklistViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ControleModeloBaixa _itemChkList;
        public ControleModeloBaixa ItemChkList
        {
            get { return _itemChkList; }
            set { _itemChkList = value; RaisePropertyChanged("ItemChkList"); }
        }
        private ObservableCollection<ControleModeloBaixa> _itensChkList;
        public ObservableCollection<ControleModeloBaixa> ItensChkList
        {
            get { return _itensChkList; }
            set { _itensChkList = value; RaisePropertyChanged("ItensChkList"); }
        }

        public async Task<ObservableCollection<ControleModeloBaixa>> GetControlesAsync(QryModeloModel modelo)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ModelosControle.Where(c => c.tema == modelo.tema && c.codcompladicional == modelo.codcompladicional && c.id_modelo == null && c.qtd_compl_chk > 0).ToListAsync();
                return new ObservableCollection<ControleModeloBaixa>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
