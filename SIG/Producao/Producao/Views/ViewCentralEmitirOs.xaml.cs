using Microsoft.EntityFrameworkCore;
using Producao.Views.PopUp;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Producao.Views
{
    /// <summary>
    /// Interação lógica para ViewCentralEmitirOs.xam
    /// </summary>
    public partial class ViewCentralEmitirOs : UserControl
    {
        public ViewCentralEmitirOs()
        {
            InitializeComponent();
            this.DataContext = new ViewCentralEmitirOsViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewCentralEmitirOsViewModel vm = (ViewCentralEmitirOsViewModel)DataContext;
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                vm.Itens = await Task.Run(vm.GetItensAsync);
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class ViewCentralEmitirOsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ModeloControleOsModel item;
        public ModeloControleOsModel Item
        {
            get { return item; }
            set { item = value; RaisePropertyChanged("Item"); }
        }
        private ObservableCollection<ModeloControleOsModel> itens;
        public ObservableCollection<ModeloControleOsModel> Itens
        {
            get { return itens; }
            set { itens = value; RaisePropertyChanged("Itens"); }
        }

        public async Task<ObservableCollection<ModeloControleOsModel>> GetItensAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.modeloControleOs.ToListAsync();
                return new ObservableCollection<ModeloControleOsModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

    public static class ContextMenuCommands
    {
        static BaseCommand? createOS;
        public static BaseCommand CreateOS
        {
            get
            {
                if (createOS == null)
                    createOS = new BaseCommand(OnCreateOSClicked);
                return createOS;
            }
        }
        private static void OnCreateOSClicked(object obj)
        {

            //var Record = { Producao.ModeloControleOsModel}
            //obj = {Syncfusion.UI.Xaml.Grid.GridRecordContextMenuInfo}
            var record = ((GridRecordContextMenuInfo)obj).Record as ModeloControleOsModel;
            var grid = ((GridRecordContextMenuInfo)obj).DataGrid;
            var item = grid.SelectedItem as ModeloControleOsModel;
            if (record?.qtd_chk_list > (int)(record?.qtd_os ?? 0))
            {
                try
                {
                    var dif = (record?.qtd_chk_list - (int)(record?.qtd_os ?? 0));
                    var window = new ModeloSetoresOrdemServico(record);
                    window.Owner = App.Current.MainWindow;
                    window.ShowDialog();
                    
                    /*Window window = new Window();
                    window.Content = new ModeloSetoresOrdemServico(747); //item?.codcompladicional
                    window.Owner = App.Current.MainWindow;
                    window.Title = "SETORES PARA EMISSÃO DA OERDEM DE SERVIÇO";
                    window.WindowStyle = WindowStyle.ToolWindow; //"ToolWindow"
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner; //"CenterOwner" 
                    window.ResizeMode = ResizeMode.NoResize; //"NoResize"
                    window.Height = 450;
                    window.Width = 500;
                    window.ShowDialog();*/


                }
                catch (Exception ex)
                {

                }
               
            }
            else
            {
                MessageBox.Show("Quantidade indisponivel para Gerar ordem de serviço.");
            }
        }


        static BaseCommand? reimprimirOS;
        public static BaseCommand ReimprimirOS
        {
            get
            {
                if (reimprimirOS == null)
                    reimprimirOS = new BaseCommand(OnReimprimirOSClicked);
                return reimprimirOS;
            }
        }
        private static void OnReimprimirOSClicked(object obj)
        {
            var grid = ((GridRecordContextMenuInfo)obj).DataGrid;
        }


        static BaseCommand? tabelaPAExcel;
        public static BaseCommand TabelaPAExcel
        {
            get
            {
                if (tabelaPAExcel == null)
                    tabelaPAExcel = new BaseCommand(OnTabelaPAExcelClicked);
                return tabelaPAExcel;
            }
        }
        private static void OnTabelaPAExcelClicked(object obj)
        {
            var grid = ((GridRecordContextMenuInfo)obj).DataGrid;
            var item = grid.SelectedItem as ModeloControleOsModel;
            if (item?.planilha != "KIT ENF PA")
            {
                MessageBox.Show("Produto não é uma P.A");
                return;
            }
        }

    }
}
