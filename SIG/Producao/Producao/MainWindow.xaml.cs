using System;
using System.Windows;
using Producao.Views;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;
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
            ViewCheckList view = new();
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
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1500.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1500.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }

        private void OnCentralStatusCheckList(object sender, RoutedEventArgs e)
        {
            ViewCentralStatusCheckList view = new();
            DocumentContainer.SetHeader(view, "STATUS CHECK-LIST");
            DocumentContainer.SetSizetoContentInMDI(view, true);
            DocumentContainer.SetMDIBounds(view, new Rect((this._mdi.ActualWidth - 1500.0) / 2.0, (this._mdi.ActualHeight - 800.0) / 2.0, 1500.0, 800.0));
            //DocumentContainer.SetMDIWindowState(view, MDIWindowState.Maximized);
            this._mdi.CanMDIMaximize = false;
            this._mdi.Items.Add(view);
        }
    }
}
