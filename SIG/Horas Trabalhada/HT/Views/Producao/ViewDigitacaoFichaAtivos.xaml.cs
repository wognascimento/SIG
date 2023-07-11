using HT.DataBase;
using HT.DataBase.Model;
using Microsoft.EntityFrameworkCore;
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
                vm.Apontamentos = await Task.Run(() => vm.GetApontamentosAsync(DateTime.Now));
                vm.Funcionarios = await Task.Run(vm.GetFuncionariosAsync);
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
    }

    public class DigitacaoFichaAtivosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName) { this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName)); }

        private ObservableCollection<ApontamentoModel> _apontamentos;
        public ObservableCollection<ApontamentoModel> Apontamentos
        {
            get { return _apontamentos; }
            set { _apontamentos = value; RaisePropertyChanged("Apontamentos"); }
        }
        private ApontamentoModel _apontamento;
        public ApontamentoModel Apontamento
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

        public async Task<ObservableCollection<ApontamentoModel>> GetApontamentosAsync(DateTime date)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Apontamentos.Where(d => d.data.Date == date.Date).ToListAsync();
                return new ObservableCollection<ApontamentoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
