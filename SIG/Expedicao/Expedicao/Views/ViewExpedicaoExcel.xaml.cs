using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Expedicao.Views
{
    /// <summary>
    /// Interação lógica para ViewExpedicaoExcel.xam
    /// </summary>
    public partial class ViewExpedicaoExcel : UserControl
    {
        public string Consulta { get; set; }

        public ViewExpedicaoExcel(string consulta)
        {
            InitializeComponent();
            this.Consulta = consulta;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.sfmcAprovados.ItemsSource = await Task.Run(() => new AprovadoViewModel().GetAprovados());
                this.sfBusyIndicator.IsBusy = false;
                this.principal.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private async void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication excel = excelEngine.Excel;
            excel.DefaultVersion = ExcelVersion.Xlsx;
            IWorkbook workbook = excel.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            await Task.Run(() => Dispatcher.Invoke(() => btnExcel.Visibility = Visibility.Hidden));
            await Task.Run(() => Dispatcher.Invoke(() => sfBusyIndicatorExcel.IsBusy = true));


            AprovadoModel aprovado = sfmcAprovados.SelectedItem as AprovadoModel;
            object dados = new object();

            if (Consulta == "ITENS_FALTANTES")
            {
                dados = await new ExpedicaoViewModel().GetItensFaltanteAsync(aprovado.SiglaServ);
            }
            else if (Consulta == "ITENS_CARREGADOS")
            {
                dados = await new ExpedicaoViewModel().GetCarregamentoItensAsync(aprovado.SiglaServ);
            }

            ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions()
            {
                FirstRow = 1,
                FirstColumn = 1,
                IncludeHeader = true,
                PreserveTypes = true
            };
            worksheet.ImportData((IEnumerable)dados, importDataOptions);
            worksheet.UsedRange.AutofitColumns();
            workbook.SaveAs($"{Consulta}.xlsx");
            workbook.Close();
            excelEngine.Dispose();

            await Task.Run(() => Dispatcher.Invoke(() => btnExcel.Visibility = Visibility.Visible));
            await Task.Run(() => Dispatcher.Invoke(() => sfBusyIndicatorExcel.IsBusy = false));

            Process.Start(new ProcessStartInfo($"{Consulta}.xlsx")
            {
                UseShellExecute = true
            });
        }

    }
}
