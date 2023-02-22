using Microsoft.EntityFrameworkCore;
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

namespace Compras.Views
{
    /// <summary>
    /// Interação lógica para ViewConsultaProdutos.xam
    /// </summary>
    public partial class ViewConsultaProdutos : UserControl
    {
        public ViewConsultaProdutos()
        {
            InitializeComponent();
            this.DataContext = new TodosProdutosViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TodosProdutosViewModel vm = (TodosProdutosViewModel)DataContext;
                vm.Descricoes = await Task.Run(vm.GetDescricaosAsync);
                loading.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                loading.Visibility = Visibility.Collapsed;
            }
        }
    }

    public class TodosProdutosViewModel : INotifyPropertyChanged
    {
        public DataBaseSettings BaseSettings = DataBaseSettings.Instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #region Todos Produtos
        private DescricaoProducaoModel descricao;
        public DescricaoProducaoModel Descricao
        {
            get { return descricao; }
            set { descricao = value; RaisePropertyChanged("Descricao"); }
        }
        private ObservableCollection<DescricaoProducaoModel> descricoes;
        public ObservableCollection<DescricaoProducaoModel> Descricoes
        {
            get { return descricoes; }
            set { descricoes = value; RaisePropertyChanged("Descricoes"); }
        }
        #endregion

        public async Task<ObservableCollection<DescricaoProducaoModel>> GetDescricaosAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.DescricoesProducao.ToListAsync();
                return new ObservableCollection<DescricaoProducaoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
