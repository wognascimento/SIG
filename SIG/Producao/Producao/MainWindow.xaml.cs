using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Producao.Views;
using Producao.Views.CadastroProduto;
using Producao.Views.CentralModelos;
using Producao.Views.CheckList;
using Producao.Views.Estoque;
using Producao.Views.OrdemServico.Produto;
using Producao.Views.OrdemServico.Requisicao;
using Producao.Views.OrdemServico.Servicos;
using Producao.Views.Planilha;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;
using Syncfusion.XlsIO;
using SizeMode = Syncfusion.SfSkinManager.SizeMode;

namespace Producao
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
        }
        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentVisualStyle = "Metro"; // "FluentLight";
	        //CurrentSizeMode = "Touch";
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

        private void MenuItemAdv_Click(object sender, RoutedEventArgs e)
        {
            ViewAprovado viewAprovado = new();
            DocumentContainer.SetHeader(viewAprovado, "PRODUÇÃO APROVADOS");
            DocumentContainer.SetSizetoContentInMDI(viewAprovado, true);
            DocumentContainer.SetMDIBounds(viewAprovado, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(viewAprovado, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(viewAprovado);
        }

        private void OnChecklist_Click(object sender, RoutedEventArgs e)
        {
            ViewCheckListNatal view = new();
            DocumentContainer.SetHeader(view, "CHECKLIST NATAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnRevisaoChecklistClick(object sender, RoutedEventArgs e)
        {
            ViewCheckListRevisao view = new();
            DocumentContainer.SetHeader(view, "REVISÃO DE CHECKLIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);

        }

        private void OnEtiquetaChecklistClick(object sender, RoutedEventArgs e)
        {
            ViewEtiquetaCheckList view = new();
            //ViewEmitirEtiquetaCheckList view = new();
            DocumentContainer.SetHeader(view, "ETIQUETA CHECKLIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnEtiquetaChecklistEmitidaClick(object sender, RoutedEventArgs e)
        {
            ViewEtiquetaCheckListEmitida view = new();
            DocumentContainer.SetHeader(view, "ETIQUETA CHECKLIST EMITIDAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnCentralCriarModelo(object sender, RoutedEventArgs e)
        {

            ViewCentralCriarModelo view = new();
            DocumentContainer.SetHeader(view, "CRIAR MODELO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            
        }

        private void OnCentralTabelaPa(object sender, RoutedEventArgs e)
        {
            ViewCentralTabelaPA view = new();
            DocumentContainer.SetHeader(view, "TABELA ARVORE P.A");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnCentralFatorConversao(object sender, RoutedEventArgs e)
        {
            ViewCentralFatorConversao view = new();
            DocumentContainer.SetHeader(view, "TABELA FATOR CONVERSÃO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnCentralEmitirOs(object sender, RoutedEventArgs e)
        {
            ViewCentralEmitirOs view = new();
            DocumentContainer.SetHeader(view, "CONTROLE ORDEM DE SERVIÇO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1024.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnCentralStatusCheckList(object sender, RoutedEventArgs e)
        {
            ViewCentralStatusCheckList view = new();
            DocumentContainer.SetHeader(view, "STATUS CHECK-LIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }


        private void OnCreateReceitaRequisicao(object sender, RoutedEventArgs e)
        {
            ViewReceitaRequisicao view = new();
            DocumentContainer.SetHeader(view, "RECEITA REQUISIÇÃO MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnCadastroProduto(object sender, RoutedEventArgs e)
        {
            ViewCadastroProduto view = new();
            DocumentContainer.SetHeader(view, "CADASTRO DE PRODUTOS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnCadastroEspanhol(object sender, RoutedEventArgs e)
        {
            CadastroDescricaoEspanhol view = new();
            DocumentContainer.SetHeader(view, "CADASTRO DE DESCRIÇÃO ESPANHOL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnTodasDescricoes(object sender, RoutedEventArgs e)
        {
            TodasDescricoes view = new();
            DocumentContainer.SetHeader(view, "TODAS DESCRIÇÕES CADASTRADAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnEmitirOSServicoClick(object sender, RoutedEventArgs e)
        {
            EmissaoServico view = new();
            DocumentContainer.SetHeader(view, "EMISSÃO DE O.S. DE SERVIÇO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 800) / 2.0, (this._mdi.ActualHeight - 400) / 2.0, 800.0, 400));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnEmitidasOSServicoClick(object sender, RoutedEventArgs e)
        {
            EmissaoServicoEmitidas view = new();
            DocumentContainer.SetHeader(view, "O.S. DE SERVIÇO EMITIDAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnAlterarRequisicoes(object sender, RoutedEventArgs e)
        {
            RequisicaoMaterialAlterar view = new();
            DocumentContainer.SetHeader(view, "ALTERAR REQUISIÇÕES DE MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnRequisicoesEmitidas(object sender, RoutedEventArgs e)
        {
            RequisicaoMaterialEmitidas view = new();
            DocumentContainer.SetHeader(view, "REQUISIÇÕES DE MATERIAIS EMITIDAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnRequisicaClick(object sender, RoutedEventArgs e)
        {
            RequisicaoMaterialEmitir view = new();
            DocumentContainer.SetHeader(view, "EMITIR REQUISIÇÕES DE MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 250) / 2.0, (this._mdi.ActualHeight - 250.5) / 2.0, 250, 250.5));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnSolicitarOSProdutoClick(object sender, RoutedEventArgs e)
        {
            SolicitacaoOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "SOLICITAR ORDEM DE SERVIÇO DE PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnEmitirOSProdutoClick(object sender, RoutedEventArgs e)
        {
            EmitirOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "EMITIR ORDEM DE SERVIÇO DE PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 768) / 2.0, 1000, 768));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnAlterarSolicitacaoOSProdutoClick(object sender, RoutedEventArgs e)
        {
            AlterarSolicitacaoOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "ALTERAR SOLICITAR ORDEM DE SERVIÇO DE PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private async void OnPendenciaProducaoClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                using DatabaseContext db = new();
                var data = await db.PendenciaProducaos.ToListAsync();

                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                //worksheet.IsGridLinesVisible = false;
                worksheet.ImportData(data, 1, 1, true);

                workbook.SaveAs("Impressos/PENDENCIA_PRODUCAO.xlsx");
                Process.Start(new ProcessStartInfo("Impressos\\PENDENCIA_PRODUCAO.xlsx")
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

        private void ControleGrupoClick(object sender, RoutedEventArgs e)
        {
            ControleGrupo view = new();
            DocumentContainer.SetHeader(view, "CONTROLE POR GRUPO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }

        private void OnEntradaEstoqueClick(object sender, RoutedEventArgs e)
        {
            MovimentacaoEntrada view = new();
            DocumentContainer.SetHeader(view, "MOVIMENTAÇÃO ENTRADA ESTOQUE");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnSaidaEstoqueClick(object sender, RoutedEventArgs e)
        {
            MovimentacaoSaida view = new();
            DocumentContainer.SetHeader(view, "MOVIMENTAÇÃO SAÍDA ESTOQUE");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnBaixaRequisicaoClick(object sender, RoutedEventArgs e)
        {
            BaixaRequisicao view = new();
            DocumentContainer.SetHeader(view, "BAIXA DE REQUISIÇÃO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnSaldoEstoqueClick(object sender, RoutedEventArgs e)
        {
            SaldoEstoque view = new();
            DocumentContainer.SetHeader(view, "SALDO DE ESTOQUE");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 700) / 2.0, (this._mdi.ActualHeight - 250) / 2.0, 700, 250));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnRelatorioCCEClick(object sender, RoutedEventArgs e)
        {
            RelatorioCCE view = new();
            DocumentContainer.SetHeader(view, "RELATÓRIO C.C.E");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 400) / 2.0, (this._mdi.ActualHeight - 70) / 2.0, 400, 70));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnDigitarCCEClick(object sender, RoutedEventArgs e)
        {
            DigitacaoCCE view = new();
            DocumentContainer.SetHeader(view, "CONTROLE ESTOQUE PROCESSADO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void CompletarChecklistClick(object sender, RoutedEventArgs e)
        {
            ViewComplementoCheckListNatal view = new();
            DocumentContainer.SetHeader(view, "COMPLETAR CHECKLIST NATAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
        }
    }
}
