using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Compras.Views
{
    /// <summary>
    /// Interação lógica para ViewSolicitacaoEncaminhamento.xam
    /// </summary>
    public partial class ViewSolicitacaoEncaminhamento : UserControl
    {
        public ViewSolicitacaoEncaminhamento()
        {
            InitializeComponent();
            this.DataContext = new SolicitacaoEncaminhadaViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SolicitacaoEncaminhadaViewModel vm = (SolicitacaoEncaminhadaViewModel)DataContext;
            try
            {
                vm.SolicitacoesEncaminhadas = await Task.Run(vm.GetSolicitacaoEncaminhadasAsync);
                loading.IsBusy = false;
                loading.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnCreateFile(object sender, RoutedEventArgs e)
        {
            SolicitacaoEncaminhadaViewModel vm = (SolicitacaoEncaminhadaViewModel)DataContext;
            try
            {

                if(vm.ItensPedido == null)
                {
                    MessageBox.Show("Não tem produtos para criar o arquivo", "Criar Arquivo");
                    return;
                }
                using ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open("Modelos/PEDIDO-COMPRA.xlsx", ExcelParseOptions.Default, false, "1@3mudar");
                IWorksheet worksheet = workbook.Worksheets[0];

                vm.Pedido = await Task.Run(() => vm.CreatePedido(new PedidoModel { datapedido = DateTime.Now }));

                worksheet.Range[$"E4"].Text = vm.Pedido.idpedido.ToString();
                worksheet.Range[$"G4"].Text = DateTime.Parse(vm.Pedido.datapedido.ToString()).ToString("dd/MM/yyyy");

                for (int i = 0; i < vm.ItensPedido.Count; i++)
                {
                    var item = vm.ItensPedido.ToList()[i];
                    worksheet.Range[$"A{i + 12}"].Text = item.codcompleadicional.ToString();
                    worksheet.Range[$"B{i + 12}"].Text = item.descricao_completa;
                    worksheet.Range[$"F{i + 12}"].Text = item.quantidade.ToString();
                    worksheet.Range[$"J{i + 12}"].Text = item.itens;
                }

                workbook.SaveAs("PEDIDO-COMPRA.xlsx");

                Process.Start(new ProcessStartInfo("PEDIDO-COMPRA.xlsx")
                {
                    UseShellExecute = true
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnDbClick(object sender, MouseButtonEventArgs e)
        {
            SolicitacaoEncaminhadaViewModel vm = (SolicitacaoEncaminhadaViewModel)DataContext;

            var dados = (from t in vm.ItensMontarPedido where t.cod_item == vm.SolicitacaoEncaminhada.cod_item select t).ToList();
            if (dados.Count > 0)
            {
                MessageBox.Show("Item já presente no pedido", "Adicionar Item", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            vm.ItensMontarPedido.Add(
                new ItemPedidoFileModel
                {
                    cod_item = vm.SolicitacaoEncaminhada.cod_item,
                    codcompleadicional = vm.SolicitacaoEncaminhada.codcompleadicional,
                    planilha = vm.SolicitacaoEncaminhada.planilha,
                    descricao_completa = vm.SolicitacaoEncaminhada.descricao_completa,
                    unidade = vm.SolicitacaoEncaminhada.unidade,
                    quantidade = vm.SolicitacaoEncaminhada.quantidade
                });



            vm.ItensPedido = (from t in vm.ItensMontarPedido
                         group t by new { t.codcompleadicional, t.planilha, t.descricao_completa, t.unidade }
                         into grp
                         select new ItemPedidoFileModel
                         {
                             codcompleadicional = grp.Key.codcompleadicional,
                             planilha = grp.Key.planilha,
                             descricao_completa = grp.Key.descricao_completa,
                             unidade = grp.Key.unidade,
                             quantidade = grp.Sum(t => t.quantidade),
                             itens = JsonConvert.SerializeObject((from i in vm.ItensMontarPedido where i.codcompleadicional == grp.Key.codcompleadicional select new { i.cod_item }).ToList()) 
                         }).ToList();


        }

        private void SfDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SolicitacaoEncaminhadaViewModel vm = (SolicitacaoEncaminhadaViewModel)DataContext;
            ItemPedidoFileModel row = (ItemPedidoFileModel)ItensPedido.SelectedItem;
            
            
            var confirmacao = MessageBox.Show("Deseja remover o produto do pedido?", "Remover Produto", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(confirmacao == MessageBoxResult.Yes)
            {
                //vm.ItensMontarPedido.Remove((from t in vm.ItensMontarPedido where t.codcompleadicional == row.codcompleadicional select t).FirstOrDefault());
                var itens = (from t in vm.ItensMontarPedido where t.codcompleadicional == row.codcompleadicional select t).ToList();
                foreach (var item in itens)
                    vm.ItensMontarPedido.Remove(item);

                vm.ItensPedido = (from t in vm.ItensMontarPedido
                                  group t by new { t.codcompleadicional, t.planilha, t.descricao_completa, t.unidade }
                         into grp
                                  select new ItemPedidoFileModel
                                  {
                                      codcompleadicional = grp.Key.codcompleadicional,
                                      planilha = grp.Key.planilha,
                                      descricao_completa = grp.Key.descricao_completa,
                                      unidade = grp.Key.unidade,
                                      quantidade = grp.Sum(t => t.quantidade),
                                      itens = JsonConvert.SerializeObject((from i in vm.ItensMontarPedido where i.codcompleadicional == grp.Key.codcompleadicional select new { i.cod_item }).ToList())
                                  }).ToList();
            }
        }
    }

    public class SolicitacaoEncaminhadaViewModel : INotifyPropertyChanged
    {
        public DataBaseSettings BaseSettings = DataBaseSettings.Instance;

        #region Solicitação Encaminhada
        private SolicitacaoEncaminhadaModel solicitacaoEncaminhada;
        public SolicitacaoEncaminhadaModel SolicitacaoEncaminhada
        {
            get { return solicitacaoEncaminhada; }
            set { solicitacaoEncaminhada = value; RaisePropertyChanged("SolicitacaoEncaminhada"); }
        }
        private ObservableCollection<SolicitacaoEncaminhadaModel> solicitacoesEncaminhadas;
        public ObservableCollection<SolicitacaoEncaminhadaModel> SolicitacoesEncaminhadas
        {
            get { return solicitacoesEncaminhadas; }
            set { solicitacoesEncaminhadas = value; RaisePropertyChanged("SolicitacoesEncaminhadas"); }
        }
        #endregion

        #region Solicitação Montar Pedido
        private ItemPedidoFileModel itemMontarPedido;
        public ItemPedidoFileModel ItemMontarPedido
        {
            get { return itemMontarPedido; }
            set { itemMontarPedido = value; RaisePropertyChanged("ItemMontarPedido"); }
        }
        private ObservableCollection<ItemPedidoFileModel> itensMontarPedido;
        public ObservableCollection<ItemPedidoFileModel> ItensMontarPedido
        {
            get { return itensMontarPedido; }
            set { itensMontarPedido = value; RaisePropertyChanged("ItensMontarPedido"); }
        }
        private ICollection<ItemPedidoFileModel> itensPedido;
        public ICollection<ItemPedidoFileModel> ItensPedido
        {
            get { return itensPedido; }
            set { itensPedido = value; RaisePropertyChanged("ItensPedido"); }
        }
        #endregion

        #region Pedido
        private PedidoModel pedido;
        public PedidoModel Pedido
        {
            get { return pedido; }
            set { pedido = value; RaisePropertyChanged("Pedido"); }
        }
        private ObservableCollection<PedidoModel> pedidos;
        public ObservableCollection<PedidoModel> Pedidos
        {
            get { return pedidos; }
            set { pedidos = value; RaisePropertyChanged("Pedidos"); }
        }
        #endregion

        public SolicitacaoEncaminhadaViewModel() 
        {
            this.ItensMontarPedido = new ObservableCollection<ItemPedidoFileModel>();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public async Task<ObservableCollection<SolicitacaoEncaminhadaModel>> GetSolicitacaoEncaminhadasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.SolicitacaoEncaminhadas.Where(e => e.tipo != "SERVIÇO").ToListAsync();
                return new ObservableCollection<SolicitacaoEncaminhadaModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PedidoModel> CreatePedido(PedidoModel pedido)
        {
            try
            {
                using DatabaseContext db = new();
                db.Pedidos.Add(pedido);
                await db.SaveChangesAsync();
                return pedido;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
