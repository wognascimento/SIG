using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Producao.Views.PopUp
{
    /// <summary>
    /// Lógica interna para ModeloReceita.xaml
    /// </summary>
    public partial class ModeloReceita : Window
    {
        private QryModeloModel Modelo { get; set; }
        private QryReceitaDetalheCriadoModel Receita { get; set; }

        public ModeloReceita(QryModeloModel Modelo)
        {
            InitializeComponent();
            this.Modelo = Modelo;
            this.DataContext = new ModeloReceitaViewModel();

            if (Modelo?.planilha == "FIADA")
            {
                btnModeloFiada.Visibility = Visibility.Visible;
                GBQtdFiadas.Visibility = Visibility.Visible;
            }
            /*else
            {
                btnModeloFiada.Visibility = Visibility.Collapsed;
                GBQtdFiadas.Visibility = Visibility.Collapsed;
            }*/
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                vm.ItensReceita = await Task.Run(() => vm.GetReceitaDetalhes(Modelo.id_modelo));
                vm.Planilhas = await Task.Run(vm.GetPlanilhasAsync);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                txtPlanilha.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnBuscaProduto(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
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
                    txtCodigoProduto.Text = vm.Descricao.codcompladicional.ToString();
                    txtPlanilha.Text = vm.Descricao.planilha;
                    txtDescricao.Text = vm.Descricao.descricao;
                    txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                    txtComplementoAdicional.Text = vm.Descricao.complementoadicional;

                    vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(vm.Descricao.planilha));
                    vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(vm.Descricao.codigo));
                    vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(vm.Descricao.coduniadicional));

                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                    txtObservacao.Focus();
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

        private async void OnOpenDescricoes(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                var window = new BuscaProduto();
                window.Owner = App.Current.MainWindow;
                if (window.ShowDialog() == true)
                {
                    vm.Descricao = window.descricao;

                    txtCodigoProduto.Text = vm.Descricao.codcompladicional.ToString();
                    txtPlanilha.Text = vm.Descricao.planilha;
                    txtDescricao.Text = vm.Descricao.descricao;
                    txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                    txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                    txtObservacao.Focus();

                    vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(vm.Descricao.planilha));
                    vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(vm.Descricao.codigo));
                    vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(vm.Descricao.coduniadicional));
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
           
        }

        private async void OnSelectedPlanilha(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                RelplanModel? planilha = e.NewValue as RelplanModel;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(planilha?.planilha));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtDescricao.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnSelectedDescricao(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                ProdutoModel? produto = e.NewValue as ProdutoModel;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(produto?.codigo));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtDescricaoAdicional.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnSelectedDescricaoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                TabelaDescAdicionalModel? adicional = e.NewValue as TabelaDescAdicionalModel;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(adicional?.coduniadicional));
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                txtComplementoAdicional.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSelectedComplementoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
            TblComplementoAdicionalModel? complemento = e.NewValue as TblComplementoAdicionalModel;
            vm.Compledicional = complemento;
            txtCodigoProduto.Text = complemento?.codcompladicional.ToString();
            txtObservacao.Focus();
        }

        private void OnLimparClick(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private void Limpar()
        {
            txtCodigoProduto.Text = string.Empty;
            txtPlanilha.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtDescricaoAdicional.Text = string.Empty;
            txtComplementoAdicional.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            txtQtdModelo.Value = double.NaN;
            txtQtdProducao.Value = double.NaN;
            mod01.Value = null;
            mod02.Value = null;
            mod03.Value = null;
            mod04.Value = null;
            mod05.Value = null;
            mod06.Value = null;
            mod07.Value = null;
            mod08.Value = null;
            mod09.Value = null;
            mod10.Value = null;
            Receita = new QryReceitaDetalheCriadoModel();
            txtPlanilha.Focus();
        }

        private async void OnAdicionarClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;

                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                var dados = new ModeloReceitaModel
                {
                    id_linha = Receita?.id_linha,
                    id_modelo = Modelo.id_modelo,
                    codcompladicional = long.Parse(txtCodigoProduto.Text),
                    qtd_modelo = txtQtdModelo.Value,
                    qtd_producao = txtQtdProducao.Value,
                    observacao = txtObservacao.Text,
                    cadastrado_por = Environment.UserName,
                    data_cadastro = DateTime.Now,
                    mod1 = (int?)mod01.Value,
                    mod2 = (int?)mod02.Value,
                    mod3 = (int?)mod03.Value,
                    mod4 = (int?)mod04.Value,
                    mod5 = (int?)mod05.Value,
                    mod6 = (int?)mod06.Value,
                    mod7 = (int?)mod07.Value,
                    mod8 = (int?)mod08.Value,
                    mod9 = (int?)mod09.Value,
                    mod10 = (int?)mod10.Value,
                };
                vm.ModeloReceita = await Task.Run(() => vm.AddReceita(dados));
                vm.ItensReceita = await Task.Run(() => vm.GetReceitaDetalhes(Modelo.id_modelo));
                Limpar();
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

        private async void OnExcluirClick(object sender, RoutedEventArgs e)
        {
            if (Receita == null)
            {
                MessageBox.Show("Precisa selecionar uma linha para excluir", "Deletar item", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show("Deseja deletar o item selecionado?", "Deletar item", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.No)
            { return; }

            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;

                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                var dados = new ModeloReceitaModel
                {
                    id_linha = Receita?.id_linha,
                    id_modelo = Modelo.id_modelo,
                    codcompladicional = long.Parse(txtCodigoProduto.Text),
                    qtd_modelo = txtQtdModelo.Value,
                    qtd_producao = txtQtdProducao.Value,
                    observacao = txtObservacao.Text,
                    cadastrado_por = Environment.UserName,
                    data_cadastro = DateTime.Now,
                };
                await Task.Run(() => vm.ExcluirAsync(dados));
                vm.ItensReceita = await Task.Run(() => vm.GetReceitaDetalhes(Modelo.id_modelo));
                Limpar();
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

        private void OnPrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;

                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = application.Workbooks.Open("Modelos/RECEITA_CENTRAL_MODELO.xlsx");
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Range["C2"].Text = Modelo.id_modelo.ToString();
                worksheet.Range["C3"].Text = Modelo.planilha;
                worksheet.Range["C4"].Text = Modelo.descricao_completa;
                worksheet.Range["C5"].Text = Modelo.tema;
                worksheet.Range["A7"].Text = Modelo.obs_modelo;
                worksheet.Range["H4"].Text = Modelo.multiplica.ToString();

                //var itens = (from i in vm.ItensReceita where i.quantidade > 0 select new { i.quantidade, i.planilha, i.descricao_completa, i.unidade }).ToList(); //new { a.Name, a.Age }
                var index = 9;

                foreach (var item in vm.ItensReceita)
                {
                    worksheet.Range[$"A{index}"].Text = item.codcompladicional.ToString();
                    worksheet.Range[$"A{index}"].CellStyle.Font.FontName = "Arial";
                    worksheet.Range[$"A{index}"].CellStyle.Font.Size = 8;

                    worksheet.Range[$"B{index}"].Text = item.planilha;
                    worksheet.Range[$"B{index}"].CellStyle.Font.FontName = "Arial";
                    worksheet.Range[$"B{index}"].CellStyle.Font.Size = 8;

                    worksheet.Range[$"C{index}"].Text = item.descricao_completa;
                    worksheet.Range[$"C{index}"].CellStyle.Font.FontName = "Arial";
                    worksheet.Range[$"C{index}"].CellStyle.Font.Size = 8;
                    worksheet.Range[$"C{index}:E{index}"].Merge();
                    worksheet.Range[$"C{index}:E{index}"].WrapText = true;

                    worksheet.Range[$"F{index}"].Text = item.observacao;
                    worksheet.Range[$"F{index}"].CellStyle.Font.FontName = "Arial";
                    worksheet.Range[$"F{index}"].CellStyle.Font.Size = 8;
                    //worksheet.Range[$"F{index}:G{index}"].Merge();
                    worksheet.Range[$"F{index}"].WrapText = true;

                    worksheet.Range[$"G{index}"].Text = item.qtd_modelo.ToString();
                    worksheet.Range[$"G{index}"].CellStyle.Font.FontName = "Arial";
                    worksheet.Range[$"G{index}"].CellStyle.Font.Size = 8;
                    worksheet.Range[$"G{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                    worksheet.Range[$"H{index}"].Text = item.qtd_producao.ToString();
                    worksheet.Range[$"H{index}"].CellStyle.Font.FontName = "Arial";
                    worksheet.Range[$"H{index}"].CellStyle.Font.Size = 8;
                    worksheet.Range[$"H{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    index++;
                }

                workbook.SaveAs($"Impressos/RECEITA_CENTRAL_MODELO_{Modelo.id_modelo}.xlsx");
                workbook.Close();

                Process.Start(new ProcessStartInfo($"Impressos\\RECEITA_CENTRAL_MODELO_{Modelo.id_modelo}.xlsx")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnModelosFiadaClick(object sender, RoutedEventArgs e)
        {
            var window = new ModeloFiada(Modelo);
            window.Owner = App.Current.MainWindow;
            window.ShowDialog();
        }

        private async void OnCopyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
                var window = new ModeloReceitaCopiar(Modelo);
                window.Owner = App.Current.MainWindow;
                if (window.ShowDialog() == true)
                {

                    var dados = window.itens;
                    if (vm.ItensReceita.Count > 0)
                    {
                        var result = MessageBox.Show("Existem itens no Modelo, desejá copiar assim mesmo?", "Copia de receita", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                            ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                            await Task.Run(() => vm.AddCopiaReceita(dados));
                        }
                        else
                            MessageBox.Show("COPIA DE ITENS CANCELADA!!!");
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                        ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                        await Task.Run(() => vm.AddCopiaReceita(dados));
                    }
                }

                vm.ItensReceita = await Task.Run(() => vm.GetReceitaDetalhes(Modelo.id_modelo));
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

        private async void dgModelos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Receita = (QryReceitaDetalheCriadoModel)dgModelos.SelectedItem;
            ModeloReceitaViewModel? vm = (ModeloReceitaViewModel)DataContext;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.Descricao = await Task.Run(() => vm.GetDescricaoAsync((long)Receita?.codcompladicional));
                txtCodigoProduto.Text = vm.Descricao.codcompladicional.ToString();
                txtPlanilha.Text = vm.Descricao.planilha;
                txtDescricao.Text = vm.Descricao.descricao;
                txtDescricaoAdicional.Text = vm.Descricao.descricao_adicional;
                txtComplementoAdicional.Text = vm.Descricao.complementoadicional;
                txtObservacao.Text = Receita?.observacao;
                txtQtdModelo.Value = Receita?.qtd_modelo;
                txtQtdProducao.Value = Receita?.qtd_producao;

                vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(vm.Descricao.planilha));
                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(vm.Descricao.codigo));
                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(vm.Descricao.coduniadicional));

                mod01.Value = Receita?.mod1;
                mod02.Value = Receita?.mod2;
                mod03.Value = Receita?.mod3;
                mod04.Value = Receita?.mod4;
                mod05.Value = Receita?.mod5;
                mod06.Value = Receita?.mod6;
                mod07.Value = Receita?.mod7;
                mod08.Value = Receita?.mod8;
                mod09.Value = Receita?.mod9;
                mod10.Value = Receita?.mod10;

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

    public class ModeloReceitaViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private QryReceitaDetalheCriadoModel _itemReceita;
        public QryReceitaDetalheCriadoModel ItemReceita
        {
            get { return _itemReceita; }
            set { _itemReceita = value; RaisePropertyChanged("ItemReceita"); }
        }
        private ObservableCollection<QryReceitaDetalheCriadoModel> _itensReceita;
        public ObservableCollection<QryReceitaDetalheCriadoModel> ItensReceita
        {
            get { return _itensReceita; }
            set { _itensReceita = value; RaisePropertyChanged("ItensReceita"); }
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

        private QryDescricao _descricao;
        public QryDescricao Descricao
        {
            get { return _descricao; }
            set { _descricao = value; RaisePropertyChanged("Descricao"); }
        }

        private ModeloReceitaModel _modeloReceita;
        public ModeloReceitaModel ModeloReceita
        {
            get { return _modeloReceita; }
            set { _modeloReceita = value; RaisePropertyChanged("ModeloReceita"); }
        }

        public async Task<ObservableCollection<QryReceitaDetalheCriadoModel>> GetReceitaDetalhes(long? idmodelo)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.qryReceitas.Where(c => c.id_modelo == idmodelo).ToListAsync();
                return new ObservableCollection<QryReceitaDetalheCriadoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<QryDescricao> GetDescricaoAsync(long codcompladicional)
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

        public async Task<ModeloReceitaModel> AddReceita(ModeloReceitaModel receita)
        {
            try
            {
                using DatabaseContext db = new();
                await db.ReceitaModelos.SingleMergeAsync(receita);
                await db.SaveChangesAsync();
                return receita;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ExcluirAsync(ModeloReceitaModel receita)
        {
            try
            {
                
                using DatabaseContext db = new();
                db.ReceitaModelos.Attach(receita);
                db.ReceitaModelos.Remove(receita);
                await db.SaveChangesAsync();
                /*
                db.ReceitaModelos.Remove(receita);
                await db.SaveChangesAsync();
                */
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddCopiaReceita(ObservableCollection<ModeloReceitaModel> receita)
        {
            using DatabaseContext db = new();
            using var transaction = db.Database.BeginTransaction();

            try
            {
                foreach (var item in receita)
                {
                    await db.ReceitaModelos.SingleMergeAsync(item);
                    await db.SaveChangesAsync();
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }


    }
}
