using Microsoft.EntityFrameworkCore;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Utility;
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

namespace Producao.Views.CentralModelos
{
    /// <summary>
    /// Interação lógica para ViewCentralEmitirOs.xam
    /// </summary>
    public partial class ViewCentralEmitirOs : UserControl
    {
        public ViewCentralEmitirOs()
        {
            InitializeComponent();
            this.DataContext = new ViewCentralEmitirOsViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewCentralEmitirOsViewModel vm = (ViewCentralEmitirOsViewModel)DataContext;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.Itens = await Task.Run(vm.GetItensAsync);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }
    }

    public class ViewCentralEmitirOsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ModeloControleOsModel item;
        public ModeloControleOsModel Item
        {
            get { return item; }
            set { item = value; RaisePropertyChanged("Item"); }
        }

        private ObservableCollection<ModeloControleOsModel> itens;
        public ObservableCollection<ModeloControleOsModel> Itens
        {
            get { return itens; }
            set { itens = value; RaisePropertyChanged("Itens"); }
        }

        private ObservableCollection<DistribuicaoPAModel> distribuicao;
        public ObservableCollection<DistribuicaoPAModel> Distribuicao
        {
            get { return distribuicao; }
            set { distribuicao = value; RaisePropertyChanged("Distribuicao"); }
        }

        public async Task<ObservableCollection<ModeloControleOsModel>> GetItensAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.modeloControleOs.ToListAsync();
                return new ObservableCollection<ModeloControleOsModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<DistribuicaoPAModel>> GetDistribuicoesAsync(long? id_modelo, string? cliente)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.DistribuicaoPAs.Where(d =>  d.id_modelo == id_modelo && d.cliente == cliente).ToListAsync();
                return new ObservableCollection<DistribuicaoPAModel>(data);
            }
            catch (Exception)
            {
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

    }

    public static class ContextMenuCommands
    {
        static BaseCommand? createOS;
        public static BaseCommand CreateOS
        {
            get
            {
                if (createOS == null)
                    createOS = new BaseCommand(OnCreateOSClicked);
                return createOS;
            }
        }
        private static void OnCreateOSClicked(object obj)
        {

            //var Record = { Producao.ModeloControleOsModel}
            //obj = {Syncfusion.UI.Xaml.Grid.GridRecordContextMenuInfo}
            var record = ((GridRecordContextMenuInfo)obj).Record as ModeloControleOsModel;
            var grid = ((GridRecordContextMenuInfo)obj).DataGrid;
            var item = grid.SelectedItem as ModeloControleOsModel;
            //if (record?.qtd_chk_list > (int)(record?.qtd_os ?? 0))
            //{
                try
                {
                    //var dif = (record?.qtd_chk_list - (int)(record?.qtd_os ?? 0));
                    var dif = record?.qtd_chk_list;
                    var window = new ModeloSetoresOrdemServico(record);
                    window.Owner = App.Current.MainWindow;
                    window.ShowDialog();
                    
                    /*Window window = new Window();
                    window.Content = new ModeloSetoresOrdemServico(747); //item?.codcompladicional
                    window.Owner = App.Current.MainWindow;
                    window.Title = "SETORES PARA EMISSÃO DA OERDEM DE SERVIÇO";
                    window.WindowStyle = WindowStyle.ToolWindow; //"ToolWindow"
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner; //"CenterOwner" 
                    window.ResizeMode = ResizeMode.NoResize; //"NoResize"
                    window.Height = 450;
                    window.Width = 500;
                    window.ShowDialog();*/


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            //}
            //else
            //{
            //    MessageBox.Show("Quantidade indisponivel para Gerar ordem de serviço.");
            //}
        }


        static BaseCommand? reimprimirOS;
        public static BaseCommand ReimprimirOS
        {
            get
            {
                if (reimprimirOS == null)
                    reimprimirOS = new BaseCommand(OnReimprimirOSClicked);
                return reimprimirOS;
            }
        }
        private async static void OnReimprimirOSClicked(object obj)
        {
            var grid = ((GridRecordContextMenuInfo)obj).DataGrid;
            var item = grid.SelectedItem as ModeloControleOsModel;
            try
            {
                ViewCentralEmitirOsViewModel vm = (ViewCentralEmitirOsViewModel)grid.DataContext;
                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open("Modelos/ORDEM_SERVICO_MODELO.xlsx");
                IWorksheet worksheet = workbook.Worksheets[0];

                var servicos = await Task.Run(() => vm.GetOsEmitidas(item.num_os_produto));
                
                IRange range = worksheet[27, 23, 53, 23];
                if (servicos.Count == 1)
                    worksheet.ShowRange(range, false);
                
                var pagina = 1;
                foreach (var servico in servicos)
                {

                    if (pagina == 1)
                    {
                        
                        worksheet.Range["E2"].Text = servico.cliente;
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

                        var setores = await Task.Run(() => vm.GetServicos(item.num_os_produto));
                        var idexSetor = 9;
                        foreach (var setor in setores)
                        {
                            worksheet.Range[$"G{idexSetor}"].Text = setor.setor_caminho_proximo;
                            idexSetor++;
                            if (idexSetor == 17)
                                break;
                        }
                    }
                    else if (pagina == 2)
                    {
                        worksheet.Range["E30"].Text = servico.cliente;
                        worksheet.Range["G30"].Text = servico.num_os_produto.ToString();
                        worksheet.Range["I30"].Text = servico.num_os_servico.ToString();
                        worksheet.Range["B32"].Text = servico.tipo;
                        worksheet.Range["D32"].Text = $"{servico.data_inicio:dd/MM/yy}";
                        worksheet.Range["F32"].Text = $"{servico.data_fim:dd/MM/yy}";
                        worksheet.Range["B33"].Text = servico.setor_caminho;
                        worksheet.Range["F33"].Text = servico.solicitado_por;
                        worksheet.Range["G32"].Text = $"META HT: {servico.meta_peca_hora}";
                        worksheet.Range["B34"].Text = servico.planilha;
                        worksheet.Range["B35"].Text = servico.descricao_completa;
                        worksheet.Range["G35"].Text = $"{servico.data_de_expedicao:dd/MM/yy}";
                        worksheet.Range["B37"].Text = servico.quantidade.ToString();
                        worksheet.Range["D37"].Text = servico.nivel.ToString();
                        worksheet.Range["B38"].Text = servico.setor_caminho_proximo;
                        worksheet.Range["B39"].Text = servico.tema;
                        worksheet.Range["A41"].Text = servico.orientacao_caminho;
                        worksheet.Range["A45"].Text = servico.acabamento_construcao;
                        worksheet.Range["A47"].Text = servico.acabamento_fibra;
                        worksheet.Range["A49"].Text = servico.acabamento_moveis;
                        worksheet.Range["A51"].Text = servico.laco;
                        worksheet.Range["A53"].Text = servico.obs_iluminacao;

                        var setores = await Task.Run(() => vm.GetServicos(item.num_os_produto));
                        var idexSetor = 37;
                        foreach (var setor in setores)
                        {
                            worksheet.Range[$"G{idexSetor}"].Text = setor.setor_caminho_proximo;
                            idexSetor++;
                            if (idexSetor == 17)
                                break;
                        }
                    }

                    pagina = 2;
                }
                //worksheet.ShowRange(range, false);
                workbook.SaveAs(@"Impressos\ORDEM_SERVICO_MODELO.xlsx");
                worksheet.Clear();
                workbook.Close();

                Process.Start(
                    new ProcessStartInfo(@"Impressos\ORDEM_SERVICO_MODELO.xlsx")
                    {
                        Verb = "Print",
                        UseShellExecute = true,
                    });
            }
            catch (Exception)
            {
                throw;
            }
        }


        static BaseCommand? tabelaPAExcel;
        public static BaseCommand TabelaPAExcel
        {
            get
            {
                if (tabelaPAExcel == null)
                    tabelaPAExcel = new BaseCommand(OnTabelaPAExcelClicked);
                return tabelaPAExcel;
            }
        }
        private async static void OnTabelaPAExcelClicked(object obj)
        {
            var grid = ((GridRecordContextMenuInfo)obj).DataGrid;
            var item = grid.SelectedItem as ModeloControleOsModel;
            if (item?.planilha != "KIT ENF PA")
            {
                MessageBox.Show("Produto não é uma P.A");
                return;
            }


            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                ViewCentralEmitirOsViewModel vm = (ViewCentralEmitirOsViewModel)grid.DataContext;
                DataBaseSettings BaseSettings = DataBaseSettings.Instance;
                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.IsGridLinesVisible = false;

                IStyle headerStyle;
                IStyle bodyStyle;

                bodyStyle = workbook.Styles.Add("BodyStyle");
                bodyStyle.BeginUpdate();
                bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                bodyStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Grey_25_percent;
                bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Grey_25_percent;
                bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].Color = ExcelKnownColors.Grey_25_percent;
                bodyStyle.Borders[ExcelBordersIndex.EdgeRight].Color = ExcelKnownColors.Grey_25_percent;
                //bodyStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                //bodyStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                //bodyStyle.Font.Bold = true;
                //bodyStyle.WrapText = true;
                bodyStyle.EndUpdate();

                headerStyle = workbook.Styles.Add("headerStyle");
                headerStyle.BeginUpdate();
                headerStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                headerStyle.Borders[ExcelBordersIndex.EdgeTop].Color = ExcelKnownColors.Grey_25_percent;
                headerStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Grey_25_percent;
                headerStyle.Borders[ExcelBordersIndex.EdgeLeft].Color = ExcelKnownColors.Grey_25_percent;
                headerStyle.Borders[ExcelBordersIndex.EdgeRight].Color = ExcelKnownColors.Grey_25_percent;
                headerStyle.Font.Bold = true;
                //headerStyle.WrapText = true;
                headerStyle.EndUpdate();

                worksheet.Range["A1"].Text = $"{item.descricao_completa}";
                worksheet.Range["A1:B1"].Merge();
                worksheet.Range["A1:B1"].CellStyle = headerStyle;

                worksheet.Range["A2"].Text = $"TEMA: {item.tema}";
                worksheet.Range["A2:B2"].Merge();
                worksheet.Range["A2:B2"].CellStyle = headerStyle;

                worksheet.Range["A3"].Text = $"CLIENTE: {item.sigla}";
                worksheet.Range["A3:B3"].Merge();
                worksheet.Range["A3:B3"].CellStyle = headerStyle;

                worksheet.Range["A4"].Text = $"COD.";
                worksheet.Range["A4"].CellStyle = headerStyle;
                worksheet.Range["B4"].Text = $"DESCRIÇÃO";
                worksheet.Range["B4"].CellStyle = headerStyle;
                worksheet.Range["C4"].Text = $"QUANT.";
                worksheet.Range["C4"].CellStyle = headerStyle;
                worksheet.Range["D4"].Text = $"PONGA";
                worksheet.Range["D4"].CellStyle = headerStyle;
                worksheet.Range["E4"].Text = $"TRIPÉ";
                worksheet.Range["E4"].CellStyle = headerStyle;
                worksheet.Range["F4"].Text = $"1º ANEL";
                worksheet.Range["F4"].CellStyle = headerStyle;
                worksheet.Range["G4"].Text = $"2º ANEL";
                worksheet.Range["G4"].CellStyle = headerStyle;
                worksheet.Range["H4"].Text = $"3º ANEL";
                worksheet.Range["H4"].CellStyle = headerStyle;
                worksheet.Range["I4"].Text = $"4º ANEL";
                worksheet.Range["I4"].CellStyle = headerStyle;
                worksheet.Range["J4"].Text = $"5º ANEL";
                worksheet.Range["J4"].CellStyle = headerStyle;
                worksheet.Range["K4"].Text = $"6º ANEL";
                worksheet.Range["K4"].CellStyle = headerStyle;
                worksheet.Range["L4"].Text = $"7º ANEL";
                worksheet.Range["L4"].CellStyle = headerStyle;
                worksheet.Range["M4"].Text = $"8º ANEL";
                worksheet.Range["M4"].CellStyle = headerStyle;
                worksheet.Range["N4"].Text = $"9º ANEL";
                worksheet.Range["N4"].CellStyle = headerStyle;
                worksheet.Range["O4"].Text = $"10º ANEL";
                worksheet.Range["O4"].CellStyle = headerStyle;
                worksheet.Range["P4"].Text = $"11º ANEL";
                worksheet.Range["P4"].CellStyle = headerStyle;
                worksheet.Range["Q4"].Text = $"12º ANEL";
                worksheet.Range["Q4"].CellStyle = headerStyle;
                worksheet.Range["R4"].Text = $"13º ANEL";
                worksheet.Range["R4"].CellStyle = headerStyle;
                worksheet.Range["S4"].Text = $"14º ANEL";
                worksheet.Range["S4"].CellStyle = headerStyle;
                worksheet.Range["T4"].Text = $"15º ANEL";
                worksheet.Range["T4"].CellStyle = headerStyle;
                worksheet.Range["U4"].Text = $"16º ANEL";
                worksheet.Range["U4"].CellStyle = headerStyle;
                worksheet.Range["V4"].Text = $"17º ANEL";
                worksheet.Range["V4"].CellStyle = headerStyle;
                worksheet.Range["W4"].Text = $"18º ANEL";
                worksheet.Range["W4"].CellStyle = headerStyle;
                worksheet.Range["X4"].Text = $"19º ANEL";
                worksheet.Range["X4"].CellStyle = headerStyle;
                worksheet.Range["Y4"].Text = $"20º ANEL";
                worksheet.Range["Y4"].CellStyle = headerStyle;
                worksheet.Range["Z4"].Text = $"21º ANEL";
                worksheet.Range["Z4"].CellStyle = headerStyle;
                worksheet.Range["AA4"].Text = $"22º ANEL";
                worksheet.Range["AA4"].CellStyle = headerStyle;


                //UsedRange excludes the blank cells
                worksheet.UsedRangeIncludesFormatting = false;

                //Adding border to highlight the used range
                worksheet.UsedRange.BorderAround();

                vm.Distribuicao = await Task.Run(async () => await vm.GetDistribuicoesAsync(item.id_modelo, item.sigla));
                int linha = 5;
                foreach (DistribuicaoPAModel dist in vm.Distribuicao)
                {
                    worksheet.Range[$"A{linha}"].Number = (double)dist.codcompladicional;
                    worksheet.Range[$"A{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"B{linha}"].Text = dist.descricao_produto;
                    worksheet.Range[$"B{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"C{linha}"].Number = (double)(dist?.qtd);
                    worksheet.Range[$"C{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"D{linha}"].Number = (double)dist.p;
                    worksheet.Range[$"D{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"E{linha}"].Number = (double)dist.t;
                    worksheet.Range[$"E{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"F{linha}"].Text = dist.anel1.ToString();
                    worksheet.Range[$"F{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"G{linha}"].Text  = dist.anel2.ToString();
                    worksheet.Range[$"G{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"H{linha}"].Text  =dist.anel3.ToString();
                    worksheet.Range[$"H{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"I{linha}"].Text  =dist.anel4.ToString();
                    worksheet.Range[$"I{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"J{linha}"].Text  =dist.anel5.ToString();
                    worksheet.Range[$"J{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"K{linha}"].Text  =dist.anel6.ToString();
                    worksheet.Range[$"K{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"L{linha}"].Text  =dist.anel7.ToString();
                    worksheet.Range[$"L{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"M{linha}"].Text = dist?.anel8.ToString();
                    worksheet.Range[$"M{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"N{linha}"].Text  =dist?.anel9.ToString();
                    worksheet.Range[$"N{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"O{linha}"].Text  =dist?.anel10.ToString();
                    worksheet.Range[$"O{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"P{linha}"].Text  =dist?.anel11.ToString();
                    worksheet.Range[$"P{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"Q{linha}"].Text  =dist?.anel12.ToString();
                    worksheet.Range[$"Q{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"R{linha}"].Text  =dist?.anel13.ToString();
                    worksheet.Range[$"R{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"S{linha}"].Text  =dist?.anel14.ToString();
                    worksheet.Range[$"S{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"T{linha}"].Text  =dist?.anel15.ToString();
                    worksheet.Range[$"T{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"U{linha}"].Text  =dist?.anel16.ToString();
                    worksheet.Range[$"U{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"V{linha}"].Text  =dist?.anel17.ToString();
                    worksheet.Range[$"V{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"W{linha}"].Text  =dist?.anel18.ToString();
                    worksheet.Range[$"W{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"X{linha}"].Text  =dist?.anel19.ToString();
                    worksheet.Range[$"X{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"Y{linha}"].Text  =dist?.anel20.ToString();
                    worksheet.Range[$"Y{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"Z{linha}"].Text  =dist?.anel21.ToString();
                    worksheet.Range[$"Z{linha}"].CellStyle = bodyStyle;
                    worksheet.Range[$"AA{linha}"].Text  =dist?.anel22.ToString();
                    worksheet.Range[$"AA{linha}"].CellStyle = bodyStyle;
                    linha++;
                }

                worksheet.Range["C1"].Text = $"TABELA DE DISTRIBUIÇÃO DE ENFEITES POR ANEL";
                worksheet.Range["$C$1:$AA$3"].Merge();
                worksheet.Range["$C$1:$AA$3"].CellStyle = headerStyle;
                worksheet.Range["$C$1:$AA$3"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range["$C$1:$AA$3"].VerticalAlignment = ExcelVAlign.VAlignCenter;



                worksheet.Range[$"A{linha}:B{linha}"].Merge();
                worksheet.Range[$"A{linha}:B{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"A{linha}"].Text = $"TOTAL DE CAIXAS";
                worksheet.Range[$"A{linha}"].HorizontalAlignment = ExcelHAlign.HAlignRight;
                worksheet.Range[$"A{linha}"].VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"B{linha}"].Text = $"";
                worksheet.Range[$"B{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"C{linha}"].Text = $"";
                worksheet.Range[$"C{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"D{linha}"].Text = $"";
                worksheet.Range[$"D{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"E{linha}"].Text = $"";
                worksheet.Range[$"E{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"F{linha}"].Text = $"";
                worksheet.Range[$"F{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"G{linha}"].Text = $"";
                worksheet.Range[$"G{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"H{linha}"].Text = $"";
                worksheet.Range[$"H{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"I{linha}"].Text = $"";
                worksheet.Range[$"I{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"J{linha}"].Text = $"";
                worksheet.Range[$"J{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"K{linha}"].Text = $"";
                worksheet.Range[$"K{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"L{linha}"].Text = $"";
                worksheet.Range[$"L{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"M{linha}"].Text = $"";
                worksheet.Range[$"M{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"N{linha}"].Text = $"";
                worksheet.Range[$"N{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"O{linha}"].Text = $"";
                worksheet.Range[$"O{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"P{linha}"].Text = $"";
                worksheet.Range[$"P{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"Q{linha}"].Text = $"";
                worksheet.Range[$"Q{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"R{linha}"].Text = $"";
                worksheet.Range[$"R{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"S{linha}"].Text = $"";
                worksheet.Range[$"S{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"T{linha}"].Text = $"";
                worksheet.Range[$"T{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"U{linha}"].Text = $"";
                worksheet.Range[$"U{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"V{linha}"].Text = $"";
                worksheet.Range[$"V{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"W{linha}"].Text = $"";
                worksheet.Range[$"W{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"X{linha}"].Text = $"";
                worksheet.Range[$"X{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"Y{linha}"].Text = $"";
                worksheet.Range[$"Y{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"Z{linha}"].Text = $"";
                worksheet.Range[$"Z{linha}"].CellStyle = headerStyle;
                worksheet.Range[$"AA{linha}"].Text = $"";
                worksheet.Range[$"AA{linha}"].CellStyle = headerStyle;




                /*
                worksheet.Range["A2"].Text = $"ITEM";
                worksheet.Range["A2"].ColumnWidth = 5;

                worksheet.Range["B2"].Text = $"LOCAL";
                worksheet.Range["B2"].ColumnWidth = 20;

                worksheet.Range["C2"].Text = $"FAMÍLIA DE PRODUTO PLANILHA";
                worksheet.Range["C2"].ColumnWidth = 20;
                worksheet.Range["C2"].WrapText = true;

                worksheet.Range["D2"].Text = $"DESCRIÇÃO";
                worksheet.Range["D2"].ColumnWidth = 45;
                worksheet.Range["D2"].WrapText = true;

                worksheet.Range["E2"].Text = $"UNID";
                worksheet.Range["E2"].ColumnWidth = 5;

                worksheet.Range["F2"].Text = $"QTDE";
                worksheet.Range["F2"].ColumnWidth = 5;

                worksheet.Range["G2"].Text = $"ORIENTAÇÃO DE MONTAGEM";
                worksheet.Range["G2"].ColumnWidth = 30;

                worksheet.Range["H2"].Text = $"COD DETALHES COMPL";
                worksheet.Range["H2"].ColumnWidth = 10;
                worksheet.Range["H2"].WrapText = true;

                worksheet.Rows[1].CellStyle = bodyStyle;

                await Task.Run(async () => await vm.GetChkGeralRelatorioAsync());
                worksheet.ImportData(vm.ChkGeralRelatorios, 3, 1, false);

                worksheet.Range[$"A3:H{vm.ChkGeralRelatorios.Count + 2}"].CellStyle = headerStyle;

                worksheet.Range[$"A3:A{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[$"A3:A{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"B3:B{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"C3:C{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"D3:D{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"E3:E{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[$"E3:E{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"F3:G{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[$"F3:G{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"G3:G{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

                worksheet.Range[$"H3:H{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                worksheet.Range[$"H3:H{vm.ChkGeralRelatorios.Count + 2}"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                */

                //worksheet.PageSetup.PrintTitleColumns = "$A:$H";
                //worksheet.PageSetup.PrintTitleRows = "$1:$2";
                worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
                worksheet.PageSetup.LeftMargin = 0.0;
                worksheet.PageSetup.RightMargin = 0.0;
                worksheet.PageSetup.TopMargin = 0.0;
                worksheet.PageSetup.BottomMargin = 0.5;
                worksheet.PageSetup.RightFooter = "&P";
                worksheet.PageSetup.LeftFooter = "&D";
                worksheet.PageSetup.CenterVertically = true;
                worksheet.PageSetup.CenterHorizontally = true;

                worksheet.UsedRange.AutofitColumns();


                workbook.SaveAs("TABELA_PA.xlsx");

                Process.Start(new ProcessStartInfo("TABELA_PA.xlsx")
                {
                    UseShellExecute = true
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
}
