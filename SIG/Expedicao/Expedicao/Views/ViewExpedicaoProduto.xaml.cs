using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TextInputLayout;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Expedicao.Views
{
    /// <summary>
    /// Interação lógica para ViewExpedicaoProduto.xam
    /// </summary>
    public partial class ViewExpedicaoProduto : UserControl
    {

        private ProdutoExpedidoModel ProdutoExpedido;

        public ViewExpedicaoProduto()
        {
            InitializeComponent();
            this.dataGrid.SearchHelper = new LocalizarHelperExt(dataGrid);

            this.Exped.SelectionController = new GridSelectionControllerExt(Exped);
        }

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            try
            {
                this.aprovados.ItemsSource = await Task.Run(async () => await new AprovadoViewModel().GetAprovados());
                this.Caixas.ItemsSource = await Task.Run(async () => await new MedidaViewModel().GetMedidasAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Aprovados_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.SelectionChangedEventArgs e)
        {
            AprovadoModel? aprovado = ((SfMultiColumnDropDownControl)sender).SelectedItem as AprovadoModel;
            try
            {
                this.dataGrid.ItemsSource = null;
                this.loadingExped.Visibility = Visibility.Hidden;
                this.loadingDetalhes.Visibility = Visibility.Visible;
                this.dataGrid.ItemsSource = await Task.Run(async () => await new ExpedicaoViewModel().GetProdutoExpedidos((int)aprovado.IdAprovado));
                this.loadingDetalhes.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void DataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //ViewExpedicaoProduto expedicaoProduto = this;
            try
            {
                Exped.ItemsSource = null;
                loadingExped.Visibility = Visibility.Visible;
                if (e.AddedItems.Count <= 0)
                    return;
                ProdutoExpedido = (e.AddedItems[0] as GridRowInfo).RowData as ProdutoExpedidoModel;

                Exped.ItemsSource = await Task.Run(async () => await new ExpedicaoViewModel().GetExpedsAsync((int)ProdutoExpedido.CodDetalhesCompl));
                loadingExped.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exped_AddNewRowInitiating(object sender, AddNewRowInitiatingEventArgs e)
        {
            (e.NewObject as ExpedModel).CodDetalhesCompl = new long?((long)ProdutoExpedido.CodDetalhesCompl);
        }

        private void Exped_RecordDeleted(object sender, RecordDeletedEventArgs e)
        {

        }

        private async void Exped_RecordDeleting(object sender, RecordDeletingEventArgs e)
        {

            if (MessageBox.Show("Confirma a exclusão do item?", "Excluir", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    ExpedModel data = e.Items[0] as ExpedModel;
                    await Task.Run((async () => await new ExpedicaoViewModel().DeleteExpedAsync(data)));
                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    int num2 = (int)MessageBox.Show(ex.Message);
                }
            }
            else
                e.Cancel = true;
        }

        private async void Exped_RowValidated(object sender, RowValidatedEventArgs e)
        {
            try
            {
                ExpedModel data = e.RowData as ExpedModel;
                ExpedModel expedModel = await Task.Run(async () => await new ExpedicaoViewModel().AddExpedAsync(data));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exped_RowValidating(object sender, RowValidatingEventArgs e)
        {
            ExpedModel rowData = e.RowData as ExpedModel;
            if (!rowData.CodDetalhesCompl.HasValue)
            {
                e.IsValid = false;
                e.ErrorMessages.Add("QtdExpedida", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("VolExp", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("VolTotExp", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("Pl", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("Pb", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("Largura", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("Altura", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("Profundidade", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("ModeloCaixa", "Erro ao selecionar a linha.");
                e.ErrorMessages.Add("Volume", "Erro ao selecionar a linha.");
            }
            else
            {
                //decimal? qtdExpedida = (decimal?)rowData.QtdExpedida;
                if (!rowData.QtdExpedida.HasValue)
                {
                    e.IsValid = false;
                    e.ErrorMessages.Add("QtdExpedida", "qtd_expedida não pode ser nulo.");
                }
                else
                {
                    //Math.Round( 2.123455909, 2);
                    //qtdExpedida = (decimal?)rowData.QtdExpedida;
                    //decimal? nullable1 = (decimal?)this.ProdutoExpedido.Qtd;
                    if (Math.Round((double)rowData.QtdExpedida,2) > Math.Round((double)ProdutoExpedido.Qtd,2) & rowData.QtdExpedida.HasValue & ProdutoExpedido.Qtd.HasValue)
                    {
                        e.IsValid = false;
                        e.ErrorMessages.Add("QtdExpedida", "qtd_expedida não pode ser maior que qtd do cheklist.");
                    }
                    else
                    {
                        
                        if (!rowData.VolExp.HasValue)
                        {
                            e.IsValid = false;
                            e.ErrorMessages.Add("VolExp", "vol_exp não pode ser nulo.");
                        }
                        else
                        {
                            if (!rowData.VolTotExp.HasValue)
                            {
                                e.IsValid = false;
                                e.ErrorMessages.Add("VolTotExp", "vol_tot_exp não pode ser nulo.");
                            }
                            else
                            {
                                if (!rowData.Pl.HasValue)
                                {
                                    e.IsValid = false;
                                    e.ErrorMessages.Add("Pl", "pl não pode ser nulo.");
                                }
                                else
                                {
                                    if (!rowData.Pb.HasValue)
                                    {
                                        e.IsValid = false;
                                        e.ErrorMessages.Add("Pb", "pb não pode ser nulo.");
                                    }
                                    else
                                    {
                                        if (rowData.ModeloCaixa == null)
                                        {
                                            if (!rowData.Largura.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Largura", "Precisa informar uma das formas de medida.");
                                                return;
                                            }
                                            if (!rowData.Altura.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Altura", "Precisa informar uma das formas de medida.");
                                                return;
                                            }
                                            if (!rowData.Profundidade.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Profundidade", "Precisa informar uma das formas de medida.");
                                                return;
                                            }
                                        }
                                        if (rowData.ModeloCaixa != null && rowData.ModeloCaixa != "CX")
                                        {
                                            if (rowData.Largura.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Largura", "Precisa informar apenas tipo da caixa ou as medidas.");
                                                return;
                                            }
                                            if (rowData.Altura.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Altura", "Precisa informar apenas tipo da caixa ou as medidas.");
                                                return;
                                            }
                                            if (rowData.Profundidade.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Profundidade", "Precisa informar apenas tipo da caixa ou as medidas.");
                                                return;
                                            }
                                        }
                                        if (rowData.ModeloCaixa == "CX")
                                        {
                                            if (!rowData.Largura.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Largura", "Com tipo de caixa CX informado, precisa informar as medidas.");
                                                return;
                                            }
                                            if (!rowData.Altura.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Altura", "Com tipo de caixa CX informado, precisa informar as medidas.");
                                                return;
                                            }
                                            if (!rowData.Profundidade.HasValue)
                                            {
                                                e.IsValid = false;
                                                e.ErrorMessages.Add("Profundidade", "Com tipo de caixa CX informado, precisa informar as medidas.");
                                                return;
                                            }

                                        }
                                        if (rowData.Volume.HasValue)
                                            return;
                                        e.IsValid = false;
                                        e.ErrorMessages.Add("Volume", "Informe o número do volume.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /* 
                 MessageBox.Show("Ctrl+G detected, NO Alt/Shift/Windows");
            */

            //if ((e.Key == Key.G) && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control))
            //if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.L))
            if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None && e.Key == Key.L)
                Localizar();


        }

        private void Localizar()
        {
            this.dataGrid.SelectedItems.Clear();
            this.dataGrid.SearchHelper.ClearSearch();
            var window = new Window();
            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            stackPanel.Margin = new Thickness(5, 5, 5, 5);
            var inputLayout = new SfTextInputLayout();
            inputLayout.Hint = "Localizar";
            TextBox textBox = new();
            textBox.PreviewKeyDown += (s, e) => 
            {
                if (e.Key == Key.Enter)
                    PerformSearch(textBox.Text);
                else if (e.Key == Key.Escape)
                    window.Close();
            };
            inputLayout.InputView = textBox;
            stackPanel.Children.Add(inputLayout);
            FocusManager.SetFocusedElement(stackPanel, textBox);
            window.Content = stackPanel;
            window.Title = "Localizar código expedição";
            window.Height = 120;
            window.Width = 350;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowStyle = WindowStyle.ToolWindow;
            window.ResizeMode = ResizeMode.NoResize;
            window.ShowDialog();
        }

        private void PerformSearch(string texto)
        {
            /*
            if (this.dataGrid.SearchHelper.SearchText.Equals(texto))
                return;

            var text = texto;
            //AllowCaseSensitiveSearch  - true -> improves the performance when search numeric fields.
            this.dataGrid.SearchHelper.AllowCaseSensitiveSearch = true;
            this.dataGrid.SearchHelper.SearchType = SearchType.Contains;
            this.dataGrid.SearchHelper.AllowFiltering = true;
            this.dataGrid.SearchHelper.Search(text);
            */

            this.dataGrid.SearchHelper.FindNext(texto);
            var rowIndex = this.dataGrid.SearchHelper.CurrentRowColumnIndex.RowIndex;
            var recordIndex = this.dataGrid.ResolveToRecordIndex(rowIndex);
            this.dataGrid.SelectedIndex = recordIndex;
        }



    }
}
