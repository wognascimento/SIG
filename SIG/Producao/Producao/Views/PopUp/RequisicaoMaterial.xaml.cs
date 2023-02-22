using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.Windows.Controls.Input;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace Producao.Views.PopUp
{
    /// <summary>
    /// Lógica interna para RequisicaoMaterial.xaml
    /// </summary>
    public partial class RequisicaoMaterial : Window
    {
        List<string> lVoltagem = new List<string>{"", "220V", "110V" };
        List<string> lLocalShopping = new List<string>{ "", "INTERNO", "EXTERNO" };
        bool dbClick;

        public RequisicaoMaterial(object obj)
        {
            InitializeComponent();
            DataContext = new RequisicaoViewModel();

            RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
            vm.ProdutoServico = (ProdutoServicoModel)obj;
            
            //cbVoltagem.ItemsSource= lVoltagem;
            //cbLocalShopping.ItemsSource= lLocalShopping;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                await Task.Run(async () => await vm.GetRequisicaoAsync());
                await Task.Run(async () => await vm.GetRequisicaoDetalhesAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Limpar()
        {
            tbCodproduto.Text = string.Empty;
            tbPlanilha.SelectedItem = null;
            tbDescricao.SelectedItem = null;
            tbDescricaoAdicional.SelectedItem = null;
            tbComplementoAdicional.SelectedItem = null;
            tbQuantidade.Value = null;
            tbPlanilha.Focus();
        }

        private void OnLimparClick(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private async void OnAdicionarClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                vm.RequisicaoDetalhe = new DetalheRequisicaoModel
                {
                    cod_det_req = null,
                    num_requisicao = vm.Requisicao.num_requisicao,
                    codcompladicional = vm.Compledicional.codcompladicional,
                    quantidade = (float?)tbQuantidade.Value,
                    data = DateTime.Now,
                    alterado_por = Environment.UserName
                };

                await Task.Run(async () => await vm.AddProdutoRequisicaoAsync());
                await Task.Run(async () => await vm.GetRequisicaoDetalhesAsync());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private async void OnEditClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                vm.RequisicaoDetalhe = new DetalheRequisicaoModel
                {
                    cod_det_req = vm.RequisicaoDetalhe.cod_det_req,
                    num_requisicao = vm.Requisicao.num_requisicao,
                    codcompladicional = vm.Compledicional.codcompladicional,
                    quantidade = (float?)tbQuantidade.Value,
                    data = DateTime.Now,
                    alterado_por = Environment.UserName
                };

                await Task.Run(async () => await vm.AddProdutoRequisicaoAsync());
                await Task.Run(async () => await vm.GetRequisicaoDetalhesAsync());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnPrintClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;

                QryRequisicaoDetalheModel requi = (from r in vm.QryRequisicaoDetalhes select r).FirstOrDefault();

                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = application.Workbooks.Open("Modelos/REQUISICAO_MODELO.xlsx");
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Range["C2"].Text = requi.num_requisicao.ToString();
                worksheet.Range["C3"].Text = requi.alterado_por;
                worksheet.Range["G3"].Text = requi.setor_caminho;
                worksheet.Range["C4"].Text = String.Format("{0:dd/MM/yyyy}", requi.data);  // "03/09/2008" //vm.Requisicao.data.ToString("MM/dd/yyyy");
                worksheet.Range["G4"].Text = requi.cliente;
                worksheet.Range["M4"].Text = requi.coddetalhescompl.ToString();
                worksheet.Range["C5"].Text = requi.item_memorial;
                worksheet.Range["G5"].Text = requi.tema;
                worksheet.Range["C6"].Text = requi.num_os_servico.ToString();
                worksheet.Range["F6"].Text = requi.produtocompleto;

                var itens = (from i in vm.QryRequisicaoDetalhes where i.quantidade > 0  select new { i.quantidade, i.planilha, i.descricao_completa, i.unidade }).ToList(); //new { a.Name, a.Age }
                var index = 9;
                foreach (var item in itens) 
                {
                    worksheet.Range[$"A{index}"].Text = item.quantidade.ToString();
                    worksheet.Range[$"A{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    worksheet.Range[$"B{index}"].Text = item.planilha;
                    worksheet.Range[$"E{index}"].Text = item.descricao_completa;
                    worksheet.Range[$"M{index}"].Text = item.unidade;
                    worksheet.Range[$"M{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    index++;
                    //worksheet.Range["D1:E1"].Merge();
                }

                //ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();
                //importDataOptions.FirstRow = 9;
                //importDataOptions.IncludeHeader = false;
                //importDataOptions.NestedDataLayoutOptions = ExcelNestedDataLayoutOptions.Merge;
                //importDataOptions.NestedDataLayoutOptions = ExcelNestedDataLayoutOptions.Repeat;

                //Import data from the nested collection.
                //worksheet.ImportData(itens, importDataOptions);
                //worksheet.ImportData(itens, 9, 1, false);

                //Save the Excel document
                workbook.SaveAs($"Impressos/REQUISICAO_{requi.num_requisicao}.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private async void OnPlanilhaSelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                if (!dbClick)
                    await Task.Run(async () => await vm.GetProdutosAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnDescricaoSelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                if (!dbClick)
                    await Task.Run(async () => await vm.GetDescAdicionaisAsync());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnDescricaoAdicionalSelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                if (!dbClick)
                    await Task.Run(async () => await vm.GetCompleAdicionaisAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnComplementoAdicionalSelectedItemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private async void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dbClick = true;
            var visualcontainer = this.itens.GetVisualContainer();
            var rowColumnIndex = visualcontainer.PointToCellRowColumnIndex(e.GetPosition(visualcontainer));
            var recordindex = this.itens.ResolveToRecordIndex(rowColumnIndex.RowIndex);
            var recordentry = this.itens.View.GroupDescriptions.Count == 0 ? this.itens.View.Records[recordindex] : this.itens.View.TopLevelGroup.DisplayElements[recordindex];
            var record = ((RecordEntry)recordentry).Data as QryRequisicaoDetalheModel;

            RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
            vm.Planilha = (from p in vm.Planilhas where p.planilha == record.planilha select p).FirstOrDefault();
            await Task.Run(async () => await vm.GetProdutosAsync());
            vm.Produto = (from p in vm.Produtos where p.codigo == record.codigo select p).FirstOrDefault();
            await Task.Run(async () => await vm.GetDescAdicionaisAsync());
            vm.DescAdicional = (from d in vm.DescAdicionais where d.coduniadicional == record.coduniadicional select d).FirstOrDefault();
            await Task.Run(async () => await vm.GetCompleAdicionaisAsync());
            vm.Compledicional = (from c in vm.CompleAdicionais where c.codcompladicional == record.codcompladicional select c).FirstOrDefault();

            tbQuantidade.Value = record.quantidade;

            vm.RequisicaoDetalhe = new DetalheRequisicaoModel
            {
                cod_det_req = record.cod_det_req,
                num_requisicao = record.num_requisicao,
                codcompladicional = record.codcompladicional,
                quantidade = (float?)record.quantidade
            };

        }

        private async void tbCodproduto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    var dado = long.Parse(((SfTextBoxExt)sender).Text);
                    RequisicaoViewModel vm = (RequisicaoViewModel)DataContext;
                    await Task.Run(async () => await vm.GetDescricaoAsync(dado));

                    dbClick = true;
                    vm.Planilha = (from p in vm.Planilhas where p.planilha == vm.Descricao.planilha select p).FirstOrDefault();
                    await Task.Run(async () => await vm.GetProdutosAsync());
                    vm.Produto = (from p in vm.Produtos where p.codigo == vm.Descricao.codigo select p).FirstOrDefault();
                    await Task.Run(async () => await vm.GetDescAdicionaisAsync());
                    vm.DescAdicional = (from d in vm.DescAdicionais where d.coduniadicional == vm.Descricao.coduniadicional select d).FirstOrDefault();
                    await Task.Run(async () => await vm.GetCompleAdicionaisAsync());
                    vm.Compledicional = (from c in vm.CompleAdicionais where c.codcompladicional == vm.Descricao.codcompladicional select c).FirstOrDefault();

                    tbQuantidade.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void OnDropDownOpened(object sender, EventArgs e)
        {
            dbClick = false;
        }
    }
}
