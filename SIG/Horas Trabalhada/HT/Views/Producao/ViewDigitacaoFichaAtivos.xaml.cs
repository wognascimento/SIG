using HT.DataBase;
using HT.DataBase.Model;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HT.Views.Producao
{
    /// <summary>
    /// Interação lógica para ViewDigitacaoFichaAtivos.xam
    /// </summary>
    public partial class ViewDigitacaoFichaAtivos : UserControl
    {
        public ViewDigitacaoFichaAtivos()
        {
            DataContext = new DigitacaoFichaAtivosViewModel();
            InitializeComponent();
        }

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            try
            {
                //var dateTime = dtApontamento.DateTime.Value; // new DateTime(2023, 07, 07, 12, 0, 0);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                DigitacaoFichaAtivosViewModel vm = (DigitacaoFichaAtivosViewModel)DataContext;
                var data = await Task.Run(() => vm.GetDataAsync(DateTime.Now));
                vm.Funcionarios = await Task.Run(vm.GetFuncionariosAsync);
                vm.Apontamentos = await Task.Run(() => vm.GetApontamentosAsync(DateTime.Now));
                txtSemana.Text = data.semana.ToString();
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void DateTimeEdit_DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Console.Out.WriteLine(e.NewValue);
            try
            {
                DateTime dateTime = (DateTime)e.NewValue; // new DateTime(2023, 07, 07, 12, 0, 0);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                DigitacaoFichaAtivosViewModel vm = (DigitacaoFichaAtivosViewModel)DataContext;
                var data = await Task.Run(() => vm.GetDataAsync(dateTime));
                vm.Apontamentos = await Task.Run(() => vm.GetApontamentosAsync(dateTime));
                txtSemana.Text = data.semana.ToString();
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
        }

        private async void OnChangedValueCell(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellValueChangedEventArgs e)
        {
            //QryCheckListGeralComplementoModel? dado = e.Record as QryCheckListGeralComplementoModel; //e.Record = Record = {HT.DataBase.Model.ViewApontamentoModel}
            //CheckListViewModel vm = (CheckListViewModel)DataContext;

            SfDataGrid? grid = sender as SfDataGrid;
            int columnindex = grid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var column = grid.Columns[columnindex];
            var rowIndex = grid.ResolveToRecordIndex(e.RowColumnIndex.RowIndex);
            //var record = grid.View.Records[rowIndex].Data as QryCheckListGeralComplementoModel;

            try
            {
                if (column.GetType() == typeof(GridCheckBoxColumn) && column.MappingName == "confirmado")
                {
                    /*
                    vm.DetCompl.coddetalhescompl = dado.coddetalhescompl;
                    vm.DetCompl.confirmado = dado.confirmado;
                    vm.DetCompl.confirmado_data = dado.confirmado == "-1" ? DateTime.Now : dado.confirmado_data;
                    vm.DetCompl.confirmado_por = dado.confirmado == "-1" ? Environment.UserName : dado.confirmado_por;
                    vm.DetCompl.desabilitado_confirmado_data = dado.confirmado == "0" ? DateTime.Now : dado.desabilitado_confirmado_data;
                    vm.DetCompl.desabilitado_confirmado_por = dado.confirmado == "0" ? Environment.UserName : dado.desabilitado_confirmado_por;
                    vm.DetCompl = await Task.Run(() => vm.ConfirmarComplementoCheckListAsync(vm.DetCompl));
                    */
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }

    public class DigitacaoFichaAtivosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName)); }

        private ObservableCollection<ViewApontamentoModel> _apontamentos;
        public ObservableCollection<ViewApontamentoModel> Apontamentos
        {
            get { return _apontamentos; }
            set { _apontamentos = value; RaisePropertyChanged("Apontamentos"); }
        }
        private ViewApontamentoModel _apontamento;
        public ViewApontamentoModel Apontamento
        {
            get { return _apontamento; }
            set { _apontamento = value; RaisePropertyChanged("Apontamento"); }
        }
        private ObservableCollection<FuncionarioAtivoModel> _funcionarios;
        public ObservableCollection<FuncionarioAtivoModel> Funcionarios
        {
            get { return _funcionarios; }
            set { _funcionarios = value; RaisePropertyChanged("Funcionarios"); }
        }
        private FuncionarioAtivoModel _funcionario;
        public FuncionarioAtivoModel Funcionario
        {
            get { return _funcionario; }
            set { _funcionario = value; RaisePropertyChanged("Funcionario"); }
        }

        public async Task<ObservableCollection<FuncionarioAtivoModel>> GetFuncionariosAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.FuncionarioAtivos.Where(d => d.ativo.Contains("1")).ToListAsync();
                return new ObservableCollection<FuncionarioAtivoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataPlanejamentoModel> GetDataAsync(DateTime date)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.DataPlanejamentos.Where(d => d.data.Date == date.Date).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<ViewApontamentoModel>> GetApontamentosAsync(DateTime date)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ViewApontamentos.Where(d => d.data.Date == date.Date).ToListAsync();
                return new ObservableCollection<ViewApontamentoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
