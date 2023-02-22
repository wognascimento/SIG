using System;
using System.Windows;
using Expedicao.Views;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Tools.Controls;
using SizeMode = Syncfusion.SfSkinManager.SizeMode;

namespace Expedicao
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DataBase dB = DataBase.Instance;

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

            dB.Database = DateTime.Now.Year.ToString();
            dB.Host = "192.168.0.23";
            dB.Username = Environment.UserName;
            dB.Password = "123mudar";
            txtUsername.Text = dB.Username;
            txtDataBase.Text = dB.Database;
        }
		/// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentVisualStyle = "Metro"; //"FluentLight";
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

        private void expedProduto_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoProduto expedicaoProduto = new();
            DocumentContainer.SetHeader((DependencyObject)expedicaoProduto, (object)"EXPEDIÇÃO PRODUTO SHOPPING");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)expedicaoProduto, true);
            DocumentContainer.SetMDIBounds((DependencyObject)expedicaoProduto, new Rect((this._dc.ActualWidth - 1000.0) / 2.0, (this._dc.ActualHeight - 700.0) / 2.0, 1000.0, 700.0));
            DocumentContainer.SetMDIWindowState((DependencyObject)expedicaoProduto, MDIWindowState.Maximized);
            this._dc.CanMDIMaximize = true;
            this._dc.Items.Add((object)expedicaoProduto);
        }

        private void expedImprimirEtiqueta_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoImpressaoEtiqueta impressaoEtiqueta = new();
            DocumentContainer.SetHeader((DependencyObject)impressaoEtiqueta, (object)"EXPEDIÇÃO IMPRESSÃO DE ETIQUETA");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)impressaoEtiqueta, true);
            DocumentContainer.SetMDIBounds((DependencyObject)impressaoEtiqueta, new Rect((this._dc.ActualWidth - 900.0) / 2.0, (this._dc.ActualHeight - 600.0) / 2.0, 900.0, 600.0));
            DocumentContainer.SetMDIWindowState((DependencyObject)impressaoEtiqueta, MDIWindowState.Maximized);
            this._dc.CanMDIMaximize = true;
            this._dc.Items.Add((object)impressaoEtiqueta);
        }

        private void liberarImpressao_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoLiberarImpressao liberarImpressao = new ViewExpedicaoLiberarImpressao();
            DocumentContainer.SetHeader((DependencyObject)liberarImpressao, (object)"EXPEDIÇÃO LIBERAR IMPRESSÃO DE ETIQUETA");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)liberarImpressao, true);
            DocumentContainer.SetMDIBounds((DependencyObject)liberarImpressao, new Rect((this._dc.ActualWidth - 900.0) / 2.0, (this._dc.ActualHeight - 600.0) / 2.0, 900.0, 600.0));
            DocumentContainer.SetMDIWindowState((DependencyObject)liberarImpressao, MDIWindowState.Maximized);
            this._dc.CanMDIMaximize = true;
            this._dc.Items.Add((object)liberarImpressao);
        }

        private void expedNovoRomaneio_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoRomaneio expedicaoRomaneio = new ViewExpedicaoRomaneio();
            DocumentContainer.SetHeader((DependencyObject)expedicaoRomaneio, (object)"EXPEDIÇÃO ROMANEIO");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)expedicaoRomaneio, true);
            DocumentContainer.SetMDIWindowState((DependencyObject)expedicaoRomaneio, MDIWindowState.Normal);
            DocumentContainer.SetMDIBounds((DependencyObject)expedicaoRomaneio, new Rect((this._dc.ActualWidth - 900.0) / 2.0, (this._dc.ActualHeight - 780.0) / 2.0, 900.0, 780.0));
            DocumentContainer.SetMDIWindowState((DependencyObject)expedicaoRomaneio, MDIWindowState.Normal);
            this._dc.CanMDIMaximize = false;
            this._dc.Items.Add((object)expedicaoRomaneio);
        }

        private void expedTodosRomaneios_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoRomaneios expedicaoRomaneios = new ViewExpedicaoRomaneios("PRINCIPAL");
            DocumentContainer.SetHeader((DependencyObject)expedicaoRomaneios, (object)"EXPEDIÇÃO ROMANEIOS");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)expedicaoRomaneios, true);
            DocumentContainer.SetMDIBounds((DependencyObject)expedicaoRomaneios, new Rect((this._dc.ActualWidth - 800.0) / 2.0, (this._dc.ActualHeight - 600.0) / 2.0, 800.0, 600.0));
            DocumentContainer.SetMDIWindowState((DependencyObject)expedicaoRomaneios, MDIWindowState.Normal);
            this._dc.CanMDIMaximize = false;
            this._dc.Items.Add((object)expedicaoRomaneios);
        }

        private void expedColetarDados_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoColetaDados expedicaoColetaDados = new ViewExpedicaoColetaDados();
            DocumentContainer.SetHeader((DependencyObject)expedicaoColetaDados, (object)"EXPEDIÇÃO COLETA DE DADOS");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)expedicaoColetaDados, true);
            DocumentContainer.SetMDIBounds((DependencyObject)expedicaoColetaDados, new Rect((this._dc.ActualWidth - 800.0) / 2.0, (this._dc.ActualHeight - 600.0) / 2.0, 800.0, 600.0));
            DocumentContainer.SetMDIWindowState((DependencyObject)expedicaoColetaDados, MDIWindowState.Maximized);
            this._dc.CanMDIMaximize = true;
            this._dc.Items.Add((object)expedicaoColetaDados);
        }

        private void ItensFaltantes_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoExcel viewExpedicaoExcel = new ViewExpedicaoExcel("ITENS_FALTANTES");
            DocumentContainer.SetHeader((DependencyObject)viewExpedicaoExcel, (object)"EXPEDIÇÃO ITENS FALTANTES");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)viewExpedicaoExcel, true);
            DocumentContainer.SetMDIWindowState((DependencyObject)viewExpedicaoExcel, MDIWindowState.Normal);
            DocumentContainer.SetMDIBounds((DependencyObject)viewExpedicaoExcel, new Rect((this._dc.ActualWidth - 600.0) / 2.0, (this._dc.ActualHeight - 80.0) / 2.0, 600.0, 80.0));
            this._dc.CanMDIMaximize = false;
            this._dc.Items.Add((object)viewExpedicaoExcel);
        }

        private void ItensCarregados_Click(object sender, RoutedEventArgs e)
        {
            ViewExpedicaoExcel viewExpedicaoExcel = new ViewExpedicaoExcel("ITENS_CARREGADOS");
            DocumentContainer.SetHeader((DependencyObject)viewExpedicaoExcel, (object)"EXPEDIÇÃO ITENS CARREGADOS");
            DocumentContainer.SetSizetoContentInMDI((DependencyObject)viewExpedicaoExcel, true);
            DocumentContainer.SetMDIWindowState((DependencyObject)viewExpedicaoExcel, MDIWindowState.Normal);
            DocumentContainer.SetMDIBounds((DependencyObject)viewExpedicaoExcel, new Rect((this._dc.ActualWidth - 600.0) / 2.0, (this._dc.ActualHeight - 80.0) / 2.0, 600.0, 80.0));
            this._dc.CanMDIMaximize = false;
            this._dc.Items.Add((object)viewExpedicaoExcel);
        }
    }
}
