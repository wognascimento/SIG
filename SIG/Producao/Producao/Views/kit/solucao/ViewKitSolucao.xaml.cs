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

namespace Producao.Views.kit.solucao
{
    /// <summary>
    /// Interação lógica para ViewKitSolucao.xam
    /// </summary>
    public partial class ViewKitSolucao : UserControl
    {
        public ViewKitSolucao()
        {
            InitializeComponent();
            DataContext = new KitSolucaoViewModel();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnSiglaSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }

    public class KitSolucaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private ObservableCollection<TblServicoModel> _siglas;
        public ObservableCollection<TblServicoModel> Siglas
        {
            get { return _siglas; }
            set { _siglas = value; RaisePropertyChanged("Siglas"); }
        }

        private TblServicoModel _sigla;
        public TblServicoModel Sigla
        {
            get { return _sigla; }
            set { _sigla = value; RaisePropertyChanged("Sigla"); }
        }
    }
}
