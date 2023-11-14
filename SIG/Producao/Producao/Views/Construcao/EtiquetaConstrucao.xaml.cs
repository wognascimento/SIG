using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Producao.Views.CheckList;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Producao.Views.Construcao
{
    /// <summary>
    /// Interação lógica para EtiquetaConstrucao.xam
    /// </summary>
    public partial class EtiquetaConstrucao : UserControl
    {
        enum Etiqueta
        {
            Primeira,
            Segunda,
            Terceira,
            Quarta
        }

        public EtiquetaConstrucao()
        {
            InitializeComponent();
            DataContext = new EtiquetaConstrucaoViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
                vm.Planilhas = await Task.Run(vm.GetPlanilhasAsync);
                vm.Tamanhos = new ObservableCollection<string>() { "P", "M", "G", "GG" };
                vm.Dificuldades = new ObservableCollection<string>() { "FÁCIL", "MÉDIO", "DIFÍCIL", "MUITO DIFÍCIL" };
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
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
                EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
                PlanilhaConstrucaoModel? planilha = e.NewValue as PlanilhaConstrucaoModel;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                vm.Produtos = new ObservableCollection<ProdutoModel>();
                txtDescricao.SelectedItem = null;
                txtDescricao.Text = string.Empty;

                vm.DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
                txtDescricaoAdicional.SelectedItem = null;
                txtDescricaoAdicional.Text = string.Empty;

                vm.CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>();
                txtComplementoAdicional.SelectedItem = null;
                txtComplementoAdicional.Text = string.Empty;

                //txtUnidade.Text = string.Empty;

                vm.Produtos = await Task.Run(() => vm.GetProdutosAsync(planilha?.planilha));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                clearMedidas();
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
                EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
                ProdutoModel? produto = e.NewValue as ProdutoModel;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                vm.DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
                txtDescricaoAdicional.SelectedItem = null;
                txtDescricaoAdicional.Text = string.Empty;

                vm.CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>();
                txtComplementoAdicional.SelectedItem = null;
                txtComplementoAdicional.Text = string.Empty;

                //txtUnidade.Text = string.Empty;

                vm.DescAdicionais = await Task.Run(() => vm.GetDescAdicionaisAsync(produto?.codigo));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                clearMedidas();
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
                EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
                TabelaDescAdicionalModel? adicional = e.NewValue as TabelaDescAdicionalModel;
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                vm.CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>();
                txtComplementoAdicional.SelectedItem = null;
                txtComplementoAdicional.Text = string.Empty;

                //txtUnidade.Text = string.Empty;

                vm.CompleAdicionais = await Task.Run(() => vm.GetCompleAdicionaisAsync(adicional?.coduniadicional));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                clearMedidas();
                txtComplementoAdicional.Focus();
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnSelectedComplementoAdicional(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
                TblComplementoAdicionalModel? complemento = e.NewValue as TblComplementoAdicionalModel;
                vm.Compledicional = complemento;



                txt_largura.Text = String.Format("{0:F}", complemento?.largura);//complemento.largura.ToString();
                txt_area.Text = String.Format("{0:F}", complemento?.area);
                txt_profundidade.Text = String.Format("{0:F}", complemento?.profundidade);
                txt_volume.Text = String.Format("{0:F}", complemento?.volume);
                txt_altura.Text = String.Format("{0:F}", complemento?.altura);
                txt_peso.Text = String.Format("{0:F}", complemento?.peso);
                txt_tamanho.SelectedItem = complemento?.tamanho_construcao;
                txt_dificuldade.SelectedItem = complemento?.dificuldade;
                

                vm.Pecas = await Task.Run(() => vm.GetPecasAsync(complemento?.codcompladicional));
                if (vm.Pecas.Count == 0)
                {
                    vm.Detalhes = await Task.Run(() => vm.GetDetalhesAsync(complemento?.codcompladicional));
                    foreach (var det in vm.Detalhes)
                    {
                        await Task.Run(() => vm.SaveConstrucaoDetalheAsync(
                            new ConstrucaoPecaModel
                            {
                                ano = DateTime.Now.Year,
                                codcompladicional = det.codcompladicional,
                                item = det.item,
                                descricao_peca = det.descricao_peca,
                                volume_etiqueta = det.volume,
                            }));
                    }
                    vm.Pecas = await Task.Run(() => vm.GetPecasAsync(complemento?.codcompladicional));
                }
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
            
        }

        private void itens_AddNewRowInitiating(object sender, Syncfusion.UI.Xaml.Grid.AddNewRowInitiatingEventArgs e)
        {
            EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
            ((ConstrucaoPecaModel)e.NewObject).codcompladicional = vm.Compledicional?.codcompladicional;
        }

        private void itens_RowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {
            ConstrucaoPecaModel rowData = (ConstrucaoPecaModel)e.RowData;
            if (!rowData.codcompladicional.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("item", "Não é possível adicionar peça na construção.");
                e.ErrorMessages.Add("descricao_peca", "Não é possível adicionar peça na construção.");
                e.ErrorMessages.Add("volume", "Não é possível adicionar peça na construção.");
            }
            else if (!rowData.item.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("item", "Informe a sequência ordinal da Peça.");
            }
            else if (rowData.descricao_peca == "")
            {
                e.IsValid = false;
                e.ErrorMessages.Add("descricao_peca", "Informe a descrição da Peça.");
            }
            else if (!rowData.volume_etiqueta.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("volume", "Informe o agrupamento dos itens");
            }
        }

        private async void itens_RowValidated(object sender, Syncfusion.UI.Xaml.Grid.RowValidatedEventArgs e)
        {
            var sfdatagrid = sender as SfDataGrid;
            EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ConstrucaoPecaModel data = (ConstrucaoPecaModel)e.RowData;

                vm.Peca = await Task.Run(() => vm.SaveConstrucaoDetalheAsync(data));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                var toRemove = vm.Pecas.Where(x => x.id_detalhes == null).ToList();
                foreach (var item in toRemove)
                    vm.Pecas.Remove(item);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnEtiquetaClick(object sender, RoutedEventArgs e)
        {
            EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;

            if (vm.Compledicional == null)
            {
                MessageBox.Show("Produto não selecionado!");
                return;
            }

            Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
            //var filteredResult = grid.View.Records.Select(recordentry => recordentry.Data);
            var volumes = vm.Pecas.OrderBy(o => o.volume_etiqueta).GroupBy(user => user.volume_etiqueta).ToList();
            var count = volumes.Count;

            using ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Xlsx;
            IWorkbook workbook = application.Workbooks.Open("Modelos/ETIQUETA_REQUISICAO_MODELO.xlsx");
            IWorksheet worksheet = workbook.Worksheets[0];

            var etiqueta = Enum.Parse(typeof(Etiqueta), "Primeira");
            int paginas = (int)Math.Ceiling(Decimal.Divide(count, 4));
            int _etiqueta = 1;
            int _pagina = 1;

            vm.Descricao = await Task.Run(() => vm.GetDescricaoAsync(vm.Compledicional.codcompladicional));
            vm.ChecklistPrduto = await Task.Run(() => vm.GetChecklistPrdutoAsync(vm.Compledicional.codcompladicional));

            //foreach (EtiquetaEmitidaModel item in filteredResult)
            for (int i = 0; i < count; i++)
            {
                try
                {
                    long volume = (long)volumes[i].Key;
                    switch (etiqueta)
                    {
                        case Etiqueta.Primeira:
                            {
                                worksheet.Range["A1"].Text = vm.ChecklistPrduto.sigla;
                                worksheet.Range["B2"].Text = $"{volume} / {count}"; //item.volumes_total > 1 ? item.volumes + " / " + item.volumes_total : item.qtd.ToString();
                                worksheet.Range["C2"].Number = DateTime.Now.Year;
                                worksheet.Range["D2"].Number = (double)vm.ChecklistPrduto.coddetalhescompl;
                                worksheet.Range["B5"].Text = vm.ChecklistPrduto.local_shoppings;
                                worksheet.Range["A8"].Text = vm.Descricao.descricao_completa.Replace("ÚNICO", null);

                                var inx = 12;
                                worksheet.Range["A12"].Text = "";
                                worksheet.Range["A13"].Text = "";
                                worksheet.Range["A14"].Text = "";
                                worksheet.Range["A15"].Text = "";
                                worksheet.Range["A16"].Text = "";
                                foreach (var item in GetProdutosEtiqueta(volume, 5))
                                {
                                    worksheet.Range[$"A{inx}"].Text = item.descricao_peca;
                                    inx++;
                                }

                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["PRIMEIRA"].CellStyle.Font.Color = ExcelKnownColors.Black;

                                etiqueta = Enum.Parse(typeof(Etiqueta), "Segunda");
                                _etiqueta++;
                                workbook.SaveAs($"Impressos/ETIQUETA_REQUISICAO_MODELO_{_pagina}.xlsx");
                                break;
                            }
                        case Etiqueta.Segunda:
                            {
                                worksheet.Range["F1"].Text = vm.ChecklistPrduto.sigla;
                                worksheet.Range["G2"].Text = $"{volume} / {count}"; //item.volumes_total > 1 ? item.volumes + " / " + item.volumes_total : item.qtd.ToString();
                                worksheet.Range["H2"].Number = DateTime.Now.Year;
                                worksheet.Range["I2"].Number = (double)vm.ChecklistPrduto.coddetalhescompl;
                                worksheet.Range["G5"].Text = vm.ChecklistPrduto.local_shoppings;
                                worksheet.Range["F8"].Text = vm.Descricao.descricao_completa.Replace("ÚNICO", null);

                                var inx = 12;
                                worksheet.Range["F12"].Text = "";
                                worksheet.Range["F13"].Text = "";
                                worksheet.Range["F14"].Text = "";
                                worksheet.Range["F15"].Text = "";
                                worksheet.Range["F16"].Text = "";
                                foreach (var item in GetProdutosEtiqueta(volume, 5))
                                {
                                    worksheet.Range[$"F{inx}"].Text = item.descricao_peca;
                                    inx++;
                                }

                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["SEGUNDA"].CellStyle.Font.Color = ExcelKnownColors.Black;

                                etiqueta = Enum.Parse(typeof(Etiqueta), "Terceira");
                                _etiqueta++;
                                workbook.SaveAs($"Impressos/ETIQUETA_REQUISICAO_MODELO_{_pagina}.xlsx");
                                break;
                            }
                        case Etiqueta.Terceira:
                            {
                                worksheet.Range["A18"].Text = vm.ChecklistPrduto.sigla;
                                worksheet.Range["B19"].Text = $"{volume} / {count}"; //item.volumes_total > 1 ? item.volumes + " / " + item.volumes_total : item.qtd.ToString();
                                worksheet.Range["C19"].Number = DateTime.Now.Year;
                                worksheet.Range["D19"].Number = (double)vm.ChecklistPrduto.coddetalhescompl;
                                worksheet.Range["B22"].Text = vm.ChecklistPrduto.local_shoppings;
                                worksheet.Range["A25"].Text = vm.Descricao.descricao_completa.Replace("ÚNICO", null);

                                var inx = 29;
                                worksheet.Range["A29"].Text = "";
                                worksheet.Range["A30"].Text = "";
                                worksheet.Range["A31"].Text = "";
                                worksheet.Range["A32"].Text = "";
                                worksheet.Range["A33"].Text = "";
                                foreach (var item in GetProdutosEtiqueta(volume, 5))
                                {
                                    worksheet.Range[$"A{inx}"].Text = item.descricao_peca;
                                    inx++;
                                }

                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["TERCEIRA"].CellStyle.Font.Color = ExcelKnownColors.Black;

                                etiqueta = Enum.Parse(typeof(Etiqueta), "Quarta");
                                _etiqueta++;
                                workbook.SaveAs($"Impressos/ETIQUETA_REQUISICAO_MODELO_{_pagina}.xlsx");
                                break;
                            }
                        case Etiqueta.Quarta:
                            {
                                worksheet.Range["F18"].Text = vm.ChecklistPrduto.sigla;
                                worksheet.Range["G19"].Text = $"{volume} / {count}"; //item.volumes_total > 1 ? item.volumes + " / " + item.volumes_total : item.qtd.ToString();
                                worksheet.Range["H19"].Number = DateTime.Now.Year;
                                worksheet.Range["I19"].Number = (double)vm.ChecklistPrduto.coddetalhescompl;
                                worksheet.Range["G22"].Text = vm.ChecklistPrduto.local_shoppings;
                                worksheet.Range["F25"].Text = vm.Descricao.descricao_completa.Replace("ÚNICO", null);

                                var inx = 29;
                                worksheet.Range["F29"].Text = "";
                                worksheet.Range["F30"].Text = "";
                                worksheet.Range["F31"].Text = "";
                                worksheet.Range["F32"].Text = "";
                                worksheet.Range["F33"].Text = "";
                                foreach (var item in GetProdutosEtiqueta(volume, 5))
                                {
                                    worksheet.Range[$"F{inx}"].Text = item.descricao_peca;
                                    inx++;
                                }

                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                                worksheet.Range["QUARTA"].CellStyle.Font.Color = ExcelKnownColors.Black;

                                etiqueta = Enum.Parse(typeof(Etiqueta), "Primeira");
                                _etiqueta = 1;
                                workbook.SaveAs($"Impressos/ETIQUETA_REQUISICAO_MODELO_{_pagina}.xlsx");

                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["PRIMEIRA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["PRIMEIRA"].CellStyle.Font.Color = ExcelKnownColors.None;

                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["SEGUNDA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["SEGUNDA"].CellStyle.Font.Color = ExcelKnownColors.None;

                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["TERCEIRA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["TERCEIRA"].CellStyle.Font.Color = ExcelKnownColors.None;

                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["QUARTA"].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.None;
                                worksheet.Range["QUARTA"].CellStyle.Font.Color = ExcelKnownColors.None;

                                _pagina++;
                                break;
                            }
                        default: break;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                }

            }


            try
            {
                int idx = 1;
                for (int i = 0; i < paginas; i++)
                {
                    Process.Start(new ProcessStartInfo($"Impressos\\ETIQUETA_REQUISICAO_MODELO_{idx}.xlsx")
                    {
                        Verb = "Print",
                        UseShellExecute = true
                    });
                    idx++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }

            Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
        }

        private ObservableCollection<ConstrucaoPecaModel> GetProdutosEtiqueta(long volume, int itens)
        {
            EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
            return new ObservableCollection<ConstrucaoPecaModel>(vm.Pecas.Where(p => p.volume_etiqueta == volume).Take(itens));
        }

        private async void OnDescricaoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                EtiquetaConstrucaoViewModel? vm = (EtiquetaConstrucaoViewModel)DataContext;

                if (vm.Compledicional == null)
                {
                    MessageBox.Show("Produto não selecionado!");
                    return;
                }

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = application.Workbooks.Open("Modelos/DESCRICOES_CONSTRUCAO_MODELO.xlsx");
                IWorksheet worksheet = workbook.Worksheets[0];

                vm.Descricao = await Task.Run(() => vm.GetDescricaoAsync(vm.Compledicional.codcompladicional));
                vm.ChecklistPrduto = await Task.Run(() => vm.GetChecklistPrdutoAsync(vm.Compledicional.codcompladicional));

                worksheet.Range["A1"].Text = vm.Descricao.descricao_completa;
                worksheet.Range["A4"].Text = vm.ChecklistPrduto.sigla;
                worksheet.Range["A5"].Number = DateTime.Now.Year;
                worksheet.Range["A6"].Text = "CÓD. DET. COMPL.:";
                worksheet.Range["A7"].Number = (double)vm.ChecklistPrduto.coddetalhescompl;
                worksheet.Range["A8"].Text = vm.ChecklistPrduto.local_shoppings;
                worksheet.Range["A9"].Text = $"( {vm.Pecas.Count()} VOLUMES )"; //itens.View.Records.Count()
                worksheet.Range["A10"].Text = $"PEÇAS";
                //worksheet.Range["C3"].Text = Modelo.planilha;
                //worksheet.Range["C4"].Text = Modelo.descricao_completa;
                //worksheet.Range["C5"].Text = Modelo.tema;
                //worksheet.Range["A7"].Text = Modelo.obs_modelo;
                //worksheet.Range["H4"].Number = Convert.ToDouble(Modelo.multiplica);//String.Format(new CultureInfo("pt-BR"), "{0:G}", Modelo.multiplica); //Modelo.multiplica.ToString();
                //worksheet.Range["H4"].NumberFormat = "0.0";
                //Console.WriteLine(String.Format(new CultureInfo("pt-BR"), "{0:C}", 189.99));

                //var itens = (from i in vm.ItensReceita where i.quantidade > 0 select new { i.quantidade, i.planilha, i.descricao_completa, i.unidade }).ToList(); //new { a.Name, a.Age }

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
                bodyStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
                bodyStyle.Font.Size = 10;
                bodyStyle.EndUpdate();



                var index = 12;
                
                foreach (var item in vm.Pecas)
                {
                    worksheet.Range[$"A{index}"].Number = (double)item.item;
                    worksheet.Range[$"A{index}"].CellStyle = bodyStyle;
                    worksheet.Range[$"A{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                    worksheet.Range[$"B{index}:J{index}"].Text = item.descricao_peca;
                    worksheet.Range[$"B{index}:J{index}"].CellStyle = bodyStyle;
                    worksheet.Range[$"B{index}:J{index}"].Merge();
                    worksheet.Range[$"B{index}:J{index}"].RowHeight = 26;
                    worksheet.Range[$"B{index}:J{index}"].WrapText = true;

                    //worksheet.Range[$"K{index}"].Number = (double)item.volume_etiqueta;
                    worksheet.Range[$"K{index}"].CellStyle = bodyStyle;
                    worksheet.Range[$"K{index}"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                    index++;
                }
                
                workbook.SaveAs($"Impressos/DESCRICOES_CONSTRUCAO_MODELO_{vm.ChecklistPrduto.coddetalhescompl}.xlsx");
                workbook.Close();

                Process.Start(new ProcessStartInfo($"Impressos\\DESCRICOES_CONSTRUCAO_MODELO_{vm.ChecklistPrduto.coddetalhescompl}.xlsx")
                {
                    UseShellExecute = true
                });

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });

            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnEtiquetaTermicaClick(object sender, RoutedEventArgs e)
        {
            try
            {
                EtiquetaConstrucaoViewModel? vm = (EtiquetaConstrucaoViewModel)DataContext;
                if (vm.Compledicional == null)
                {
                    MessageBox.Show("Produto não selecionado!");
                    return;
                }

                var rDados = new Dictionary<string, object>();

                string ip = "192.168.2.192";
                int porta = 9100;
                var temperatura = "";

                var printQueue = new Queue<string>();
                int stepSize = 20;

                if (string.IsNullOrEmpty(ip))
                    throw new InvalidOperationException("Impressora não informada.");
                /*
                if (string.IsNullOrEmpty(temperatura))
                    throw new InvalidOperationException("Temperatura não informada.");
                */
                var volumes = vm.Pecas.OrderBy(o => o.volume_etiqueta).GroupBy(user => user.volume_etiqueta).ToList();
                vm.Descricao = await Task.Run(() => vm.GetDescricaoAsync(vm.Compledicional.codcompladicional));
                vm.ChecklistPrduto = await Task.Run(() => vm.GetChecklistPrdutoAsync(vm.Compledicional.codcompladicional));
                var fieldCount = volumes.Count;
                
                for (int i = 0; i < fieldCount; i++)
                {
                    long volume = (long)volumes[i].Key;
                    rDados.Add(volume.ToString(), Convert.IsDBNull(volumes[i]) ? null : volumes[i]);
                    //ConstrucaoPecaModel objt = (ConstrucaoPecaModel)volumes[i];
                    /*
                    foreach (var item in GetProdutosEtiqueta(volume))
                    {
                        worksheet.Range[$"A{inx}"].Text = item.descricao_peca;
                        inx++;
                    }
                    */
                    var dataToSend = new StringBuilder();
                    dataToSend.AppendLine("CT~~CD,~CC^~CT~");
                    dataToSend.AppendLine("^XA^CI28");
                    dataToSend.AppendLine("^PW799");
                    dataToSend.AppendLine("^FO0,0^GFA,07680,07680,00048,:Z64:");
                    dataToSend.AppendLine("eJzt2TEBAAAEAEHN9G+jARlMDPfzNfgI6XHZi4rneZ7neZ7neZ7neZ7neZ7nef76bkinDQEo3NE=:FD4C");
                    dataToSend.AppendLine("^FO0,128^GFA,16000,16000,00100,:Z64:");
                    dataToSend.AppendLine("eJztzTEJAAAIADCb2b+ToB0ED2ErsAgAWMg+Vg6Hw+FwOBwOh8PhcDgcDofD4XA4HA6Hw+FwOBwOh8PhcDgcDofD4XA4vh0AwHsDPozsxw==:CD3A");

                    int inx = 1;
                    foreach (var item in GetProdutosEtiqueta(volume, 4))
                    {
                        if(inx == 1)
                        {
                            dataToSend.AppendLine("^FO0,256^GFA,06400,06400,00100,:Z64:");
                            dataToSend.AppendLine("eJztz0ENwCAYxWAOU4OKJ2J4JFO5wxT8h45A+hlo2pokbWLQ7n8+QgcuOvAJHfCjJHTAj5LQAT9KQgf8KAkdOOijP7BJT0iSJElLvXZeZAg=:73CE");
                            dataToSend.AppendLine($"^FT255,285^AAN,18,10^FH\\^FD{item.descricao_peca}^FS");
                        }

                        if (inx == 2)
                        {
                            dataToSend.AppendLine("^FO0,288^GFA,03200,03200,00100,:Z64:");
                            dataToSend.AppendLine("eJztzzERACAMBMEUqEHFiwCPDCopUJDiKMKvgZuLsO9M2njzITrQ6MAlOuCPFNEBf6SIDvgjRXTAHymiA4U++oYteqKUA1xDZAg=:AB30");
                            dataToSend.AppendLine($"^FT255,310^AAN,18,10^FH\\^FD{item.descricao_peca}^FS");
                        }

                        if (inx == 3)
                        {
                            dataToSend.AppendLine("^FO0,288^GFA,06400,06400,00100,:Z64:");
                            dataToSend.AppendLine("eJztz0ENwCAYxWAOU4OKJ4J5JKjkgIL/0IUs/Qw0bU2SJEnS9V7a+OYjdOChA0fogB8loQN+lIQO+FESOuBHSejAjz76gk16QtIFNvvmZAg=:5269");
                            dataToSend.AppendLine($"^FT255,335^AAN,18,10^FH\\^FD{item.descricao_peca}^FS");
                        }

                        if (inx == 4)
                        {
                            dataToSend.AppendLine("^FO0,320^GFA,06400,06400,00100,:Z64:");
                            dataToSend.AppendLine("eJztz0ENwCAYxWAOU4OKJwI8kqncAQX/knLY+hlo2pokSdKPTNo48xE6cNGBLXTAj5LQAT9KQgf8KAkd8KMkdOBDH/2GLXpCkqTXHo8YZAg=:E99D");
                            dataToSend.AppendLine($"^FT255,360^AAN,18,10^FH\\^FD{item.descricao_peca}^FS");
                        }

                        inx++;

                    }

                    dataToSend.AppendLine("^FO352,0^GFA,01920,01920,00020,:Z64:");
                    dataToSend.AppendLine("eJxjYBgFJACmVShgAU51qFyFUXWj6kbVjaojqI7I8mUUDBMAAAn+F3M=:F17D");
                    dataToSend.AppendLine("^FO640,0^GFA,01920,01920,00020,:Z64:");
                    dataToSend.AppendLine("eJxjYBgF2IFoKCoIwaFOAI3PMqpuVN2oulF1BNQRW76MguEMAPHDDOU=:BA27");
                    dataToSend.AppendLine("^FO352,64^GFA,05376,05376,00056,:Z64:");
                    dataToSend.AppendLine("eJztzrERABAUREGBSCTRGT0aVWpAwp8R7eY371ICgJM27vVArz5ssp6enp6enp6ent7nXln3ZuAnELUBxog13Q==:EA59");
                    dataToSend.AppendLine($"^FT302,395^A0N,18,16^FH\\^FDTOTAL DE {fieldCount} VOLUMES^FS");
                    dataToSend.AppendLine("^FT426,37^A0N,18,16^FB29,1,0,C^FH\\^FDANO^FS");
                    dataToSend.AppendLine($"^FT406,62^AAN,27,15^FB73,1,0,C^FH\\^FD{DateTime.Now.Year}^FS");
                    dataToSend.AppendLine("^FT690,36^A0N,18,16^FB70,1,0,C^FH\\^FDCÓD. DET.^FS");
                    dataToSend.AppendLine($"^FT690,61^AAN,27,15^FB73,1,0,C^FH\\^FD{vm.ChecklistPrduto.coddetalhescompl}^FS");
                    dataToSend.AppendLine("^FT507,90^A0N,18,16^FB163,1,0,C^FH\\^FDLOCAL SHOPPING^FS");
                    dataToSend.AppendLine($"^FT377,111^AAN,18,10^FB409,1,0,C^FH\\^FD{vm.ChecklistPrduto.local_shoppings[0..34]}^FS"); //34 caracteres
                    dataToSend.AppendLine($"^FT377,136^AAN,18,10^FB409,1,0,C^FH\\^FD{vm.ChecklistPrduto.local_shoppings[35..]}^FS");
                    dataToSend.AppendLine("^FO59,23^GB263,34,34^FS");
                    dataToSend.AppendLine("^FT59,50^A0N,28,28^FB263,1,0,C^FR^FH\\^FDETIQUERA PRODUÇÃO^FS");
                    dataToSend.AppendLine("^FO165,79^GB52,34,34^FS");
                    dataToSend.AppendLine($"^FT165,106^A0N,28,28^FB52,1,0,C^FR^FH\\^FD{vm.ChecklistPrduto.sigla}^FS");
                    dataToSend.AppendLine("^FO236,171^GB329,34,34^FS");
                    dataToSend.AppendLine($"^FT236,198^A0N,28,28^FB329,1,0,C^FR^FH\\^FD{vm.Descricao.descricao}-{vm.Descricao.descricao_adicional}^FS");
                    dataToSend.AppendLine("^FO123,216^GB555,34,34^FS");
                    dataToSend.AppendLine($"^FT123,243^A0N,28,28^FB555,1,0,C^FR^FH\\^FD{vm.Descricao.complementoadicional}^FS");
                    dataToSend.AppendLine("^PQ1,0,1,Y^XZ");
                    dataToSend.AppendLine();

                    printQueue.Enqueue(dataToSend.ToString());

                    //rDados.Clear();
                }

                while (printQueue.Any())
                {
                    using var client = new TcpClient();
                    var serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), porta);
                    //IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), porta);
                    client.Connect(serverEndPoint);
                    using var clientStream = client.GetStream();
                    for (int i = 0; i < stepSize; i++)
                    {
                        var encoder = new ASCIIEncoding();
                        byte[] buffer = encoder.GetBytes(printQueue.Dequeue());
                        clientStream.Write(buffer, 0, buffer.Length);
                        if (!printQueue.Any()) break;
                    }
                    clientStream.Flush();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            /*
            EtiquetaConstrucaoViewModel? vm = (EtiquetaConstrucaoViewModel)DataContext;
            if (vm.Compledicional == null)
            {
                MessageBox.Show("Produto não selecionado!");
                return;
            }
            MessageBox.Show("Impressão em impressora térmica, ainda não foi implementado.");
            */
        }

        private void clearMedidas()
        {
            txt_largura.Text = null;
            txt_area.Text = null;
            txt_profundidade.Text = null;
            txt_volume.Text = null;
            txt_altura.Text = null;
            txt_peso.Text = null;
            txt_tamanho.Text = null;
            txt_dificuldade.Text = null;
        }
    }

    public class EtiquetaConstrucaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private ObservableCollection<PlanilhaConstrucaoModel> _planilhas;
        public ObservableCollection<PlanilhaConstrucaoModel> Planilhas
        {
            get { return _planilhas; }
            set { _planilhas = value; RaisePropertyChanged("Planilhas"); }
        }
        private PlanilhaConstrucaoModel _planilha;
        public PlanilhaConstrucaoModel Planilha
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
        private ChecklistPrdutoConstrucaoModel _checklistPrduto;
        public ChecklistPrdutoConstrucaoModel ChecklistPrduto
        {
            get { return _checklistPrduto; }
            set { _checklistPrduto = value; RaisePropertyChanged("ChecklistPrduto"); }
        }
        private ObservableCollection<ConstrucaoDetalheModel> _detalhes;
        public ObservableCollection<ConstrucaoDetalheModel> Detalhes
        {
            get { return _detalhes; }
            set { _detalhes = value; RaisePropertyChanged("Detalhes"); }
        }
        private ConstrucaoDetalheModel _detalhe;
        public ConstrucaoDetalheModel Detalhe
        {
            get { return _detalhe; }
            set { _detalhe = value; RaisePropertyChanged("Detalhe"); }
        }
        
        private ObservableCollection<ConstrucaoPecaModel> _pecas;
        public ObservableCollection<ConstrucaoPecaModel> Pecas
        {
            get { return _pecas; }
            set { _pecas = value; RaisePropertyChanged("Pecas"); }
        }
        private ConstrucaoPecaModel _peca;
        public ConstrucaoPecaModel Peca
        {
            get { return _peca; }
            set { _peca = value; RaisePropertyChanged("Peca"); }
        }

        private ObservableCollection<string> _tamanhos;
        public ObservableCollection<string> Tamanhos
        {
            get { return _tamanhos; }
            set { _tamanhos = value; RaisePropertyChanged("Tamanhos"); }
        }

        private ObservableCollection<string> _dificuldades;
        public ObservableCollection<string> Dificuldades
        {
            get { return _dificuldades; }
            set { _dificuldades = value; RaisePropertyChanged("Dificuldades"); }
        }

        public async Task<ObservableCollection<PlanilhaConstrucaoModel>> GetPlanilhasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                return new ObservableCollection<PlanilhaConstrucaoModel>(await db.PlanilhasConstrucao.OrderBy(c => c.planilha).ToListAsync());
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
                Produtos = new ObservableCollection<ProdutoModel>();
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
                DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
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

        public async Task<ObservableCollection<ConstrucaoDetalheModel>> GetDetalhesAsync(long? codcompladicional)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ConstrucaoDetalhes
                    .OrderBy(c => c.item)
                    .Where(c => c.codcompladicional == codcompladicional)
                    .ToListAsync();
                return new ObservableCollection<ConstrucaoDetalheModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<ObservableCollection<ConstrucaoPecaModel>> GetPecasAsync(long? codcompladicional)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ConstrucaoPecas
                    .OrderBy(c => c.item)
                    .Where(c => c.codcompladicional == codcompladicional)
                    .ToListAsync();
                return new ObservableCollection<ConstrucaoPecaModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ConstrucaoPecaModel> SaveConstrucaoDetalheAsync(ConstrucaoPecaModel construcaoPeca)
        {
            try
            {
                using DatabaseContext db = new();
                await db.ConstrucaoPecas.SingleMergeAsync(construcaoPeca);
                await db.SaveChangesAsync();
                return construcaoPeca;
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
                return await db.Descricoes.Where(c => c.codcompladicional == codcompladicional).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<ChecklistPrdutoConstrucaoModel> GetChecklistPrdutoAsync(long? codcompladicional)
        {
            try
            {
                using DatabaseContext db = new();
                return await db.ChecklistPrdutoConstrucaos.Where(c => c.codcompladicional == codcompladicional).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
