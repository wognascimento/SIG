﻿using Microsoft.EntityFrameworkCore;
using Producao.Views.PopUp;
using Syncfusion.UI.Xaml.Grid;
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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Producao.Views.OrdemServico.Requisicao
{
    /// <summary>
    /// Interação lógica para ViewReceitaRequisicao.xam
    /// </summary>
    public partial class ViewReceitaRequisicao : UserControl
    {

        private ReceitaRequisicaoViewModel? vm;

        public ViewReceitaRequisicao()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ReceitaRequisicaoViewModel();
            vm = (ReceitaRequisicaoViewModel)DataContext;
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.Planilhas = await Task.Run(vm.GetPlanilhasAsync);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                //txtPlanilha.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnBuscaProdutoProduto(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    string text = ((TextBox)sender).Text;
                    vm.DescricaoProduto = await Task.Run(() => vm.GetDescricaoAsync(long.Parse(text)));
                    if (vm.DescricaoProduto == null)
                    {
                        MessageBox.Show("Produto não encontrado", "Busca de produto");
                        Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                        return;
                    }
                    txtCodigoProduto.Text = vm.DescricaoProduto.codcompladicional.ToString();
                    txtProdutoReceita.Text = vm.DescricaoProduto.descricao_completa;
                    vm.Itens = await Task.Run(() => vm.GetItensAsync(vm.DescricaoProduto.codcompladicional));
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

        private async void OnOpenDescricoesProduto(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new BuscaProduto();
                window.Owner = App.Current.MainWindow;
                if (window.ShowDialog() == true)
                {
                    vm.DescricaoProduto = window.descricao;
                    txtCodigoProduto.Text = vm.DescricaoProduto.codcompladicional.ToString();
                    txtProdutoReceita.Text = vm.DescricaoProduto.descricao_completa;
                }
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.Itens = await Task.Run(() => vm.GetItensAsync(vm.DescricaoProduto.codcompladicional));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnBuscaProduto(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
                    txtCodigoProdutoReceita.Text = vm.Descricao.codcompladicional.ToString();
                    txtPlanilha.Text = vm.Descricao.planilha;
                    txtDescricao.Text = vm.Descricao.descricao;
                    txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                    txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                    txtQuantidade.Focus();

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
            if (window.ShowDialog() == true)
            {
                vm.Descricao = window.descricao;
                txtCodigoProdutoReceita.Text = vm.Descricao.codcompladicional.ToString();
                txtPlanilha.Text = vm.Descricao.planilha;
                txtDescricao.Text = vm.Descricao.descricao;
                txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                txtQuantidade.Focus();
            }
        }

        private async void OnSelectedPlanilha(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                RelplanModel? planilha = e.NewValue as RelplanModel;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(planilha?.planilha));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                txtDescricao.Focus();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnSelectedDescricao(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ProdutoModel? produto = e.NewValue as ProdutoModel;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(produto?.codigo));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                txtDescricaoAdicional.Focus();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnSelectedDescricaoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                TabelaDescAdicionalModel? adicional = e.NewValue as TabelaDescAdicionalModel;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(adicional?.coduniadicional));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                txtComplementoAdicional.Focus();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSelectedComplementoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TblComplementoAdicionalModel? complemento = e.NewValue as TblComplementoAdicionalModel;
            vm.Compledicional = complemento;
            txtCodigoProdutoReceita.Text = complemento?.codcompladicional.ToString();
            txtQuantidade.Focus();
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
                var dados = new RequisicaoReceitaModel
                {
                    codcompladicional_produto = vm.DescricaoProduto.codcompladicional,
                    codcompladicional_receita = long.Parse(txtCodigoProdutoReceita.Text),
                    quantidade = Convert.ToDouble(txtQuantidade.Text),
                    inserido_por = Environment.UserName,
                    inserido_em = DateTime.Now,
                };
                vm.RequiReceita = await Task.Run(() => vm.AddReceita(dados));
                //vm.ItensReceita = await Task.Run(() => vm.GetReceitaDetalhes(Modelo.id_modelo));
                vm.Itens = await Task.Run(() => vm.GetItensAsync(vm.DescricaoProduto.codcompladicional));
                Limpar();

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private void dgModelos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void dgModelos_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void Limpar()
        {
            txtCodigoProdutoReceita.Text = string.Empty;
            txtPlanilha.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtDescricaoAdicional.Text = string.Empty;
            txtComplementoAdicional.Text = string.Empty;
            txtQuantidade.Text = string.Empty;
            txtPlanilha.Focus();
        }

    }

    public class ReceitaRequisicaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
        private QryDescricao _descricaoProduto;
        public QryDescricao DescricaoProduto
        {
            get { return _descricaoProduto; }
            set { _descricaoProduto = value; RaisePropertyChanged("DescricaoProduto"); }
        }
        private QryDescricao _descricao;
        public QryDescricao Descricao
        {
            get { return _descricao; }
            set { _descricao = value; RaisePropertyChanged("Descricao"); }
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

        private ObservableCollection<Item> _itens;
        public ObservableCollection<Item> Itens
        {
            get { return _itens; }
            set { _itens = value; RaisePropertyChanged("Itens"); }
        }

        private RequisicaoReceitaModel _requiReceita;
        public RequisicaoReceitaModel RequiReceita
        {
            get { return _requiReceita; }
            set { _requiReceita = value; RaisePropertyChanged("RequiReceita"); }
        }

        

        public async Task<ObservableCollection<RelplanModel>> GetPlanilhasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Relplans
                    .OrderBy(c => c.planilha)
                    .Where(c => c.ativo.Equals("1"))
                    .ToListAsync();
                return new ObservableCollection<RelplanModel>(data);
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

        public async Task<RequisicaoReceitaModel> AddReceita(RequisicaoReceitaModel receita)
        {
            try
            {
                using DatabaseContext db = new();
                await db.RequisicaoReceitas.SingleMergeAsync(receita);
                await db.SaveChangesAsync();
                return receita;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<Item>> GetItensAsync(long? codcompladicional_produto)
        {
            try
            {
                using DatabaseContext db = new();
                /*
                var data = await db.ComplementoAdicionais
                    .OrderBy(c => c.complementoadicional)
                    .Where(c => c.coduniadicional.Equals(coduniadicional))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                */
                
                var data = db.RequisicaoReceitas
                    .Join(db.Descricoes, o => o.codcompladicional_receita, i => i.codcompladicional, (o, i) =>
                    new Item
                    {
                        id = o.id,
                        codcompladicional_receita = o.codcompladicional_receita,
                        planilha = i.planilha,
                        descricao_completa = i.descricao_completa,
                        unidade = i.unidade,
                        quantidade = o.quantidade,
                        inserido_por = o.inserido_por,
                        inserido_em = o.inserido_em

                    }).Take(5);
                
                /*
                var data = (from t in db.RequisicaoReceitas
                             join il in db.Descricoes
                             on t.codcompladicional_receita equals il.codcompladicional
                             where t.codcompladicional_produto == codcompladicional_produto
                             select (new Item
                             {
                                 id = t.id,
                                 codcompladicional_receita = t.codcompladicional_receita,
                                 planilha = il.planilha,
                                 descricao_completa = il.descricao_completa,
                                 unidade = il.unidade,
                                 quantidade = t.quantidade,
                                 inserido_por = t.inserido_por,
                                 inserido_em = t.inserido_em
                             })
                    ).ToListAsync();
                */
                return new ObservableCollection<Item>((IEnumerable<Item>)data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class Item
    {
        public long? id { get; set; }
        public long? codcompladicional_receita { get; set; }
        public string? planilha { get; set; }
        public string? descricao_completa { get; set;}
        public string? unidade { get; set; }
        public double? quantidade { get; set; }
        public string? inserido_por { get; set; }
        public DateTime? inserido_em { get; set; }
    }
}
