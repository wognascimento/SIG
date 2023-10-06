using Microsoft.EntityFrameworkCore;
using Npgsql;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.ScrollAxis;
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

namespace Producao.Views.kit.manutencao
{
    /// <summary>
    /// Interação lógica para ViewDetalhesKitManutencao.xam
    /// </summary>
    public partial class ViewDetalhesKitManutencao : UserControl
    {
        private OsKitSolucaoModel OsKit;
        public ViewDetalhesKitManutencao(OsKitSolucaoModel osKit)
        {
            InitializeComponent();
            this.OsKit = osKit;
            cbSiglaShopping.Text = OsKit.shopping[..^2];
            tbId.Text = "0";
            tbItem.Text = "0";
            tbLocalShopping.Text = "KIT MANUTENÇÃO";
            DataContext = new ViewDetalhesKitManutencaoViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;
                vm.Planilhas = await Task.Run(vm.GetPlanilhasAsync);
                vm.CheckListGerais = await Task.Run(async () => await vm.GetCheckListGeralAsync(OsKit.os));
                vm.Classificacoes = await Task.Run(vm.GetClassificacoesAsync);

                vm.Sigla = await Task.Run(() => vm.GetSiglaAsync(OsKit.shopping[..^2]));

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnPlanilhaSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;

                vm.ComplementoCheckList.codproduto = null;
                vm.Produtos = new ObservableCollection<ProdutoModel>();
                cbDescricao.SelectedItem = null;
                cbDescricao.Text = string.Empty;

                vm.ComplementoCheckList.coduniadicional = null;
                vm.DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
                cbDescricaoAdicional.SelectedItem = null;
                cbDescricaoAdicional.Text = string.Empty;

                vm.Produtos = await Task.Run(async () => await vm.GetProdutosAsync(vm.Planilha?.planilha));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnDescricaoSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;

                vm.ComplementoCheckList.coduniadicional = null;
                vm.DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
                cbDescricaoAdicional.SelectedItem = null;

                vm.DescAdicionais = await Task.Run(async () => await vm.GetDescAdicionaisAsync(vm.Produto?.codigo));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private void OnLimparClick(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private async void OnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.ComplementoCheckList.codcompl = null;
                vm.ComplementoCheckList.sigla = cbSiglaShopping.Text;
                vm.ComplementoCheckList.id_aprovado = vm.Sigla.id_aprovado;
                vm.ComplementoCheckList.kp = this.OsKit.os;
                vm.ComplementoCheckList.ordem = tbId.Text;
                vm.ComplementoCheckList.item_memorial = tbItem.Text;
                vm.ComplementoCheckList.local_shoppings = tbLocalShopping.Text;
                vm.ComplementoCheckList.class_solucao = cmbClassificacoes.SelectedItem.ToString();
                vm.ComplementoCheckList.motivos = cmbMotivos.SelectedItem.ToString();
                vm.ComplementoCheckList.inserido_por = Environment.UserName;
                vm.ComplementoCheckList.inserido_em = DateTime.Now;

                ComplementoCheckListModel compl = await vm.AddComplementoCheckListAsync(vm.ComplementoCheckList);

                vm.CheckListGerais = await Task.Run(() => vm.GetCheckListGeralAsync(this.OsKit.os));

                RowColumnIndex rowColumnIndex = new();
                this.dgCheckListGeral.SelectedItems.Clear();
                this.dgCheckListGeral.SearchHelper.Search(compl.codcompl.ToString());
                this.dgCheckListGeral.SearchHelper.FindNext(compl.codcompl.ToString());
                rowColumnIndex.RowIndex = this.dgCheckListGeral.SearchHelper.CurrentRowColumnIndex.RowIndex;
                dgCheckListGeral.ScrollInView(rowColumnIndex);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao inserir", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                vm.ComplementoCheckList.class_solucao = cmbClassificacoes.SelectedItem.ToString();
                vm.ComplementoCheckList.motivos = cmbMotivos.SelectedItem.ToString();
                vm.ComplementoCheckList.alterado_por = Environment.UserName;
                vm.ComplementoCheckList.alterado_em = DateTime.Now;
                ComplementoCheckListModel compl = await vm.AddComplementoCheckListAsync(vm.ComplementoCheckList);

                vm.CheckListGerais = await Task.Run(() => vm.GetCheckListGeralAsync(this.OsKit.os));
                
                RowColumnIndex rowColumnIndex = new();
                this.dgCheckListGeral.SelectedItems.Clear();
                this.dgCheckListGeral.SearchHelper.Search(compl.codcompl.ToString());
                this.dgCheckListGeral.SearchHelper.FindNext(compl.codcompl.ToString());
                rowColumnIndex.RowIndex = this.dgCheckListGeral.SearchHelper.CurrentRowColumnIndex.RowIndex;
                dgCheckListGeral.ScrollInView(rowColumnIndex);
                
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao inserir", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnPrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;

                //QryRequisicaoDetalheModel requi = (from r in vm.QryRequisicaoDetalhes select r).FirstOrDefault();

                vm.ChkGerais = await Task.Run(() => vm.GetKitCheckListGeralAsync(this.OsKit.os));
                vm.ChkGeral = (from r in vm.ChkGerais select r).FirstOrDefault();

                using ExcelEngine excelEngine = new();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = application.Workbooks.Open("Modelos/REQUISICAO_KIT_MODELO.xlsx", ExcelParseOptions.Default, false, "1@3mudar");

                IStyle borderStyle = workbook.Styles.Add("BorderStyle");
                borderStyle.BeginUpdate();
                //borderStyle.Color = Color.FromArgb(255, 174, 33);
                //borderStyle.Font.Bold = true;
                borderStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                borderStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                borderStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                borderStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                borderStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                borderStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                borderStyle.WrapText = true;
                borderStyle.EndUpdate();




                IWorksheet worksheet = workbook.Worksheets[0];

                worksheet.Range["A1"].Text = "KIT SOLUÇÃO";
                worksheet.Range["C2"].Text = vm.ChkGeral.sigla;
                worksheet.Range["F2"].Number = (double)vm.ChkGeral.t_os_mont;
                worksheet.Range["I2"].Number = (double)vm.ChkGeral.os;
                worksheet.Range["C3"].Number = (double)vm.ChkGeral.distancia; //String.Format("{0:dd/MM/yyyy}", requi.data);  // "03/09/2008" //vm.Requisicao.data.ToString("MM/dd/yyyy");
                worksheet.Range["G3"].Text = vm.ChkGeral.cidade;
                worksheet.Range["L3"].Text = vm.ChkGeral.est;
                worksheet.Range["C4"].Text = vm.ChkGeral.solicitante;
                worksheet.Range["J4"].Text = String.Format("{0:dd/MM/yyyy HH:mm:ss}", vm.ChkGeral.data_solicitacao); //(DateTime)vm.ChkGeral.data_solicitacao;
                worksheet.Range["C6"].Text = vm.ChkGeral.noite_montagem;

                //worksheet.Range["C6"].Text = requi.num_os_servico.ToString();
                //worksheet.Range["F6"].Text = requi.produtocompleto;

                //var itens = (from i in vm.QryRequisicaoDetalhes where i.quantidade > 0 select new { i.quantidade, i.planilha, i.descricao_completa, i.unidade, i.observacao, i.codcompladicional }).ToList(); //new { a.Name, a.Age }
                var index = 9;
                foreach (var item in vm.ChkGerais)
                {
                    worksheet.Range[$"A{index}"].Number = (double)item.qtd;
                    worksheet.Range[$"A{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"A{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    //worksheet.Range[$"A{index}"].CellStyle.Font.Size = 7;

                    worksheet.Range[$"B{index}"].Number = (double)item.codcompladicional;
                    worksheet.Range[$"B{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"B{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    //worksheet.Range[$"B{index}"].CellStyle.Font.Size = 7;

                    worksheet.Range[$"C{index}:D{index}"].Text = item.planilha;
                    worksheet.Range[$"C{index}:D{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"C{index}:D{index}"].CellStyle.Font.Size = 7;
                    worksheet.Range[$"C{index}:D{index}"].Merge();
                    worksheet.Range[$"C{index}:D{index}"].WrapText = true;

                    worksheet.Range[$"E{index}:K{index}"].Text = item.descricao_completa;
                    worksheet.Range[$"E{index}:K{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"E{index}:K{index}"].CellStyle.Font.Size = 7;
                    worksheet.Range[$"E{index}:K{index}"].Merge();
                    worksheet.Range[$"E{index}:K{index}"].WrapText = true;

                    worksheet.Range[$"L{index}"].Text = item.unidade;
                    worksheet.Range[$"L{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"L{index}"].CellStyle.Font.Size = 7;

                    worksheet.Range[$"M{index}:N{index}"].Text = item.obs;
                    worksheet.Range[$"M{index}:N{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"M{index}:N{index}"].CellStyle.Font.Size = 7;
                    worksheet.Range[$"M{index}:N{index}"].Merge();
                    worksheet.Range[$"M{index}:N{index}"].WrapText = true;
                    worksheet.Range[$"E{index}:K{index}"].RowHeight = 26;

                    worksheet.Range[$"O{index}"].Number = (double)item.coddetalhescompl;
                    worksheet.Range[$"O{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"O{index}"].CellStyle.Font.Size = 7;

                    //worksheet.Range[$"P{index}"].Number = (double)item.coddetalhescompl;
                    worksheet.Range[$"P{index}"].CellStyle = borderStyle;
                    //worksheet.Range[$"O{index}"].CellStyle.Font.Size = 7;

                    index++;
                }


                IWorksheet wsheet = workbook.Worksheets[1];
                wsheet.SetColumnWidth(6, 1);
                int etiqueta = 1;
                int col1E1 = 1;
                int col2E1 = 2;
                int col1E2 = 1;
                int col2E2 = 2;
                foreach (var item in vm.ChkGerais)
                {
                    if(etiqueta == 1)
                    {
                        wsheet.Range[$"A{col1E1}:E{col2E1}"].Text = item.nome;
                        wsheet.Range[$"A{col1E1}:E{col2E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:E{col2E1}"].CellStyle = borderStyle;
                        col1E1+=2;
                        col2E1+=2;

                        wsheet.Range[$"A{col1E1}:C{col2E1}"].Text = item.shopping;
                        wsheet.Range[$"A{col1E1}:C{col2E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:C{col2E1}"].CellStyle = borderStyle;

                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Text = "CONTROLE";
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Merge();
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].CellStyle.Font.Bold = true;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Number = (double)item.kp;
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Merge();
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].CellStyle = borderStyle;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"A{col1E1}:D{col1E1}"].Text = item.cidade;
                        wsheet.Range[$"A{col1E1}:D{col1E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:D{col1E1}"].CellStyle = borderStyle;

                        wsheet.Range[$"E{col1E1}"].Text = item.est;
                        wsheet.Range[$"E{col1E1}"].Merge();
                        wsheet.Range[$"E{col1E1}"].CellStyle = borderStyle;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"A{col1E1}:B{col1E1}"].Text = "CODDETCOMPL";
                        wsheet.Range[$"A{col1E1}:B{col1E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:B{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"A{col1E1}:B{col1E1}"].CellStyle.Font.Bold = true;

                        wsheet.Range[$"C{col1E1}:E{col1E1}"].Text = "PLANILHA";
                        wsheet.Range[$"C{col1E1}:E{col1E1}"].Merge();
                        wsheet.Range[$"C{col1E1}:E{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"C{col1E1}:E{col1E1}"].CellStyle.Font.Bold = true;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"A{col1E1}:B{col1E1}"].Number = (double)item.coddetalhescompl;
                        wsheet.Range[$"A{col1E1}:B{col1E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:B{col1E1}"].CellStyle = borderStyle;

                        wsheet.Range[$"C{col1E1}:E{col1E1}"].Text = item.planilha;
                        wsheet.Range[$"C{col1E1}:E{col1E1}"].Merge();
                        wsheet.Range[$"C{col1E1}:E{col1E1}"].CellStyle = borderStyle;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"A{col1E1}:C{col1E1}"].Text = "DESCRIÇÃO";
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].CellStyle.Font.Bold = true;

                        wsheet.Range[$"D{col1E1}"].Text = "QTD";
                        wsheet.Range[$"D{col1E1}"].Merge();
                        wsheet.Range[$"D{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"D{col1E1}"].CellStyle.Font.Bold = true;

                        wsheet.Range[$"E{col1E1}"].Number = (double)item.qtd;
                        wsheet.Range[$"E{col1E1}"].Merge();
                        wsheet.Range[$"E{col1E1}"].CellStyle = borderStyle;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"A{col1E1}:E{col2E1}"].Text = item.descricao_completa;
                        wsheet.Range[$"A{col1E1}:E{col2E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:E{col2E1}"].CellStyle = borderStyle;
                        col1E1+=2;
                        col2E1+=2;

                        wsheet.Range[$"A{col1E1}:C{col1E1}"].Text = "SOLICITANTE";
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].CellStyle.Font.Bold = true;

                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Text = "ATENDENTE";
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Merge();
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].CellStyle = borderStyle;
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].CellStyle.Font.Bold = true;
                        col1E1++;
                        col2E1++;

                        wsheet.Range[$"A{col1E1}:C{col1E1}"].Text = item.solicitante;
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].Merge();
                        wsheet.Range[$"A{col1E1}:C{col1E1}"].CellStyle = borderStyle;

                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Text = item.atendente;
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].Merge();
                        wsheet.Range[$"D{col1E1}:E{col1E1}"].CellStyle = borderStyle;
                        col1E1+=2;
                        col2E1+=2;

                        etiqueta = 2;   
                        continue;       
                                        
                    }                   
                                        
                    if (etiqueta == 2)  
                    {
                        // col1E2
                        // col2E2

                        wsheet.Range[$"G{col1E2}:K{col2E2}"].Text = item.nome;
                        wsheet.Range[$"G{col1E2}:K{col2E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:K{col2E2}"].CellStyle = borderStyle;
                        col1E2+=2;
                        col2E2+=2;

                        wsheet.Range[$"G{col1E2}:I{col2E2}"].Text = item.shopping;
                        wsheet.Range[$"G{col1E2}:I{col2E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:I{col2E2}"].CellStyle = borderStyle;
                                             
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Text = "CONTROLE";
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Merge();
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].CellStyle.Font.Bold = true;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Number = (double)item.kp;
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Merge();
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].CellStyle = borderStyle;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"G{col1E2}:J{col1E2}"].Text = item.cidade;
                        wsheet.Range[$"G{col1E2}:J{col1E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:J{col1E2}"].CellStyle = borderStyle;
                                             
                        wsheet.Range[$"K{col1E2}"].Text = item.est;
                        wsheet.Range[$"K{col1E2}"].Merge();
                        wsheet.Range[$"K{col1E2}"].CellStyle = borderStyle;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"G{col1E2}:H{col1E2}"].Text = "CODDETCOMPL";
                        wsheet.Range[$"G{col1E2}:H{col1E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:H{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"G{col1E2}:H{col1E2}"].CellStyle.Font.Bold = true;
                                             
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].Text = "PLANILHA";
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].Merge();
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].CellStyle.Font.Bold = true;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"G{col1E2}:H{col1E2}"].Number = (double)item.coddetalhescompl;
                        wsheet.Range[$"G{col1E2}:H{col1E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:H{col1E2}"].CellStyle = borderStyle;
                                             
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].Text = item.planilha;
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].Merge();
                        wsheet.Range[$"I{col1E2}:K{col1E2}"].CellStyle = borderStyle;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"G{col1E2}:I{col1E2}"].Text = "DESCRIÇÃO";
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].CellStyle.Font.Bold = true;
                                        
                        wsheet.Range[$"J{col1E2}"].Text = "QTD";
                        wsheet.Range[$"J{col1E2}"].Merge();
                        wsheet.Range[$"J{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"J{col1E2}"].CellStyle.Font.Bold = true;
                                        
                        wsheet.Range[$"K{col1E2}"].Number = (double)item.qtd;
                        wsheet.Range[$"K{col1E2}"].Merge();
                        wsheet.Range[$"K{col1E2}"].CellStyle = borderStyle;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"G{col1E2}:K{col2E2}"].Text = item.descricao_completa;
                        wsheet.Range[$"G{col1E2}:K{col2E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:K{col2E2}"].CellStyle = borderStyle;
                        col1E2+=2;
                        col2E2+=2;

                        wsheet.Range[$"G{col1E2}:I{col1E2}"].Text = "SOLICITANTE";
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].CellStyle.Font.Bold = true;
                                              
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Text = "ATENDENTE";
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Merge();
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].CellStyle = borderStyle;
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].CellStyle.Font.Bold = true;
                        col1E2++;
                        col2E2++;

                        wsheet.Range[$"G{col1E2}:I{col1E2}"].Text = item.solicitante;
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].Merge();
                        wsheet.Range[$"G{col1E2}:I{col1E2}"].CellStyle = borderStyle;
                                              
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Text = item.atendente;
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].Merge();
                        wsheet.Range[$"J{col1E2}:K{col1E2}"].CellStyle = borderStyle;
                        col1E2 += 2;
                        col2E2 += 2;

                        etiqueta = 1;
                        continue;
                    }
                }

                wsheet.PageSetup.LeftMargin = 2;
                wsheet.PageSetup.RightMargin = 2;

                //Save the Excel document
                workbook.SaveAs($"Impressos/REQUISICAO_KIT_{vm.ChkGeral.os}.xlsx");
                Process.Start(new ProcessStartInfo($"Impressos\\REQUISICAO_KIT_{vm.ChkGeral.os}.xlsx")
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

        private void dgCheckListGeral_RowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {

        }

        private async void OnSelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {

            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;
                var record = vm.CheckListGeral;

                vm.ComplementoCheckList = new ComplementoCheckListModel
                {
                    ordem = vm?.CheckListGeral?.id,
                    sigla = vm?.CheckListGeral?.sigla,
                    local_shoppings = vm?.CheckListGeral?.local_shoppings,
                    codproduto = vm?.CheckListGeral?.codigo,
                    obs = vm?.CheckListGeral?.obs,
                    dataalteracaodesc = vm?.CheckListGeral?.dataalteracaodesc,
                    alteradopor = vm?.CheckListGeral?.alteradopor,
                    orient_montagem = vm?.CheckListGeral?.orient_montagem,
                    item_memorial = vm?.CheckListGeral?.item_memorial,
                    incluidopordesc = vm?.CheckListGeral?.incluidopordesc,
                    kp = vm?.CheckListGeral?.kp,
                    orient_desmont = vm?.CheckListGeral?.orient_desmont,
                    qtd = vm?.CheckListGeral?.qtd,
                    coduniadicional = vm?.CheckListGeral?.coduniadicional,
                    codcompl = vm?.CheckListGeral?.codcompl,
                    nivel = vm?.CheckListGeral?.nivel,
                    carga = vm?.CheckListGeral?.carga,
                    class_solucao = vm?.CheckListGeral?.class_solucao,
                    motivos = vm?.CheckListGeral?.motivos,
                    id_aprovado = vm?.CheckListGeral?.id_aprovado,
                    historico = vm?.CheckListGeral?.historico,
                    agrupar = vm?.CheckListGeral?.agrupar,
                    inserido_por = vm?.CheckListGeral?.inserido_por,
                    inserido_em = vm?.CheckListGeral?.inserido_em,
                };

                vm.Planilha = (from p in vm.Planilhas where p.planilha == record?.planilha select p).FirstOrDefault();
                vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(vm?.Planilha?.planilha));
                vm.Produto = (from p in vm.Produtos where p.codigo == record?.codigo select p).FirstOrDefault();
                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(vm?.Produto?.codigo));
                vm.DescAdicional = (from d in vm.DescAdicionais where d.coduniadicional == record?.coduniadicional select d).FirstOrDefault();

                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(vm?.CheckListGeral?.coduniadicional));
                vm.CheckListGeralComplementos = await Task.Run(() => vm.GetCheckListGeralComplementoAsync(vm?.CheckListGeral?.codcompl));

                cmbClassificacoes.SelectedItem = record?.class_solucao;
                cmbMotivos.SelectedItem = record?.motivos;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private void dgComplemento_AddNewRowInitiating(object sender, Syncfusion.UI.Xaml.Grid.AddNewRowInitiatingEventArgs e)
        {
            ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;

            ((QryCheckListGeralComplementoModel)e.NewObject).codcompl = vm.CheckListGeral.codcompl;
        }

        private void OnCurrentCellDropDownSelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellDropDownSelectionChangedEventArgs e)
        {
            var sfdatagrid = sender as SfDataGrid;
            var viewModel = (ViewDetalhesKitManutencaoViewModel)sfdatagrid.DataContext;
            int rowIndex = sfdatagrid.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);

            QryCheckListGeralComplementoModel record;

            if (rowIndex == -1)
                record = (QryCheckListGeralComplementoModel)sfdatagrid.View.CurrentAddItem;
            //record = new();
            else
                record = (QryCheckListGeralComplementoModel)(sfdatagrid.View.Records[rowIndex] as RecordEntry).Data;

            record.unidade = ((TblComplementoAdicionalModel)e.SelectedItem).unidade; //viewModel.UnitPriceDict[e.SelectedItem.ToString()];
            record.saldoestoque = ((TblComplementoAdicionalModel)e.SelectedItem).saldo_estoque; //viewModel.QuantityDict[e.SelectedItem.ToString()];
        }

        private void dgComplemento_CurrentCellValueChanged(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellValueChangedEventArgs e)
        {

        }

        private async void dgComplemento_RowValidated(object sender, Syncfusion.UI.Xaml.Grid.RowValidatedEventArgs e)
        {
            var sfdatagrid = sender as SfDataGrid;
            ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                QryCheckListGeralComplementoModel data = (QryCheckListGeralComplementoModel)e.RowData;
                vm.DetCompl.coddetalhescompl = data.coddetalhescompl;
                vm.DetCompl.codcompl = data.codcompl;
                vm.DetCompl.codcompladicional = data.codcompladicional;
                vm.DetCompl.qtd = data.qtd;
                vm.DetCompl.confirmado = data.confirmado;
                vm.DetCompl.confirmado_data = data.confirmado == "-1" ? DateTime.Now : null;
                vm.DetCompl.confirmado_por = data.confirmado == "-1" ? Environment.UserName : null;
                vm.DetCompl.desabilitado_confirmado_data = data.confirmado == "-1" ? DateTime.Now : null;
                vm.DetCompl.desabilitado_confirmado_por = data.confirmado == "-1" ? Environment.UserName : null;
                vm.DetCompl.local_producao = "JACAREÍ";
                vm.DetCompl.os = data.os;

                vm.DetCompl = await Task.Run(() => vm.AddDetalhesComplementoCheckListAsync(vm.DetCompl));
                //QryCheckListGeralComplementoModel record = (QryCheckListGeralComplementoModel)sfdatagrid.View.CurrentAddItem;
                //record.coddetalhescompl = vm.DetCompl.coddetalhescompl;
                ((QryCheckListGeralComplementoModel)e.RowData).coddetalhescompl = vm.DetCompl.coddetalhescompl;
                sfdatagrid.View.Refresh();
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                var toRemove = vm.CheckListGeralComplementos.Where(x => x.coddetalhescompl == null).ToList();
                foreach (var item in toRemove)
                    vm.CheckListGeralComplementos.Remove(item);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private void dgComplemento_RowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {

        }

        private async void ComboBoxAdv_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string classificacao = e.AddedItems[0].ToString();
                //sender = {Syncfusion.Windows.Tools.Controls.ComboBoxAdv Items.Count:6}
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;
                vm.Motivos = await Task.Run(async () => await vm.GetMotivosAsync(classificacao));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        void Limpar()
        {
            ViewDetalhesKitManutencaoViewModel vm = (ViewDetalhesKitManutencaoViewModel)DataContext;

            //tbId.Text = string.Empty;
            //tbItem.Text = string.Empty;
            //tbLocalShopping.Text = string.Empty;
            cbPlanilha.SelectedItem = null;
            cbPlanilha.Text = string.Empty;
            cbDescricao.SelectedItem = null;
            cbDescricao.Text = string.Empty;
            cbDescricaoAdicional.SelectedItem = null;
            cbDescricaoAdicional.Text = string.Empty;
            tbQtde.Value = null;
            tbOrientacaoProducao.Text = string.Empty;
            tbOrientacaoMontagem.Text = string.Empty;
            tbOrientacaoDesmontagem.Text = string.Empty;

            dgCheckListGeral.SelectedItem = null;
            dgComplemento.SelectedItem = null;

            cmbMotivos.SelectedItem = null; 
            cmbClassificacoes.SelectedItem = null;
            //btnAddicionar.Visibility = Visibility.Collapsed;

            vm.ComplementoCheckList = new ComplementoCheckListModel();

            tbId.Focus();
        }
    }

    public class ViewDetalhesKitManutencaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private SiglaChkListModel _sigla;
        public SiglaChkListModel Sigla
        {
            get { return _sigla; }
            set { _sigla = value; RaisePropertyChanged("Sigla"); }
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

        private ObservableCollection<ComplementoCheckListModel> _complementoCheckLists;
        public ObservableCollection<ComplementoCheckListModel> ComplementoCheckLists
        {
            get { return _complementoCheckLists; }
            set { _complementoCheckLists = value; RaisePropertyChanged("ComplementoCheckLists"); }
        }

        private ComplementoCheckListModel complementoCheckList;
        public ComplementoCheckListModel ComplementoCheckList
        {
            get { return complementoCheckList; }
            set { complementoCheckList = value; RaisePropertyChanged("ComplementoCheckList"); }
        }

        private QryCheckListGeralModel _checkListGeral;
        public QryCheckListGeralModel CheckListGeral
        {
            get { return _checkListGeral; }
            set { _checkListGeral = value; RaisePropertyChanged("CheckListGeral"); }
        }
        private ObservableCollection<QryCheckListGeralModel> _checkListGerais;
        public ObservableCollection<QryCheckListGeralModel> CheckListGerais
        {
            get { return _checkListGerais; }
            set { _checkListGerais = value; RaisePropertyChanged("CheckListGerais"); }
        }

        private ObservableCollection<string> _classificacoes;
        public ObservableCollection<string> Classificacoes
        {
            get { return _classificacoes; }
            set { _classificacoes = value; RaisePropertyChanged("Classificacoes"); }
        }

        private ObservableCollection<string> _motivos;
        public ObservableCollection<string> Motivos
        {
            get { return _motivos; }
            set { _motivos = value; RaisePropertyChanged("Motivos"); }
        }

        private QryCheckListGeralComplementoModel _checkListGeralComplemento;
        public QryCheckListGeralComplementoModel CheckListGeralComplemento
        {
            get { return _checkListGeralComplemento; }
            set { _checkListGeralComplemento = value; RaisePropertyChanged("CheckListGeralComplemento"); }
        }
        private ObservableCollection<QryCheckListGeralComplementoModel> _checkListGeralComplementos;
        public ObservableCollection<QryCheckListGeralComplementoModel> CheckListGeralComplementos
        {
            get { return _checkListGeralComplementos; }
            set { _checkListGeralComplementos = value; RaisePropertyChanged("CheckListGeralComplementos"); }
        }

        private DetalhesComplemento _detCompl;
        public DetalhesComplemento DetCompl
        {
            get { return _detCompl; }
            set { _detCompl = value; RaisePropertyChanged("DetCompl"); }
        }
        private ObservableCollection<DetalhesComplemento> _detCompls;
        public ObservableCollection<DetalhesComplemento> DetCompls
        {
            get { return _detCompls; }
            set { _detCompls = value; RaisePropertyChanged("DetCompls"); }
        }
        
        private KitChkGeralModel _chkGeral;
        public KitChkGeralModel ChkGeral
        {
            get { return _chkGeral; }
            set { _chkGeral = value; RaisePropertyChanged("ChkGeral"); }
        }
        private ObservableCollection<KitChkGeralModel> _chkGerais;
        public ObservableCollection<KitChkGeralModel> ChkGerais
        {
            get { return _chkGerais; }
            set { _chkGerais = value; RaisePropertyChanged("ChkGerais"); }
        }

        public ViewDetalhesKitManutencaoViewModel()
        {
            DetCompl = new DetalhesComplemento();
            ComplementoCheckList = new ComplementoCheckListModel();
            Planilhas = new ObservableCollection<RelplanModel>();
        }

        public async Task<SiglaChkListModel> GetSiglaAsync(string sigla)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Siglas.Where(c => c.sigla_serv == sigla).FirstOrDefaultAsync();
                return data;
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
                var data = await db.Relplans.OrderBy(c => c.planilha).Where(c => c.ativo.Equals("1")).ToListAsync();
                return new ObservableCollection<RelplanModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<QryCheckListGeralModel>> GetCheckListGeralAsync(long? kp)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.CheckListGerals
                    .OrderBy(c => c.id)
                    .Where(c => c.kp == kp)
                    .ToListAsync();
                return new ObservableCollection<QryCheckListGeralModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<string>> GetClassificacoesAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = db.ClassificacaoSolucaos.GroupBy(m => m.classificacao).OrderBy(c => c.Key).Select(g => g.Key).ToList();
                return new ObservableCollection<string>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<string>> GetMotivosAsync(string classificacao)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ClassificacaoSolucaos.OrderBy(c => c.motivo).Where(c => c.classificacao == classificacao).Select(p => p.motivo).ToListAsync();
                return new ObservableCollection<string>(data);
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

        public async Task<ComplementoCheckListModel> AddComplementoCheckListAsync(ComplementoCheckListModel ComplementoCheckList)
        {
            try
            {
                using DatabaseContext db = new();
                //db.Entry(ComplementoCheckList).State = ComplementoCheckList.codcompl == null ? EntityState.Added : EntityState.Modified;
                await db.ComplementoCheckLists.SingleMergeAsync(ComplementoCheckList);
                await db.SaveChangesAsync();

                return ComplementoCheckList;
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<TblComplementoAdicionalModel>> GetCompleAdicionaisAsync(long? coduniadicional)
        {
            try
            {
                CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>();
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

        public async Task<ObservableCollection<QryCheckListGeralComplementoModel>> GetCheckListGeralComplementoAsync(long? codcompl)
        {
            try
            {
                //CheckListGeralComplementos = new ObservableCollection<QryCheckListGeralComplementoModel>();
                using DatabaseContext db = new();
                var data = await db.CheckListGeralComplementos
                    .OrderBy(c => c.coddetalhescompl)
                    .Where(c => c.codcompl == codcompl)
                    .ToListAsync();

                return new ObservableCollection<QryCheckListGeralComplementoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DetalhesComplemento> AddDetalhesComplementoCheckListAsync(DetalhesComplemento detCompl)
        {
            try
            {
                using DatabaseContext db = new();
                //db.Entry(detCompl).State = detCompl.coddetalhescompl == null ? EntityState.Added : EntityState.Modified;
                await db.DetalhesComplementos.SingleMergeAsync(detCompl);
                await db.SaveChangesAsync();

                return detCompl;
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<KitChkGeralModel>> GetKitCheckListGeralAsync(long? os)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.KitChkGerals
                    .OrderBy(c => c.planilha)
                    .ThenBy(c => c.descricao_completa)
                    .Where(c => c.os == os)
                    .ToListAsync();

                return new ObservableCollection<KitChkGeralModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
