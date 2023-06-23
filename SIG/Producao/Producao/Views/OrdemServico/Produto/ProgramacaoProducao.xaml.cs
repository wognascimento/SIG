using Microsoft.EntityFrameworkCore;
using Producao.DataBase.Model;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.Windows.Tools.Controls;
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

namespace Producao.Views.OrdemServico.Produto
{
    /// <summary>
    /// Interação lógica para ProgramacaoProducao.xam
    /// </summary>
    public partial class ProgramacaoProducao : UserControl
    {
        public ProgramacaoProducao()
        {
            InitializeComponent();
            DataContext = new ProgramacaoProducaoViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ProgramacaoProducaoViewModel vm = (ProgramacaoProducaoViewModel)DataContext;
                vm.Locais = await Task.Run(vm.GetLocalicacoesAsync);
                vm.Programacoes = await Task.Run(vm.GetProgramacaoItensAsync);

                txtFila.Text = vm.Programacoes.Where(p => p.programacao_status == "FILA/M.O").Count().ToString(); //DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'FILA/M.O'")
                txtDiretoria.Text = vm.Programacoes.Where(p => p.programacao_status == "ESPAÇO FÍSICO").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'ESPAÇO FÍSICO'")
                txtProjeto.Text = vm.Programacoes.Where(p => p.programacao_status == "EMBALAGEM/EXPEDIÇÃO").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'EMBALAGEM/EXPEDIÇÃO'")
                txtAndamento.Text = vm.Programacoes.Where(p => p.programacao_status == "EM ANDAMENTO").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'EM ANDAMENTO'")
                txtIndefinido.Text = vm.Programacoes.Where(p => p.programacao_status == "INDEFINIDO").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'INDEFINIDO'")
                txtProjetos.Text = vm.Programacoes.Where(p => p.programacao_status == "PROJETOS").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'PROJETOS'")
                txtFaltaMaterialTranf.Text = vm.Programacoes.Where(p => p.programacao_status == "FALTA MATERIAL INTERNO / TRANSF").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'FALTA MATERIAL INTERNO / TRANSF'")
                txtFaltaMaterialCompras.Text = vm.Programacoes.Where(p => p.programacao_status == "FALTA MATERIAL EXTERNO / COMPRAS").Count().ToString();//DCount("[num_os]", "qry_programacao_producao_global_producao", "[programacao_status] = 'FALTA MATERIAL EXTERNO / COMPRAS'")


                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private async void LocaisSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBoxAdv)sender; //= { Syncfusion.Windows.Tools.Controls.ComboBoxAdv Items.Count: 5}
            var local = (SetorProducaoModel)comboBox.SelectedItem;
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });
                ProgramacaoProducaoViewModel vm = (ProgramacaoProducaoViewModel)DataContext;
                vm.Setores = await Task.Run(() => vm.GetSetorsAsync(local.localizacao));
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private void SetoresSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SelectedItems = Count = 2
            var comboBox = (ComboBoxAdv)sender; //= { Syncfusion.Windows.Tools.Controls.ComboBoxAdv Items.Count: 5}
            var setores = (List<SetorModel>)comboBox.SelectedItem; 
        }

        private void OnAddNewRowInitiating(object sender, Syncfusion.UI.Xaml.Grid.AddNewRowInitiatingEventArgs e)
        {

        }

        private void OnCurrentCellDropDownSelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellDropDownSelectionChangedEventArgs e)
        {

        }

        private void OnRowValidated(object sender, Syncfusion.UI.Xaml.Grid.RowValidatedEventArgs e)
        {

        }

        private void OnRowValidating(object sender, Syncfusion.UI.Xaml.Grid.RowValidatingEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //((MainWindow)Application.Current.MainWindow)._mdi.Items.Remove(this);
        }
    }

    class ProgramacaoProducaoViewModel : INotifyPropertyChanged
    {
        private SetorModel _setor;
        public SetorModel Setor
        {
            get { return _setor; }
            set { _setor = value; RaisePropertyChanged("Setor"); }
        }
        private ObservableCollection<SetorModel> _setores;
        public ObservableCollection<SetorModel> Setores
        {
            get { return _setores; }
            set { _setores = value; RaisePropertyChanged("Setores"); }
        }

        private SetorProducaoModel _local;
        public SetorProducaoModel Local
        {
            get { return _local; }
            set { _local = value; RaisePropertyChanged("Local"); }
        }
        private ObservableCollection<SetorProducaoModel> _locais;
        public ObservableCollection<SetorProducaoModel> Locais
        {
            get { return _locais; }
            set { _locais = value; RaisePropertyChanged("Locais"); }
        }

        private ProgramacaoProducaoModel _programacao;
        public ProgramacaoProducaoModel Programacao
        {
            get { return _programacao; }
            set { _programacao = value; RaisePropertyChanged("Programacao"); }
        }
        private ObservableCollection<ProgramacaoProducaoModel> _programacoes;
        public ObservableCollection<ProgramacaoProducaoModel> Programacoes
        {
            get { return _programacoes; }
            set { _programacoes = value; RaisePropertyChanged("Programacoes"); }
        }

        public async Task<ObservableCollection<SetorProducaoModel>> GetLocalicacoesAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.SetorProducaos
                    .GroupBy(p => p.localizacao)
                    .Select(g => g.OrderBy(p => p.localizacao).FirstOrDefault())
                    .ToListAsync();

                return new ObservableCollection<SetorProducaoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<SetorModel>> GetSetorsAsync(string localizacao)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await (from s in db.SetorProducaos orderby s.setor where s.inativo == "0    " && s.localizacao == localizacao select new SetorModel { setor = s.setor + " - " + s.galpao, codigo_setor = s.codigo_setor }).ToListAsync();
                return new ObservableCollection<SetorModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<ProgramacaoProducaoModel>> GetProgramacaoItensAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ProgramacaoProducoes
                    .ToListAsync();

                return new ObservableCollection<ProgramacaoProducaoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
