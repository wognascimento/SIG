﻿using Producao.Views.popup;
using Syncfusion.Data;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.ScrollAxis;
using Syncfusion.XlsIO;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Producao.Views
{
    /// <summary>
    /// Interação lógica para ViewCheckList.xam
    /// </summary>
    public partial class ViewCheckList : UserControl
    {
        private bool dbClick;
        public ViewCheckList()
        {
            DataContext = new CheckListViewModel();
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            /*
            ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
            //var itens = await new CheckListViewModel().GetSiglasAsync();

            var itens =  await Task.Run(async () => await new CheckListViewModel().GetSiglasAsync());

            ((CheckListViewModel)DataContext).Siglas = itens;
            ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            */
        }

        private void OnSelectionClient(object sender, Syncfusion.UI.Xaml.Grid.SelectionChangedEventArgs e)
        {

        }

        private void OnSelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            CheckListViewModel vm = (CheckListViewModel)DataContext;
            vm.CheckListGeralComplemento = new QryCheckListGeralComplementoModel();
            vm.CheckListGeralComplementos = new ObservableCollection<QryCheckListGeralComplementoModel>();
            btnAddicionar.Visibility = Visibility.Collapsed;
        }

        private void OnLimparClick(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        void Limpar()
        {
            tbId.Text = string.Empty;
            tbItem.Text = string.Empty;
            tbLocalShopping.Text = string.Empty;
            cbPlanilha.SelectedItem = null;
            cbDescricao.SelectedItem = null;
            cbDescricaoAdicional.SelectedItem = null;
            tbQtde.Value = null;
            tbOrientacaoProducao.Text = string.Empty;
            tbOrientacaoMontagem.Text = string.Empty;
            tbOrientacaoDesmontagem.Text = string.Empty;
            tbId.Focus();
        }

        private async void OnPlanilhaSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckListViewModel vm = (CheckListViewModel)DataContext;
            if(!dbClick)
                await Task.Run(async () => await vm.GetProdutosAsync());

        }

        private async void OnDescricaoSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckListViewModel vm = (CheckListViewModel)DataContext;
            if (!dbClick)
                await Task.Run(async () => await vm.GetDescAdicionaisAsync());
            
        }

        private async void OnSiglaSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckListViewModel vm = (CheckListViewModel)DataContext;

            ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
            SiglaChkListModel valor = (SiglaChkListModel)this.cbSiglaShopping.SelectedItem;
            var locais = await Task.Run(async () => await vm.GetLocaisShoppAsync());

            var itens = await Task.Run(async () => await vm.GetCheckListGeralAsync());
            ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Collapsed;
        }

        private async void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dbClick = true;
            var visualcontainer = this.dgCheckListGeral.GetVisualContainer();
            var rowColumnIndex = visualcontainer.PointToCellRowColumnIndex(e.GetPosition(visualcontainer));
            var recordindex = this.dgCheckListGeral.ResolveToRecordIndex(rowColumnIndex.RowIndex);
            var recordentry = this.dgCheckListGeral.View.GroupDescriptions.Count == 0 ? this.dgCheckListGeral.View.Records[recordindex] : this.dgCheckListGeral.View.TopLevelGroup.DisplayElements[recordindex];
            var record = ((RecordEntry)recordentry).Data as QryCheckListGeralModel;

            CheckListViewModel vm = (CheckListViewModel)DataContext;
            
            vm.ComplementoCheckList = new ComplementoCheckListModel
            {
                ordem = vm.CheckListGeral.id,
                sigla = vm.CheckListGeral.sigla,
                local_shoppings = vm.CheckListGeral.local_shoppings,
                codproduto = vm.CheckListGeral.codigo,
                obs = vm.CheckListGeral.obs,
                dataalteracaodesc = vm.CheckListGeral.dataalteracaodesc,
                alteradopor = vm.CheckListGeral.alteradopor,
                orient_montagem = vm.CheckListGeral.orient_montagem,
                item_memorial = vm.CheckListGeral.item_memorial,
                incluidopordesc = vm.CheckListGeral.incluidopordesc,
                kp = vm.CheckListGeral.kp,
                orient_desmont = vm.CheckListGeral.orient_desmont,
                qtd = vm.CheckListGeral.qtd,
                coduniadicional = vm.CheckListGeral.coduniadicional,
                codcompl = vm.CheckListGeral.codcompl,
                nivel = vm.CheckListGeral.nivel,
                carga = vm.CheckListGeral.carga,
                class_solucao = vm.CheckListGeral.class_solucao,
                id_aprovado = vm.CheckListGeral.id_aprovado,
                historico = vm.CheckListGeral.historico,
                agrupar = vm.CheckListGeral.agrupar
            };
            
            vm.Planilha = (from p in vm.Planilhas where p.planilha == record.planilha select p).FirstOrDefault();
            await Task.Run(async () => await vm.GetProdutosAsync());
            vm.Produto = (from p in vm.Produtos where p.codigo == record.codigo select p).FirstOrDefault();
            await Task.Run(async () => await vm.GetDescAdicionaisAsync());
            vm.DescAdicional = (from d in vm.DescAdicionais where d.coduniadicional == record.coduniadicional select d).FirstOrDefault();

            await Task.Run(async () => await vm.GetCheckListGeralComplementoAsync());
            await Task.Run(async () => await vm.GetCompleAdicionaisAsync());
            btnAddicionar.Visibility = Visibility.Visible;

        }

        private async void OnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CheckListViewModel vm = (CheckListViewModel)DataContext;

                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.ComplementoCheckList.codcompl = null;
                vm.ComplementoCheckList.sigla = vm.Sigla.sigla_serv;
                ComplementoCheckListModel compl = await vm.AddComplementoCheckListAsync();

                SiglaChkListModel valor = (SiglaChkListModel)this.cbSiglaShopping.SelectedItem;
                var locais = await Task.Run(async () => await vm.GetLocaisShoppAsync());
                vm.Locaisshopping = locais;

                await Task.Run(async () => await vm.GetCheckListGeralAsync());

                RowColumnIndex rowColumnIndex = new RowColumnIndex();
                this.dgCheckListGeral.SelectedItems.Clear();
                this.dgCheckListGeral.SearchHelper.Search(compl.ordem);
                this.dgCheckListGeral.SearchHelper.FindNext(compl.ordem);
                rowColumnIndex.RowIndex = this.dgCheckListGeral.SearchHelper.CurrentRowColumnIndex.RowIndex;
                dgCheckListGeral.ScrollInView(rowColumnIndex);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Erro ao inserir", MessageBoxButton.OK, MessageBoxImage.Error);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Collapsed;
            }
            
        }

        private async void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckListViewModel vm = (CheckListViewModel)DataContext;
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                ComplementoCheckListModel compl = await vm.AddComplementoCheckListAsync();

                SiglaChkListModel valor = (SiglaChkListModel)this.cbSiglaShopping.SelectedItem;
                var locais = await Task.Run(async () => await vm.GetLocaisShoppAsync());
                vm.Locaisshopping = locais;

                var itens = await Task.Run(async () => await vm.GetCheckListGeralAsync());

                RowColumnIndex rowColumnIndex = new RowColumnIndex();
                this.dgCheckListGeral.SelectedItems.Clear();
                this.dgCheckListGeral.SearchHelper.Search(compl.ordem);
                this.dgCheckListGeral.SearchHelper.FindNext(compl.ordem);
                rowColumnIndex.RowIndex = this.dgCheckListGeral.SearchHelper.CurrentRowColumnIndex.RowIndex;
                dgCheckListGeral.ScrollInView(rowColumnIndex);


                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Erro ao inserir", MessageBoxButton.OK, MessageBoxImage.Error);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Collapsed;
            }
        }

        private void OnCurrentCellDropDownSelectionChanged(object sender, CurrentCellDropDownSelectionChangedEventArgs e)
        {

            QryCheckListGeralComplementoModel record;
            var sfdatagrid = sender as SfDataGrid;
            var viewModel = sfdatagrid.DataContext as CheckListViewModel;
            int rowIndex = sfdatagrid.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);

            //Console.Out.WriteLine("ADD NEW " + sfdatagrid.IsAddNewIndex(e.RowColumnIndex.RowIndex));

            Debug.WriteLine("something" + sfdatagrid.IsAddNewIndex(e.RowColumnIndex.RowIndex));

            if (rowIndex == -1)
                record = sfdatagrid.View.CurrentAddItem as QryCheckListGeralComplementoModel;
            //record = new();
            else
                record = (sfdatagrid.View.Records[rowIndex] as RecordEntry).Data as QryCheckListGeralComplementoModel;

            sfdatagrid.View.BeginInit();
            record.unidade = ((TblComplementoAdicionalModel)e.SelectedItem).unidade;
            record.saldoestoque = ((TblComplementoAdicionalModel)e.SelectedItem).saldo_estoque;
            sfdatagrid.View.EndInit();
            sfdatagrid.View.Refresh();

            //RowColumnIndex rowColumnIndex = new RowColumnIndex(3, 2);
            //this.dataGrid.MoveCurrentCell(rowColumnIndex);
            //this.dataGrid.SelectionController.CurrentCellManager.BeginEdit();

            //sfdatagrid.SelectionController.CurrentCellManager.BeginEdit();

            //this.dgComplemento.UpdateDataRow(e.RowColumnIndex.RowIndex);

            //record.UnitPrice = viewModel.UnitPriceDict[e.SelectedItem.ToString()];

            //record.Quantity = viewModel.QuantityDict[e.SelectedItem.ToString()];
        }

        private void OnAddComplemento(object sender, RoutedEventArgs e)
        {

            CheckListViewModel vm = (CheckListViewModel)DataContext;

            if(vm.CheckListGeral == null)
            {
                MessageBox.Show("Seleciona uma linha para poder completas", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            vm.DetCompl = new DetalhesComplemento();
            AddDetalhesComplemento detailsWindow = new AddDetalhesComplemento(vm);
            detailsWindow.ShowDialog();
        }

        private async void OnPrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckListViewModel vm = (CheckListViewModel)DataContext;
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
                bodyStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                bodyStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                bodyStyle.Font.Bold = true;
                bodyStyle.WrapText = true;
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
                headerStyle.WrapText = true;
                headerStyle.EndUpdate();

                worksheet.Range["A1"].Text = $"{vm.Sigla.sigla_serv} CHECK LIST {BaseSettings.Database}";
                worksheet.Range["A1"].CellStyle.Font.Bold = true;
                worksheet.Range["A1"].CellStyle.Font.Size = 25;

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

                worksheet.PageSetup.PrintTitleColumns = "$A:$H";
                worksheet.PageSetup.PrintTitleRows = "$1:$2";
                worksheet.PageSetup.Orientation = ExcelPageOrientation.Landscape;
                worksheet.PageSetup.LeftMargin = 0.0;
                worksheet.PageSetup.RightMargin = 0.0;
                worksheet.PageSetup.TopMargin = 0.0;
                worksheet.PageSetup.BottomMargin = 0.5;
                worksheet.PageSetup.RightFooter = "&P";
                worksheet.PageSetup.LeftFooter = "&D";
                worksheet.PageSetup.CenterVertically = true;
                worksheet.PageSetup.CenterHorizontally = true;

                workbook.SaveAs("CHECKLIST.xlsx");

                Process.Start(new ProcessStartInfo("CHECKLIST.xlsx")
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

        }

        private void OnDropDownOpened(object sender, EventArgs e)
        {
            dbClick = false;
        }

        private async void dgComplemento_CurrentCellValueChanged(object sender, CurrentCellValueChangedEventArgs e)
        {
            DetalhesComplemento? dado = e.Record as DetalhesComplemento;
            CheckListViewModel vm = (CheckListViewModel)DataContext;
            try
            {
                vm.DetCompl.qtd = dado.qtd;
                vm.DetCompl.confirmado = dado.confirmado;
                vm.DetCompl.confirmado_data = vm.DetCompl.confirmado == "-1" ? DateTime.Now : null;
                vm.DetCompl.confirmado_por = vm.DetCompl.confirmado == "-1" ? Environment.UserName : null;
                vm.DetCompl.desabilitado_confirmado_data = vm.DetCompl.confirmado == "-1" ? DateTime.Now : null;
                vm.DetCompl.desabilitado_confirmado_por = vm.DetCompl.confirmado == "-1" ? Environment.UserName : null;

                await Task.Run(async () => await vm.AddDetalhesComplementoCheckListAsync());
                await Task.Run(async () => await vm.GetCheckListGeralComplementoAsync());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class NameButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "CRIAR";
            else
                return "ABRIR";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
