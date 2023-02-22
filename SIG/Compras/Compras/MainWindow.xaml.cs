﻿using Compras.Views;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;
using Syncfusion.XlsIO;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Enum = System.Enum;
using SizeMode = Syncfusion.SfSkinManager.SizeMode;

namespace Compras
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		#region Fields
        private string currentVisualStyle;
		private string currentSizeMode;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current visual style.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string CurrentVisualStyle
        {
            get
            {
                return currentVisualStyle;
            }
            set
            {
                currentVisualStyle = value;
                OnVisualStyleChanged();
            }
        }
		
		/// <summary>
        /// Gets or sets the current Size mode.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string CurrentSizeMode
        {
            get
            {
                return currentSizeMode;
            }
            set
            {
                currentSizeMode = value;
                OnSizeModeChanged();
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
			this.Loaded += OnLoaded;

            DataBaseSettings BaseSettings = DataBaseSettings.Instance;
            txtUsername.Text = BaseSettings.Username;
            txtDataBase.Text = BaseSettings.Database;

            //MessageBox.Show(DateTime.Now.Month.ToString());

            
        }
		/// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentVisualStyle = "Metro";
	        CurrentSizeMode = "Default";
        }
		/// <summary>
        /// On Visual Style Changed.
        /// </summary>
        /// <remarks></remarks>
        private void OnVisualStyleChanged()
        {
            VisualStyles visualStyle = VisualStyles.Default;
            Enum.TryParse(CurrentVisualStyle, out visualStyle);            
            if (visualStyle != VisualStyles.Default)
            {
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetVisualStyle(this, visualStyle);
                SfSkinManager.ApplyStylesOnApplication = false;
            }
        }
		
		/// <summary>
        /// On Size Mode Changed event.
        /// </summary>
        /// <remarks></remarks>
        private void OnSizeModeChanged()
        {
            SizeMode sizeMode = SizeMode.Default;
            Enum.TryParse(CurrentSizeMode, out sizeMode);
            if (sizeMode != SizeMode.Default)
            {
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetSizeMode(this, sizeMode);
                SfSkinManager.ApplyStylesOnApplication = false;
            }
        }

        private void OnOpenSolicitacao(object sender, RoutedEventArgs e)
        {
            ViewSolicitacao view = new();
            DocumentContainer.SetHeader(view, "SOLICITAÇÃO MATERIAL/SERVIÇO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnOpenEncaminhamento(object sender, RoutedEventArgs e)
        {
            ViewSolicitacaoEncaminhamento view = new();
            DocumentContainer.SetHeader(view, "ENCAMINHAMENTO SOLICITAÇÃO MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnOpenEncaminhamentoServico(object sender, RoutedEventArgs e)
        {

        }

        private async void OnImportFile(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "PEDIDO-COMPRA"; // Default file name
            dialog.DefaultExt = ".xlsx"; // Default file extension
            dialog.Filter = "Pasta de Trabalho do Excel (.xlsx)|*.xlsx"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                try
                {
                    string filename = dialog.FileName;
                    using ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;
                    //IWorkbook workbook = application.Workbooks.Open(filename);
                    IWorkbook workbook = application.Workbooks.OpenReadOnly(filename);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //2206

                    //Access a cell value from Excel
                    var pedido = worksheet.Range["E4"].Value;
                    var PedidoDt = worksheet.Range["G4"].Value;
                    var PedidoEntrega = worksheet.Range["C9"].Value;
                    var empresa = worksheet.Range["C6"].Value;
                    var fornecedor = worksheet.Range["C7"].Value;
                    var condicoes = worksheet.Range["D8"].Value;

                    if (pedido == "" || pedido == "#N/A")
                    {
                        MessageBox.Show("Nº do pedido não informado", "Valiação de Dados");
                        return;
                    }
                    else if (PedidoDt == "" || PedidoDt == "#N/A")
                    {
                        MessageBox.Show("Data do pedido não informada", "Valiação de Dados");
                        return;
                    }
                    else if (empresa == "" || empresa == "#N/A")
                    {
                        MessageBox.Show("Empresa Cipolatti não informada", "Valiação de Dados");
                        return;
                    }
                    else if (fornecedor == "" || fornecedor == "#N/A")
                    {
                        MessageBox.Show("Fornecedor não informado", "Valiação de Dados");
                        return;
                    }
                    else if (condicoes == "" || condicoes == "#N/A")
                    {
                        MessageBox.Show("Condições de pagamento não informada", "Valiação de Dados");
                        return;
                    }
                    else if (PedidoEntrega == "" || PedidoEntrega == "#N/A")
                    {
                        MessageBox.Show("Data de entrega não informada", "Valiação de Dados");
                        return;
                    }

                    ObservableCollection<PedidoDetalhesModel> produtos = new();
                    for (int i = 12; i < 31; i++)
                    {
                       if (worksheet.Range[$"A{i}"].Value == "" || worksheet.Range[$"A{i}"].Value == "#N/A")
                            break;

                       if(worksheet.Range[$"E{i}"].Value == "" || worksheet.Range[$"E{i}"].Value == "#N/A")
                        {
                            MessageBox.Show($"Valor não informado para o produro {worksheet.Range[$"B{i}"].Value}", "Valiação de Dados");
                            return;
                        }

                        if (worksheet.Range[$"F{i}"].Value == "" || worksheet.Range[$"F{i}"].Value == "#N/A")
                        {
                            MessageBox.Show($"Quantida não informado para o produro {worksheet.Range[$"B{i}"].Value}. \nSe o Produto não compor ao pedido deixa a quantidade zerada(0)", "Valiação de Dados");
                            return;
                        }

                        var qtde = worksheet.Range[$"F{i}"].Value;
                        var vlrunit = worksheet.Range[$"E{i}"].Value;
                        var codcompladicional = worksheet.Range[$"A{i}"].Value;
                        var obs = worksheet.Range[$"I{i}"].Value;
                        var itens = worksheet.Range[$"J{i}"].Value;
                        produtos.Add(
                            new PedidoDetalhesModel
                            {
                                idpedido = long.Parse(pedido),
                                codcompladicional = long.Parse(codcompladicional),
                                qtde = double.Parse(qtde),
                                vlrunit = double.Parse(vlrunit),
                                obs = obs,
                                classificacao_cipolatti = "VERIFICAR",
                                solicitacao = itens
                            });
                    }

                    try
                    {
                        var _pedido = new PedidoModel 
                        {
                            idpedido = long.Parse(pedido), 
                            codempresa = long.Parse(empresa),
                            codfornecedor= long.Parse(fornecedor),
                            id_cond_pagamento= long.Parse(condicoes),
                            status = "FINALIZADO", 
                            status_por = Environment.UserName, 
                            status_data = DateTime.Now,
                            data_emissao_nf = DateTime.Parse(PedidoEntrega),
                            dataentrega = DateTime.Parse(PedidoEntrega),
                            comprador = Environment.UserName,
                        };

                        var itens =  await InsertProdutoPedido(produtos, _pedido);

                        MessageBox.Show("Arquivo importado com sucesso!", "Pedido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    workbook.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private async Task<ObservableCollection<PedidoDetalhesModel>> InsertProdutoPedido(ObservableCollection<PedidoDetalhesModel> produtos, PedidoModel pedido)
        {
            using DatabaseContext db = new();
            using var transaction = db.Database.BeginTransaction();
            try
            {
                //await db.BulkInsertAsync(produtos);

                //var pedido = (from p in produtos select p).FirstOrDefault();

                db.PedidoDetalhes.AddRange(produtos);
                await db.SaveChangesAsync();

                db.Pedidos.Update(pedido);
                await db.SaveChangesAsync();

                transaction.Commit();

                return produtos;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private void OnAbrirPedidos(object sender, RoutedEventArgs e)
        {
            ViewPedidos view = new();
            DocumentContainer.SetHeader(view, "TODOS PEDIDOS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnOpenCadastroFornecedor(object sender, RoutedEventArgs e)
        {
            ViewCadastroFornecedor view = new();
            DocumentContainer.SetHeader(view, "CADASTRO FORNECEDOR");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnOpenCastroCondicaoPagamento(object sender, RoutedEventArgs e)
        {
            ViewCadastroCondicaoPagamento view = new();
            DocumentContainer.SetHeader(view, "CADASTRO CONDIÇÃO DE PAGAMENTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnOpenCadastroFamiliaComprador(object sender, RoutedEventArgs e)
        {
            ViewCadastroFamiliaComprador view = new();
            DocumentContainer.SetHeader(view, "CADASTRO COMPRADOR(A) FAMÍLIA");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 500.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 500.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnOpenTodasDescricoes(object sender, RoutedEventArgs e)
        {
            ViewConsultaProdutos view = new();
            DocumentContainer.SetHeader(view, "TODOS PRONTOS CIPOLATTI");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnOpenConsultaGerencial(object sender, RoutedEventArgs e)
        {
            ViewConsultaGerencial view = new();
            DocumentContainer.SetHeader(view, "CONSULTA GERENCIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }
    }
}
