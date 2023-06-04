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

namespace Producao.Views.Estoque
{
    /// <summary>
    /// Interação lógica para MovimentacaoSaida.xam
    /// </summary>
    public partial class MovimentacaoSaida : UserControl
    {
        public MovimentacaoSaida()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnBuscaProduto(object sender, KeyEventArgs e)
        {

        }

        private void OnOpenDescricoes(object sender, RoutedEventArgs e)
        {

        }

        private void OnSelectedPlanilha(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnSelectedDescricao(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnSelectedDescricaoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnSelectedComplementoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void OnAdicionarClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }

    class MovimentacaoSaidaViewModel : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
