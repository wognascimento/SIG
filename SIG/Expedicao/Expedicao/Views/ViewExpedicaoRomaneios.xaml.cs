using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Expedicao.Views
{
    /// <summary>
    /// Interação lógica para ViewExpedicaoRomaneios.xam
    /// </summary>
    public partial class ViewExpedicaoRomaneios : UserControl
    {
        public List<RomaneioModel> Romaneios = new List<RomaneioModel>();

        public string LocalAberto { get; set; }
        public RomaneioModel Romaneio { get; set; }

        public ViewExpedicaoRomaneios(string localAberto)
        {
            InitializeComponent();
            LocalAberto = localAberto;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                itens.ItemsSource = await Task.Run(async () => await new RomaneioViewModel().GetRomaneiosAsync());
                if (LocalAberto == "PRINCIPAL")
                {
                    BSelecionados.Visibility = Visibility.Hidden;
                    loadingDetalhes.Visibility = Visibility.Hidden;
                    itens.SelectionMode = GridSelectionMode.Single;
                }
                else
                {
                    loadingDetalhes.Visibility = Visibility.Hidden;
                    BSelecionados.Visibility = Visibility.Visible;
                    itens.SelectionMode = GridSelectionMode.Multiple;
                    acao.Width = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (RomaneioModel selectedItem in itens.SelectedItems.Cast<RomaneioModel>())
            {
                Romaneios.Add(selectedItem);
            }

            Window.GetWindow(sender as DependencyObject).DialogResult = new bool?(true);
        }

        private void btnAcao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RomaneioModel dataContext = (e.Source as Button).DataContext as RomaneioModel;
                if (this.LocalAberto == "PRINCIPAL")
                {
                    Window window = new Window();
                    window.Title = "EXPEDIÇÃO ROMANEIO " + dataContext.CodRomaneiro.ToString();
                    window.Content = new ViewExpedicaoRomaneio(dataContext);
                    window.SizeToContent = SizeToContent.WidthAndHeight;
                    window.ResizeMode = ResizeMode.NoResize;
                    window.ShowDialog();
                }
                else
                {
                    if (!(this.LocalAberto == "CARREGAMENTO"))
                        return;
                    foreach (RomaneioModel selectedItem in (Collection<object>)this.itens.SelectedItems)
                        this.Romaneios.Add(selectedItem);
                    Window.GetWindow(sender as DependencyObject).DialogResult = new bool?(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
