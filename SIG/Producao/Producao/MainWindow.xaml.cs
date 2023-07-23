using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Producao.DataBase.Model;
using Producao.Views;
using Producao.Views.CadastroProduto;
using Producao.Views.CentralModelos;
using Producao.Views.CheckList;
using Producao.Views.Controlado;
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

        private void adicionarFilho(object filho, string title, string name)
        {
            var doc = ExistDocumentInDocumentContainer(name);
            if (doc == null)
            {
                doc = (FrameworkElement?)filho;
                DocumentContainer.SetHeader(doc, title);
                doc.Name = name.ToLower();
                _mdi.Items.Add(doc);
            }
            else
            {
                //_mdi.RestoreDocument(doc as UIElement);
                _mdi.ActiveDocument = doc;
            }
        }

        private FrameworkElement ExistDocumentInDocumentContainer(string name_)
        {
            foreach (FrameworkElement element in _mdi.Items)
            {
                if (name_.ToLower() == element.Name)
                {
                    return element;
                }
            }
            return null;
        }

        private void MenuItemAdv_Click(object sender, RoutedEventArgs e)
        {
            /*
            ViewAprovado viewAprovado = new();
            DocumentContainer.SetHeader(viewAprovado, "PRODUÇÃO APROVADOS");
            DocumentContainer.SetSizetoContentInMDI(viewAprovado, true);
            DocumentContainer.SetMDIBounds(viewAprovado, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(viewAprovado, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(viewAprovado);
            */

            adicionarFilho(new ViewAprovado(), "PRODUÇÃO APROVADOS", "APROVADOS");
        }

        private void OnChecklist_Click(object sender, RoutedEventArgs e)
        {
            /*
            ViewCheckListNatal view = new();
            DocumentContainer.SetHeader(view, "CHECKLIST NATAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCheckListNatal(), "CHECKLIST NATAL", "CHECKLIST_NATAL");
        }

        private void OnRevisaoChecklistClick(object sender, RoutedEventArgs e)
        {
            /*
            ViewCheckListRevisao view = new();
            DocumentContainer.SetHeader(view, "REVISÃO DE CHECKLIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCheckListRevisao(), "REVISÃO DE CHECKLIST", "REVISAO_CHECKLIST");

        }

        private void OnEtiquetaChecklistClick(object sender, RoutedEventArgs e)
        {
            /*
            ViewEtiquetaCheckList view = new();
            DocumentContainer.SetHeader(view, "ETIQUETA CHECKLIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewEtiquetaCheckList(), "ETIQUETA CHECKLIST", "ETIQUETA_CHECKLIST");
        }

        private void OnEtiquetaChecklistEmitidaClick(object sender, RoutedEventArgs e)
        {
            /*
            ViewEtiquetaCheckListEmitida view = new();
            DocumentContainer.SetHeader(view, "ETIQUETA CHECKLIST EMITIDAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewEtiquetaCheckListEmitida(), "ETIQUETA CHECKLIST EMITIDAS", "ETIQUETA_CHECKLIST_EMITIDAS");
        }

        private void OnCentralCriarModelo(object sender, RoutedEventArgs e)
        {
            /*
            ViewCentralCriarModelo view = new();
            DocumentContainer.SetHeader(view, "CRIAR MODELO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCentralCriarModelo(), "CRIAR MODELO", "CRIAR_MODELO");
        }

        private void OnCentralTabelaPa(object sender, RoutedEventArgs e)
        {
            /*
            ViewCentralTabelaPA view = new();
            DocumentContainer.SetHeader(view, "TABELA ARVORE P.A");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCentralTabelaPA(), "TABELA ARVORE P.A", "TABELA_ARVORE_PA");
        }

        private void OnCentralFatorConversao(object sender, RoutedEventArgs e)
        {
            /*
            ViewCentralFatorConversao view = new();
            DocumentContainer.SetHeader(view, "TABELA FATOR CONVERSÃO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCentralFatorConversao(), "TABELA FATOR CONVERSÃO", "TABELA_FATOR_CONVERSAO");
        }

        private void OnCentralEmitirOs(object sender, RoutedEventArgs e)
        {
            /*
            ViewCentralEmitirOs view = new();
            DocumentContainer.SetHeader(view, "CONTROLE ORDEM DE SERVIÇO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1024.0, 800.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCentralEmitirOs(), "CONTROLE ORDEM DE SERVIÇO", "CONTROLE_ORDEM_SERVICO");
        }

        private void OnCentralStatusCheckList(object sender, RoutedEventArgs e)
        {
            /*
            ViewCentralStatusCheckList view = new();
            DocumentContainer.SetHeader(view, "STATUS CHECK-LIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCentralStatusCheckList(), "STATUS CHECK-LIST", "STATUS_CHECK_LIST");
        }


        private void OnCreateReceitaRequisicao(object sender, RoutedEventArgs e)
        {
            /*
            ViewReceitaRequisicao view = new();
            DocumentContainer.SetHeader(view, "RECEITA REQUISIÇÃO MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewReceitaRequisicao(), "RECEITA REQUISIÇÃO MATERIAL", "RECEITA_REQUISICAO_MATERIAL");
        }

        private void OnCadastroProduto(object sender, RoutedEventArgs e)
        {
            /*
            ViewCadastroProduto view = new();
            DocumentContainer.SetHeader(view, "CADASTRO DE PRODUTOS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewCadastroProduto(), "CADASTRO DE PRODUTOS", "CADASTRO_PRODUTOS");
        }

        private void OnCadastroEspanhol(object sender, RoutedEventArgs e)
        {
            /*
            CadastroDescricaoEspanhol view = new();
            DocumentContainer.SetHeader(view, "CADASTRO DE DESCRIÇÃO ESPANHOL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new CadastroDescricaoEspanhol(), "CADASTRO DE DESCRIÇÃO ESPANHOL", "CADASTRO_DESCRICAO_ESPANHOL");
        }

        private void OnTodasDescricoes(object sender, RoutedEventArgs e)
        {
            /*
            TodasDescricoes view = new();
            DocumentContainer.SetHeader(view, "TODAS DESCRIÇÕES CADASTRADAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new TodasDescricoes(), "TODAS DESCRIÇÕES CADASTRADAS", "TODAS_DESCRICOES_CADASTRADAS");
        }

        private void OnEmitirOSServicoClick(object sender, RoutedEventArgs e)
        {
            /*
            EmissaoServico view = new();
            DocumentContainer.SetHeader(view, "EMISSÃO DE O.S. DE SERVIÇO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 800) / 2.0, (this._mdi.ActualHeight - 400) / 2.0, 800.0, 400));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new EmissaoServico(), "EMISSÃO DE O.S. DE SERVIÇO", "EMISSAO_OS_SERVICO");
        }

        private void OnEmitidasOSServicoClick(object sender, RoutedEventArgs e)
        {
            /*
            EmissaoServicoEmitidas view = new();
            DocumentContainer.SetHeader(view, "O.S. DE SERVIÇO EMITIDAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new EmissaoServicoEmitidas(), "O.S. DE SERVIÇO EMITIDAS", "OS_SERVICO_EMITIDAS");
        }

        private void OnAlterarRequisicoes(object sender, RoutedEventArgs e)
        {
            /*
            RequisicaoMaterialAlterar view = new();
            DocumentContainer.SetHeader(view, "ALTERAR REQUISIÇÕES DE MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */

            adicionarFilho(new RequisicaoMaterialAlterar(), "ALTERAR REQUISIÇÕES DE MATERIAL", "ALTERAR_REQUISICOES_MATERIAL");
        }

        private async void OnRequisicoesEmitidas(object sender, RoutedEventArgs e)
        {
            /*
            RequisicaoMaterialEmitidas view = new();
            DocumentContainer.SetHeader(view, "REQUISIÇÕES DE MATERIAIS EMITIDAS");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            //adicionarFilho(new RequisicaoMaterialEmitidas(), "REQUISIÇÕES DE MATERIAIS EMITIDAS", "REQUISICOES_MATERIAIS_EMITIDAS");

            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                using DatabaseContext db = new();
                //var data = await db.PendenciaProducaos.ToListAsync();
                var data = await db.RequisicoesProducao.ToListAsync();

                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                //worksheet.IsGridLinesVisible = false;
                worksheet.ImportData(data, 1, 1, true);

                workbook.SaveAs("Impressos/REQUISICOES_MATERIAIS_EMITIDAS.xlsx");
                Process.Start(new ProcessStartInfo("Impressos\\REQUISICOES_MATERIAIS_EMITIDAS.xlsx")
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

        private void OnRequisicaClick(object sender, RoutedEventArgs e)
        {
            /*
            RequisicaoMaterialEmitir view = new();
            DocumentContainer.SetHeader(view, "EMITIR REQUISIÇÕES DE MATERIAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 250) / 2.0, (this._mdi.ActualHeight - 250.5) / 2.0, 250, 250.5));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new RequisicaoMaterialEmitir(), "EMITIR REQUISIÇÕES DE MATERIAL", "EMITIR_REQUISICOES_MATERIAL");
        }

        private void OnSolicitarOSProdutoClick(object sender, RoutedEventArgs e)
        {
            /*
            SolicitacaoOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "SOLICITAR ORDEM DE SERVIÇO DE PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new SolicitacaoOrdemServicoProduto(), "SOLICITAR ORDEM DE SERVIÇO DE PRODUTO", "SOLICITAR_ORDEM_SERVICO_PRODUTO");
        }

        private void OnEmitirOSProdutoClick(object sender, RoutedEventArgs e)
        {
            /*
            EmitirOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "EMITIR ORDEM DE SERVIÇO DE PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 768) / 2.0, 1000, 768));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new EmitirOrdemServicoProduto(), "EMITIR ORDEM DE SERVIÇO DE PRODUTO", "EMITIR_ORDEM_SERVICO_PRODUTO");
        }

        private void OnAlterarSolicitacaoOSProdutoClick(object sender, RoutedEventArgs e)
        {
            /*
            AlterarSolicitacaoOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "ALTERAR SOLICITAR ORDEM DE SERVIÇO DE PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000) / 2.0, (this._mdi.ActualHeight - 600) / 2.0, 1000, 600));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new AlterarSolicitacaoOrdemServicoProduto(), "ALTERAR SOLICITAR ORDEM DE SERVIÇO DE PRODUTO", "ALTERAR_SOLICITAR_ORDEM_SERVICO_PRODUTO");
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
            /*
            ControleGrupo view = new();
            DocumentContainer.SetHeader(view, "CONTROLE POR GRUPO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1000.0, 800.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ControleGrupo(), "CONTROLE POR GRUPO", "CONTROLE_POR_GRUPO");
        }

        private void OnEntradaEstoqueClick(object sender, RoutedEventArgs e)
        {
            /*
            MovimentacaoEntrada view = new();
            DocumentContainer.SetHeader(view, "MOVIMENTAÇÃO ENTRADA ESTOQUE");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new MovimentacaoEntrada(), "MOVIMENTAÇÃO ENTRADA ESTOQUE", "MOVIMENTACAO_ENTRADA_ESTOQUE");
        }

        private void OnSaidaEstoqueClick(object sender, RoutedEventArgs e)
        {
            /*
            MovimentacaoSaida view = new();
            DocumentContainer.SetHeader(view, "MOVIMENTAÇÃO SAÍDA ESTOQUE");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new MovimentacaoSaida(), "MOVIMENTAÇÃO SAÍDA ESTOQUE", "MOVIMENTACAO_SAIDA_ESTOQUE");
        }

        private void OnBaixaRequisicaoClick(object sender, RoutedEventArgs e)
        {
            /*
            BaixaRequisicao view = new();
            DocumentContainer.SetHeader(view, "BAIXA DE REQUISIÇÃO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new BaixaRequisicao(), "BAIXA DE REQUISIÇÃO", "BAIXA_REQUISICAO");
        }

        private void OnSaldoEstoqueClick(object sender, RoutedEventArgs e)
        {
            /*
            SaldoEstoque view = new();
            DocumentContainer.SetHeader(view, "SALDO DE ESTOQUE");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 700) / 2.0, (this._mdi.ActualHeight - 250) / 2.0, 700, 250));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new SaldoEstoque(), "SALDO DE ESTOQUE", "SALDO_ESTOQUE");
        }

        private void OnRelatorioCCEClick(object sender, RoutedEventArgs e)
        {
            /*
            RelatorioCCE view = new();
            DocumentContainer.SetHeader(view, "RELATÓRIO C.C.E");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 400) / 2.0, (this._mdi.ActualHeight - 70) / 2.0, 400, 70));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new RelatorioCCE(), "RELATÓRIO C.C.E", "RELATORIO_CCE");
        }

        private void OnDigitarCCEClick(object sender, RoutedEventArgs e)
        {
            /*
            DigitacaoCCE view = new();
            DocumentContainer.SetHeader(view, "CONTROLE ESTOQUE PROCESSADO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1024.00) / 2.0, (this._mdi.ActualHeight - 768.00) / 2.0, 1024.00, 768.00));
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new DigitacaoCCE(), "CONTROLE ESTOQUE PROCESSADO", "CONTROLE_ESTOQUE_PROCESSADO");
        }

        private void CompletarChecklistClick(object sender, RoutedEventArgs e)
        {
            /*
            ViewComplementoCheckListNatal view = new();
            DocumentContainer.SetHeader(view, "COMPLETAR CHECKLIST NATAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ViewComplementoCheckListNatal(), "COMPLETAR CHECKLIST NATAL", "COMPLETAR_CHECKLIST_NATAL");
        }

        private async void OnConsultaCCEClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                using DatabaseContext db = new();
                var data = await db.DetalhesProcessamentoSemanas.ToListAsync();

                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Xlsx;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                //worksheet.IsGridLinesVisible = false;
                worksheet.ImportData(data, 1, 1, true);

                workbook.SaveAs("Impressos/CCE.xlsx");
                Process.Start(new ProcessStartInfo("Impressos\\CCE.xlsx")
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

        private void OnBaixaServisoClick(object sender, RoutedEventArgs e)
        {
            /*
            ViewComplementoCheckListNatal view = new();
            DocumentContainer.SetHeader(view, "COMPLETAR CHECKLIST NATAL");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            //adicionarFilho(new ViewComplementoCheckListNatal(), "COMPLETAR CHECKLIST NATAL", "COMPLETAR_CHECKLIST_NATAL");
        }

        private void OnBaixaProdutoClick(object sender, RoutedEventArgs e)
        {
            /*
            BaixaOrdemServicoProduto view = new();
            DocumentContainer.SetHeader(view, "BAIXA ORDEM DE SERVIÇO PRODUTO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new BaixaOrdemServicoProduto(), "BAIXA ORDEM DE SERVIÇO PRODUTO", "BAIXA_ORDEM_SERVICO_PRODUTO");
        }

        private async void OnEmitidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    using DatabaseContext db = new();
                    var data = await db.OrdemServicoEmitidas.ToListAsync();

                    using ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;

                    application.DefaultVersion = ExcelVersion.Xlsx;

                    //Create a workbook
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    //worksheet.IsGridLinesVisible = false;
                    worksheet.ImportData(data, 1, 1, true);

                    workbook.SaveAs("Impressos/EMITIDAS.xlsx");
                    Process.Start(new ProcessStartInfo("Impressos\\EMITIDAS.xlsx")
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnConcluidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    using DatabaseContext db = new();
                    var data = await db.OrdemServicoEmitidas.Where(os => os.concluida_os_data != null).ToListAsync();

                    using ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;

                    application.DefaultVersion = ExcelVersion.Xlsx;

                    //Create a workbook
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    //worksheet.IsGridLinesVisible = false;
                    worksheet.ImportData(data, 1, 1, true);

                    workbook.SaveAs("Impressos/EMITIDAS.xlsx");
                    Process.Start(new ProcessStartInfo("Impressos\\EMITIDAS.xlsx")
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnNaoConcluidasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    using DatabaseContext db = new();
                    var data = await db.OrdemServicoEmitidas.Where(os => os.concluida_os_data == null).ToListAsync();

                    using ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;

                    application.DefaultVersion = ExcelVersion.Xlsx;

                    //Create a workbook
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    //worksheet.IsGridLinesVisible = false;
                    worksheet.ImportData(data, 1, 1, true);

                    workbook.SaveAs("Impressos/EMITIDAS.xlsx");
                    Process.Start(new ProcessStartInfo("Impressos\\EMITIDAS.xlsx")
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnCanceladasClick(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                    using DatabaseContext db = new();
                    var data = await db.OrdemServicoEmitidas.Where(os => os.cancelada_os == "-1").ToListAsync();

                    using ExcelEngine excelEngine = new ExcelEngine();
                    IApplication application = excelEngine.Excel;

                    application.DefaultVersion = ExcelVersion.Xlsx;

                    //Create a workbook
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    //worksheet.IsGridLinesVisible = false;
                    worksheet.ImportData(data, 1, 1, true);

                    workbook.SaveAs("Impressos/EMITIDAS.xlsx");
                    Process.Start(new ProcessStartInfo("Impressos\\EMITIDAS.xlsx")
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnProgramacaoProducaoClick(object sender, RoutedEventArgs e)
        {
            /*
            ProgramacaoProducao view = new();
            DocumentContainer.SetHeader(view, "PROGRAMAÇÃO DE PRODUÇÃO");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1000.0) / 2.0, (this._mdi.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = true;
            this._mdi.Items.Add(view);
            */
            adicionarFilho(new ProgramacaoProducao(), "PROGRAMAÇÃO DE PRODUÇÃO", "PROGRAMACAO_PRODUCAO");
        }

        private void _mdi_CloseButtonClick(object sender, CloseButtonEventArgs e)
        {
            var tab = (DocumentContainer)sender;
            _mdi.Items.Remove(tab.ActiveDocument);
        }

        private void _mdi_CloseAllTabs(object sender, CloseTabEventArgs e)
        {
            _mdi.Items.Clear();
        }

        private void OnImprimirEtiquetaControlado(object sender, RoutedEventArgs e)
        {
            adicionarFilho(new ImprimirEtiqueta(), "IMPRESSÃO ETIQUETA CONTROLADO", "IMPRESSAO_ETIQUETA_CONTROLADO");
        }
    }
}
