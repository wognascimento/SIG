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
using System.Linq;
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
                                foreach (var item in GetProdutosEtiqueta(volume))
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
                                foreach (var item in GetProdutosEtiqueta(volume))
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
                                foreach (var item in GetProdutosEtiqueta(volume))
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
                                foreach (var item in GetProdutosEtiqueta(volume))
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
            /*
            Process.Start(
                            new ProcessStartInfo(@"Impressos\ORDEM_SERVICO_MODELO.xlsx")
                            {
                                Verb = "Print",
                                UseShellExecute = true,
                            });
            */

            try
            {
                for (int i = 1; i < _pagina; i++)
                {
                    Process.Start(new ProcessStartInfo($"Impressos\\ETIQUETA_REQUISICAO_MODELO_{i}.xlsx")
                    {
                        Verb = "Print",
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }

            Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
        }

        private ObservableCollection<ConstrucaoPecaModel> GetProdutosEtiqueta(long volume)
        {
            EtiquetaConstrucaoViewModel vm = (EtiquetaConstrucaoViewModel)DataContext;
            return new ObservableCollection<ConstrucaoPecaModel>(vm.Pecas.Where(p => p.volume_etiqueta == volume).Take(5));
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

        private void OnEtiquetaTermicaClick(object sender, RoutedEventArgs e)
        {
            EtiquetaConstrucaoViewModel? vm = (EtiquetaConstrucaoViewModel)DataContext;
            if (vm.Compledicional == null)
            {
                MessageBox.Show("Produto não selecionado!");
                return;
            }
            MessageBox.Show("Impressão em impressora térmica, ainda não foi implementado.");
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
