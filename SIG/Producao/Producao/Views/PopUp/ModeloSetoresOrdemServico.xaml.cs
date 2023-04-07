using Microsoft.EntityFrameworkCore;
//using Microsoft.Office.Interop.Excel;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
//using Excel = Microsoft.Office.Interop.Excel;


namespace Producao.Views.PopUp
{
    /// <summary>
    /// Lógica interna para ModeloSetoresOrdemServico.xaml
    /// </summary>
    public partial class ModeloSetoresOrdemServico : Window
    {

        //private long? codcompladicional;
        private ModeloControleOsModel modeloControle;

        public ModeloSetoresOrdemServico(ModeloControleOsModel modeloControle)
        {
            InitializeComponent();
            this.DataContext = new ModeloSetoresOrdemServicoViewModel();
            this.modeloControle = modeloControle;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ModeloSetoresOrdemServicoViewModel vm = (ModeloSetoresOrdemServicoViewModel)DataContext;
            
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Setores = await Task.Run(vm.GetSetorsAsync);
                vm.Itens = await Task.Run(() => vm.GetSetoresProdutoAsync(modeloControle.codcompladicional));
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });
                vm.Itens.Add(new HistoricoSetorModel { selesao = false });

                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
        }

        private void dgItens_CurrentCellDropDownSelectionChanged(object sender, CurrentCellDropDownSelectionChangedEventArgs e)
        {
            var sfdatagrid = sender as SfDataGrid;
            int rowIndex = sfdatagrid.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
            var record = sfdatagrid.View.Records[rowIndex].Data as HistoricoSetorModel;
            record.selesao = true;
            record.setor = ((SetorModel)e.SelectedItem).setor;
        }

        private void dgItens_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dgItens_AddNewRowInitiating(object sender, AddNewRowInitiatingEventArgs e)
        {

        }

        private void OnAddSetor(object sender, RoutedEventArgs e)
        {
            adicionarSetor();
        }

        private async void adicionarSetor()
        {
            Window window = new()
            {
                Owner = this,
                Title = "ADICIONAR SETOR",
                WindowStyle = WindowStyle.ToolWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Content = new AddSetorOrdemServico(this.DataContext),
                Width = 300,
                Height = 300,
            };
            window.ShowDialog();
        }

        private async void OnCreateOrdemServico(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                ModeloSetoresOrdemServicoViewModel vm = (ModeloSetoresOrdemServicoViewModel)DataContext;
                
                var setor = false;
                foreach (var item in vm.Itens)
                    if (item.selesao == true) setor = true; 

                if (setor == false) 
                {
                    MessageBox.Show("Não foi selecionado nenhum setor", "Não é possível emitir Ordem de Serviço");
                    ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                    return;
                }

                vm.Planilhas = await Task.Run(() => vm.GetPlanilasReceita(modeloControle.id_modelo));
                if (vm.Planilhas.Count == 0) 
                {
                    MessageBox.Show("Não existe item na receita do modelo", "Não é possível emitir Ordem de Serviço");
                    ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                    return;
                }

                await Task.Run(() => vm.CreateOrdenServicoAsync(modeloControle));

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

        private async void OnPrintOS(object sender, RoutedEventArgs e)
        {
            ModeloSetoresOrdemServicoViewModel vm = (ModeloSetoresOrdemServicoViewModel)DataContext;

            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                //IWorkbook workbook = excelEngine.Excel.Workbooks.Open("Modelos/ORDEM_SERVICO_MODELO.xlsx", ExcelParseOptions.Default, false, "1@3mudar");
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open("Modelos/ORDEM_SERVICO_MODELO.xlsx");
                IWorksheet worksheet = workbook.Worksheets[0];

                var servicos = await Task.Run(() => vm.GetOsEmitidas(5002)); //modeloControle.num_os_produto

                IRange range = worksheet[27, 22, 53, 22];
                if (servicos.Count == 1)
                    worksheet.ShowRange(range, false);

                var pagina = 1;
                foreach (var servico in servicos)
                {
                    
                    if (pagina == 1)
                    {
                        worksheet.Range["G2"].Text = servico.num_os_produto.ToString(); 
                        worksheet.Range["I2"].Text = servico.num_os_servico.ToString();
                        worksheet.Range["B4"].Text = servico.tipo;
                        worksheet.Range["D4"].Text = $"{servico.data_inicio:dd/MM/yy}";
                        worksheet.Range["F4"].Text = $"{servico.data_fim:dd/MM/yy}";
                        worksheet.Range["B5"].Text = servico.setor_caminho;
                        worksheet.Range["F5"].Text = servico.solicitado_por;
                        worksheet.Range["G4"].Text = $"META HT: {servico.meta_peca_hora}";
                        worksheet.Range["B6"].Text = servico.planilha;
                        worksheet.Range["B7"].Text = servico.descricao_completa;
                        worksheet.Range["G7"].Text = $"{servico.data_de_expedicao:dd/MM/yy}";
                        worksheet.Range["B9"].Text = servico.quantidade.ToString(); // FORMATAR CASO NECESSÁRIO
                        worksheet.Range["D9"].Text = servico.nivel.ToString();
                        worksheet.Range["B10"].Text = servico.setor_caminho_proximo;
                        worksheet.Range["B11"].Text = servico.tema;
                        worksheet.Range["A13"].Text = servico.orientacao_caminho;
                        worksheet.Range["A17"].Text = servico.acabamento_construcao;
                        worksheet.Range["A19"].Text = servico.acabamento_fibra;
                        worksheet.Range["A21"].Text = servico.acabamento_moveis;
                        worksheet.Range["A23"].Text = servico.laco;
                        worksheet.Range["A25"].Text = servico.obs_iluminacao;

                        var setores = await Task.Run(() => vm.GetServicos(modeloControle.num_os_produto));
                        var idexSetor = 9;
                        foreach (var setor in setores)
                        {
                            worksheet.Range[$"G{idexSetor}"].Text = setor.setor_caminho_proximo;
                            idexSetor ++;
                            if (idexSetor == 17)
                                break;
                        }
                    }
                    else if(pagina == 2)
                    {

                    }

                    pagina = 2;
                }

                

                //worksheet.Range[$"E4"].Text = vm.Pedido.idpedido.ToString();
                //worksheet.ShowColumn(2, false);

                //IRange range = worksheet[27, 1, 53,1];
                worksheet.ShowRange(range, false);

                //workbook.PasswordToOpen = "sig@protect";
                //workbook.SetWriteProtectionPassword("sig@protect");
                //workbook.ReadOnlyRecommended = true;

                workbook.SaveAs(@"Impressos\ORDEM_SERVICO_MODELO.xlsx");
                worksheet.Clear();
                workbook.Close();



                Process.Start(
                    new ProcessStartInfo(@"Impressos\ORDEM_SERVICO_MODELO.xlsx")
                    {
                        Verb = "Print",
                        UseShellExecute = true,
                    });

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }

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
        public HistoricoSetorModel Iteme
        {
            get { return _item; }
            set { _item = value; RaisePropertyChanged("Iteme"); }
        }
        private ObservableCollection<HistoricoSetorModel> _itens;
        public ObservableCollection<HistoricoSetorModel> Itens
        {
            get { return _itens; }
            set { _itens = value; RaisePropertyChanged("Itens"); }
        }

        private SetorModel _setor;
        public SetorModel Setor
        {
            get { return _setor; }
            set { _setor = value; RaisePropertyChanged("Setor"); }
        }
        private ObservableCollection<SetorModel> _setores;
        public ObservableCollection<SetorModel> Setores
        {
            get { return _setores; }
            set { _setores = value; RaisePropertyChanged("Setores"); }
        }

        private OsEmissaoProducaoImprimirModel _emitida;
        public OsEmissaoProducaoImprimirModel Emitida
        {
            get { return _emitida; }
            set { _emitida = value; RaisePropertyChanged("Emitida"); }
        }
        private ObservableCollection<OsEmissaoProducaoImprimirModel> _emitidas;
        public ObservableCollection<OsEmissaoProducaoImprimirModel> Emitidas
        {
            get { return _emitidas; }
            set { _emitidas = value; RaisePropertyChanged("Emitidas"); }
        }

        private ProdutoServicoModel _serviso;
        public ProdutoServicoModel Servico
        {
            get { return _serviso; }
            set { _serviso = value; RaisePropertyChanged("Servico"); }
        }
        private ObservableCollection<ProdutoServicoModel> _servicos;
        public ObservableCollection<ProdutoServicoModel> Servicos
        {
            get { return _servicos; }
            set { _servicos = value; RaisePropertyChanged("Servicos"); }
        }

        private ObservableCollection<string> _planilhas;
        public ObservableCollection<string> Planilhas
        {
            get { return _planilhas; }
            set { _planilhas = value; RaisePropertyChanged("Planilhas"); }
        }



        public async Task<ObservableCollection<HistoricoSetorModel>> GetSetoresProdutoAsync(long? codcompladicional)
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
        public async Task<ObservableCollection<SetorModel>> GetSetorsAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await (from s in db.SetorProducaos where s.inativo == "0    " select new SetorModel { setor =  s.setor + " - " + s.galpao, codigo_setor = s.codigo_setor}).ToListAsync();
                return new ObservableCollection<SetorModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateOrdenServicoAsync(ModeloControleOsModel modeloControle)
        {
            using DatabaseContext db = new();
            using var transaction = db.Database.BeginTransaction();

            var Setores = (from e in Itens where e.selesao == true select e).ToList();

            if (Setores.Count == 0)
                throw new InvalidOperationException("Não existe setor para criar ordem de serviço.");

            var quantidade = (modeloControle?.qtd_chk_list - (int)(modeloControle?.qtd_os ?? 0));

            try
            {
                var produto = await db.Descricoes.Where(d => d.codcompladicional == modeloControle.codcompladicional).FirstOrDefaultAsync();
                var produtoOsModel = new ProdutoOsModel
                {
                    tipo = "KIT",
                    planilha = produto.planilha,
                    quantidade = (float?)quantidade,
                    responsavel_emissao = Environment.UserName,
                    data_emissao = DateTime.Now,
                    cod_produto = produto.codigo,
                    cod_desc_adicional = produto.coduniadicional,
                    cod_compl_adicional = produto.codcompladicional,
                    id_modelo = modeloControle.id_modelo
                };
                await db.ProdutoOs.AddAsync(produtoOsModel);
                await db.SaveChangesAsync();

                for (int i = 0; i < Setores.Count; i++)
                {
                    var item = Setores[i];
                    var Obs = new ObsOsModel
                    {
                        num_os_produto = produtoOsModel.num_os_produto,
                        cod_compl_adicional = produto.codcompladicional,
                        num_caminho = i + 1,
                        codigo_setor = item.codigo_setor,
                        setor_caminho = item.setor,
                        orientacao_caminho = item.observacao,
                        distribuir_os = "No setor",
                        cliente = modeloControle.sigla,
                        solicitado_por = Environment.UserName,
                        solicitado_data = DateTime.Now
                    };

                    await db.ObsOs.AddAsync(Obs);
                    await db.SaveChangesAsync();
                }
                
                var solictAberta = await db.OrdemServicoEmissaoAbertas.Where(o => o.num_os_produto == produtoOsModel.num_os_produto).ToListAsync();
                for (int i = 0; i < solictAberta.Count; i++)
                {
                    var item = solictAberta[i];
                    var teste = ((i + 1) < solictAberta.Count);
                    var produtoServicoModel = new ProdutoServicoModel
                    {
                        num_os_produto = item.num_os_produto,
                        tipo = item.tipo,
                        codigo_setor = item.codigo_setor,
                        setor_caminho = item.setor_caminho,
                        quantidade = item.quantidade,
                        data_inicio = DateTime.Now,
                        data_fim = DateTime.Now.AddDays(15),
                        cliente = item.cliente,
                        tema = item.tema,
                        orientacao_caminho = item.orientacao_caminho,
                        codigo_setor_proximo = ((i+1) < solictAberta.Count) ? solictAberta[i+1].codigo_setor : 39,
                        setor_caminho_proximo = ((i+1) < solictAberta.Count) ? solictAberta[i+1].setor_caminho : "FINAL - TODOS",
                        fase = "PRODUÇÃO",
                        responsavel_emissao_os = Environment.UserName,
                        emitida_por = Environment.UserName,
                        emitida_data = DateTime.Now,
                        turno = "DIURNO",
                        id_modelo = item.id_modelo
                    };

                    await db.ProdutoServicos.AddAsync(produtoServicoModel);
                    await db.SaveChangesAsync();


                    //ADICIONAR REQUISIÇÃO DE MATERIAL
                    if (i == 0)
                    {
                        foreach (var planilha in Planilhas)
                        {
                            if (planilha != "ESTOQUE FITAS")
                            {
                                await db.Requisicoes.AddAsync(new RequisicaoModel { num_os_servico = produtoServicoModel.num_os_servico, data = DateTime.Now, alterado_por = Environment.UserName });
                                await db.SaveChangesAsync();

                                adicinar requisicao

                                db.RequisicaoDetalhes.AddAsync(new DetalheRequisicaoModel { });

                            }
                        }
                    }
                    
                }
                
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<ObservableCollection<OsEmissaoProducaoImprimirModel>> GetOsEmitidas(long? num_os_produto)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ImprimirOsS.Where(i => i.num_os_produto == num_os_produto).ToListAsync();
                return new ObservableCollection<OsEmissaoProducaoImprimirModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<ProdutoServicoModel>> GetServicos(long? num_os_produto)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ProdutoServicos.Where(i => i.num_os_produto == num_os_produto).ToListAsync();
                return new ObservableCollection<ProdutoServicoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<string>> GetPlanilasReceita(long? id_modelo)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.qryReceitas.Where(r => r.id_modelo == id_modelo).OrderBy(g => g.planilha).GroupBy(g => new { g.planilha, g.id_modelo }).Select(p => new { p.Key.planilha }).ToListAsync();

                ObservableCollection<string> nos = new ObservableCollection<string>();
                foreach (var item in data)
                    nos.Add(item.planilha);

                return nos;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
