using Expedicao.DataBaseLocal;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Interação lógica para ViewExpedicaoRomaneio.xam
    /// </summary>
    public partial class ViewExpedicaoRomaneio : UserControl
    {

        private List<string> operacoes = new List<string>()
        {
          "CARREGAMENTO SHOPPING",
          "CARREGAMENTO TRANSFERÊNCIA",
          "DESCARREGAMENTO SHOPPING",
          "DESCARREGAMENTO TRANSFERÊNCIA",
          "KIT SOLUÇÃO"
        };
        private List<string> condicaoCaminhao = new List<string>()
        {
          "BOA",
          "REGULAR",
          "PÉSSIMA"
        };
        private RomaneioModel Romaneio;

        public List<string> OperacoesList
        {
            get => this.operacoes;
            set => this.operacoes = value;
        }

        public List<string> CondicaoCaminhaoList
        {
            get => this.condicaoCaminhao;
            set => this.condicaoCaminhao = value;
        }

        public ViewExpedicaoRomaneio()
        {
            InitializeComponent();
            this.DataContext = new RomaneioViewModel();
        }

        public ViewExpedicaoRomaneio(RomaneioModel romaneio)
        {
            this.InitializeComponent();
            this.Romaneio = romaneio;
            //this.DataContext = this;
        }

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = Cursors.Wait; });

                RomaneioViewModel vm = (RomaneioViewModel)DataContext;
                shopping_destino.ItemsSource = await Task.Run(() => new AprovadoViewModel().GetAprovados());
                codtransportadora.ItemsSource = await Task.Run(() => new TranportadoraViewModel().GetTransportadoras());
    
                if (Romaneio == null)
                {
                    Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                    return;
                }
                    

                IList<AprovadoModel> itemsSource1 = (IList<AprovadoModel>)shopping_destino.ItemsSource;
                int num1 = itemsSource1.IndexOf(itemsSource1.Where(a => a.SiglaServ.Equals(Romaneio.ShoppingDestino)).FirstOrDefault());

                IList<TranportadoraModel> itemsSource2 = (IList<TranportadoraModel>)codtransportadora.ItemsSource;
                int num2 = itemsSource2.IndexOf(itemsSource2.Where(t => t.CodTransportadora.Equals(Romaneio.CodTransportadora)).FirstOrDefault());

                operacao.ItemsSource = OperacoesList;
                condicao_caminhao.ItemsSource = CondicaoCaminhaoList;
                operacao.SelectedIndex = OperacoesList.FindIndex(o => o.Equals(Romaneio.Operacao));
                cod_romaneiro.Value = Romaneio.CodRomaneiro;
                data_carregamento.DateTime = new DateTime?((DateTime)Romaneio.DataCarregamento);
                hora_chegada.Value = Romaneio.HoraChegada.ToString();
                shopping_destino.SelectedIndex = num1;
                numero_caminhao.Value = new long?((long)Romaneio.NumeroCaminhao);
                local_carregamento.Text = Romaneio.LocalCarregamento;
                codtransportadora.SelectedIndex = num2;
                nome_motorista.Text = Romaneio.NomeMotorista;
                numero_cnh.Text = Romaneio.NumeroCnh;
                telefone_motorista.Text = Romaneio.TelefoneMotorista;
                condicao_caminhao.SelectedIndex = CondicaoCaminhaoList.FindIndex(c => c.Equals(Romaneio.CondicaoCaminhao));
                placa_caminhao.Text = Romaneio.PlacaCaminhao;
                placa_cidade.Text = Romaneio.PlacaCidade;
                placa_estado.Text = Romaneio.PlacaEstado;
                placa_carroceria.Text = Romaneio.PlacaCarroceria;
                placa_carroceria_cidade.Text = Romaneio.PlacaCarroceriaCidade;
                placa_carroceria_estado.Text = Romaneio.PlacaCarroceriaEstado;
                bau_altura.Value = new double?((double)Romaneio.BauAltura);
                bau_largura.Value = new double?((double)Romaneio.BauLargura);
                bau_profundidade.Value = new double?((double)Romaneio.BauProfundidade);
                m3_carregado.Value = new double?((double)Romaneio.M3Carregado);
                bau_soba.Value = new double?((double)Romaneio.BauSoba);
                m3_portaria.Value = new double?((double)Romaneio.M3Portaria);
                nome_conferente.Text = Romaneio.NomeConferente;
                num_lacres.Text = Romaneio.NumLacres;
                numero_container.Text = Romaneio.NumeroContainer;
                dateSaida.DateTime = new DateTime?((DateTime)Romaneio.DataHoraLiberacao);

                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
                MessageBox.Show(ex.Message);
            }
        }

        private void nome_motorista_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void placa_caminhao_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void placa_carroceria_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            Limpar();
        }

        private async void btnGravar_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Validar())
                return;
            try
            {
                //if (this.Romaneio == null)
                //{
                    foreach (AprovadoModel selectedItem in shopping_destino.SelectedItems)
                    {
                        RomaneioModel Romaneio = new RomaneioModel();
                        Romaneio.CodRomaneiro = this.Romaneio?.CodRomaneiro;
                        Romaneio.Operacao = operacao.SelectionBoxItem.ToString();
                        Romaneio.DataCarregamento = data_carregamento.DateTime.Value;
                        Romaneio.HoraChegada = TimeSpan.Parse(hora_chegada.Value.ToString());
                        Romaneio.ShoppingDestino = selectedItem.SiglaServ;
                        Romaneio.NumeroCaminhao = numero_caminhao.Value.Value;
                        Romaneio.LocalCarregamento = local_carregamento.Text;
                        Romaneio.CodTransportadora = (codtransportadora.SelectedItem as TranportadoraModel).CodTransportadora;
                        Romaneio.NomeMotorista = nome_motorista.Text;
                        Romaneio.NumeroCnh = numero_cnh.Text;
                        Romaneio.TelefoneMotorista = telefone_motorista.Text;
                        Romaneio.CondicaoCaminhao = condicao_caminhao.SelectionBoxItem.ToString();
                        Romaneio.PlacaCaminhao = placa_caminhao.Text;
                        Romaneio.PlacaCidade = placa_cidade.Text;
                        Romaneio.PlacaEstado = placa_estado.Text;
                        Romaneio.PlacaCarroceria = placa_carroceria.Text;
                        Romaneio.PlacaCarroceriaCidade = placa_carroceria_cidade.Text;
                        Romaneio.PlacaCarroceriaEstado = placa_carroceria_estado.Text;
                        Romaneio.BauAltura = (double)bau_altura.Value;
                        Romaneio.BauLargura = (double)bau_largura.Value;
                        Romaneio.BauProfundidade = (double)bau_profundidade.Value;
                        Romaneio.M3Carregado = (double)m3_carregado.Value;
                        Romaneio.BauSoba = (double)bau_soba.Value;
                        Romaneio.M3Portaria = (double)m3_portaria.Value;
                        Romaneio.NomeConferente = nome_conferente.Text;
                        Romaneio.NumLacres = num_lacres.Text;
                        Romaneio.NumeroContainer = numero_container.Text;
                        Romaneio.DataHoraLiberacao = dateSaida.DateTime.Value;
                        RomaneioModel romaneioModel = await new RomaneioViewModel().SaveAsync(Romaneio);
                    }
                //}
                MessageBox.Show("Romaneio salvo com sucesso...", "Romaneio", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Limpar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Limpar()
        {
            this.operacao.SelectedValue = null;
            this.operacao.IsDropDownOpen = true;
            this.cod_romaneiro.Text = "0";
            this.data_carregamento.DateTime = new DateTime?();
            this.hora_chegada.Value = null;
            this.shopping_destino.SelectedValue = null;
            this.numero_caminhao.Text = null;
            ((TextBox)this.local_carregamento).Text = "JACAREÍ";
            this.codtransportadora.SelectedValue = null;
            ((TextBox)this.nome_motorista).Text = null;
            ((TextBox)this.numero_cnh).Text = null;
            ((TextBox)this.telefone_motorista).Text = null;
            this.condicao_caminhao.SelectedValue = null;
            ((TextBox)this.placa_caminhao).Text = null;
            ((TextBox)this.placa_cidade).Text = null;
            ((TextBox)this.placa_estado).Text = null;
            ((TextBox)this.placa_carroceria).Text = null;
            ((TextBox)this.placa_carroceria_cidade).Text = null;
            ((TextBox)this.placa_carroceria_estado).Text = null;
            this.bau_altura.Value = new double?();
            this.bau_largura.Value = new double?();
            this.bau_profundidade.Value = new double?();
            this.m3_carregado.Value = new double?();
            this.bau_soba.Value = new double?();
            this.m3_portaria.Value = new double?();
            ((TextBox)this.nome_conferente).Text = null;
            ((TextBox)this.num_lacres).Text = null;
            ((TextBox)this.numero_container).Text = null;
            this.operacao.Focusable = true;
            this.operacao.Focus();
        }

        private bool Validar()
        {
            if (operacao.SelectedValue == null)
            {
                MessageBox.Show("Operação é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                operacao.Focus();
                operacao.IsDropDownOpen = true;
                return false;
            }
            if (!data_carregamento.DateTime.HasValue)
            {
                MessageBox.Show("Data de Carregamento é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                data_carregamento.Focus();
                data_carregamento.IsDropDownOpen = true;
                return false;
            }
            if (hora_chegada.Text == "")
            {
                MessageBox.Show("Hora do Carregamento é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                hora_chegada.Focus();
                return false;
            }
            if (shopping_destino.SelectedValue == null)
            {
                MessageBox.Show("Shopping é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                shopping_destino.Focus();
                shopping_destino.IsDropDownOpen = true;
                return false;
            }
            if (numero_caminhao.Text == "")
            {
                MessageBox.Show("Nº de Caminhão é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                numero_caminhao.Focus();
                return false;
            }
            if (local_carregamento.Text == "")
            {
                MessageBox.Show("Local de Carregamento é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                local_carregamento.Focus();
                return false;
            }
            if (codtransportadora.SelectedValue == null)
            {
                MessageBox.Show("Transportadora é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                codtransportadora.Focus();
                codtransportadora.IsDropDownOpen = true;
                return false;
            }
            if (nome_motorista.Text == "")
            {
                MessageBox.Show("Motorista é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                nome_motorista.Focus();
                return false;
            }
            if (numero_cnh.Text == "")
            {
                MessageBox.Show("Nº da CNH é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                numero_cnh.Focus();
                return false;
            }
            if (telefone_motorista.Text == "")
            {
                MessageBox.Show("Telefone é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                telefone_motorista.Focus();
                return false;
            }
            if (condicao_caminhao.SelectedValue == null)
            {
                MessageBox.Show("Condição do Caminhão é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                condicao_caminhao.Focus();
                condicao_caminhao.IsDropDownOpen = true;
                return false;
            }
            if (placa_caminhao.Text == "")
            {
                MessageBox.Show("Placa Caminhão/Cavalo é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                placa_caminhao.Focus();
                return false;
            }
            if (placa_cidade.Text == "")
            {
                MessageBox.Show("Cidade é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                placa_cidade.Focus();
                return false;
            }
            if (placa_estado.Text == "")
            {
                MessageBox.Show("Estado é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                placa_estado.Focus();
                return false;
            }
            if (bau_altura.Text == "")
            {
                MessageBox.Show("Altura é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                bau_altura.Focus();
                return false;
            }
            if (bau_largura.Text == "")
            {
                MessageBox.Show("Largura é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                bau_largura.Focus();
                return false;
            }
            if (bau_profundidade.Text == "")
            {
                MessageBox.Show("Profundidade é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                bau_profundidade.Focus();
                return false;
            }
            if (nome_conferente.Text == "")
            {
                MessageBox.Show("Conferente é obrigatório", "Informação requerida", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                nome_conferente.Focus();
                return false;
            }
            return true;
        }
    }
}
