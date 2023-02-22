using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Z.EntityFramework.Extensions.BatchUpdate;

namespace Producao.Views
{
    /// <summary>
    /// Interação lógica para ViewAprovado.xam
    /// </summary>
    public partial class ViewAprovado : UserControl
    {
        public ObservableCollection<AprovadoModel> AprovadosList{ get; set; }

        public ViewAprovado()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Visible;
                itens.ItemsSource = await Task.Run(async () => await new AprovadoViewModel().GetAprovados());
                //AprovadosList = await Task.Run(async () => await new AprovadoViewModel().GetAprovados());
                ((MainWindow)Application.Current.MainWindow).PbLoading.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnCurrentCellEndEdit(object sender, CurrentCellEndEditEventArgs e)
        {
            
        }

        private async void OnCurrentCellValueChanged(object sender, CurrentCellValueChangedEventArgs e)
        {
            /*
            SfDataGrid grid = sender as SfDataGrid;
            int columnindex = grid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var column = grid.Columns[columnindex];
            if (column.GetType() == typeof(GridCheckBoxColumn) && column.MappingName == "IsDelivered")
            {
                var rowIndex = grid.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
                var record = grid.View.Records[rowIndex].Data as OrderInfo;

                var value = record.IsDelivered;
            }
            */

            var dataGrid = sender as SfDataGrid;

            var provider = dataGrid.View.GetPropertyAccessProvider();

            //here get the information of the changed row by using RowIndex
            AprovadoModel record = dataGrid.GetRecordAtRowIndex(e.RowColumnIndex.RowIndex) as AprovadoModel;

            //if (record == null)
            //    return;

            //here get the column by using column Index
            var column = dataGrid.Columns[dataGrid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex)];

            //here get the changed cell value
            //var cellValue = provider.GetValue(record, column.MappingName);

            //MessageBox.Show("Changed data in a row : " + cellValue);

            var dataRow = this.itens.RowGenerator.Items.FirstOrDefault(item => item.RowIndex == e.RowColumnIndex.RowIndex);
            

            if (column.MappingName.Equals("OkPlantaPca"))
            {
                record.PlantaPca = Environment.UserName;
                record.LiberacaoPlantaPca = DateTimeOffset.Now;

                (dataRow.RowData as AprovadoModel).PlantaPca = Environment.UserName;
                (dataRow.RowData as AprovadoModel).LiberacaoPlantaPca = DateTimeOffset.Now;
            }
            else if (column.MappingName.Equals("OkPlantaBase"))
            {
                record.PlantaBase = Environment.UserName;
                record.LiberacaoPlantaBase = DateTimeOffset.Now;
            }
            else if (column.MappingName.Equals("OkPlantaMall"))
            {
                record.PlantaMall = Environment.UserName;
                record.ConclusaoPlantaMall = DateTimeOffset.Now;
            }
            else if (column.MappingName.Equals("OkPlantaFachada"))
            {
                record.PlantaFachada = Environment.UserName;
                record.ConclusaoPlantaFachada = DateTimeOffset.Now;
            }

            try
            {
                //await new AprovadoViewModel().SaveAsync(record);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            //MessageBox.Show("Changed data in a row : ");

        }
    }
}
