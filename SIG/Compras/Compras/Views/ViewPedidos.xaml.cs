﻿using Compras.DataBase.Model;
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

namespace Compras.Views
{
    /// <summary>
    /// Interação lógica para ViewPedidos.xam
    /// </summary>
    public partial class ViewPedidos : UserControl
    {
        public ViewPedidos()
        {
            InitializeComponent();
            this.DataContext = new QryPedidoViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                QryPedidoViewModel vm = (QryPedidoViewModel)DataContext;
                await Task.Run(vm.GetPedidosAsync);
                loading.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void OnDbClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                loading.Visibility = Visibility.Visible;
                QryPedidoViewModel vm = (QryPedidoViewModel)DataContext;
                await Task.Run(()=> vm.GetPedidoDetalhesAsync(vm.Pedido.idpedido));
                loading.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }
    }

    public class QryPedidoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #region Consulta Pedido
        private QryPedidoModel pedido;
        public QryPedidoModel Pedido
        {
            get { return pedido; }
            set { pedido = value; RaisePropertyChanged("Pedido"); }
        }
        private ObservableCollection<QryPedidoModel> pedidos;
        public ObservableCollection<QryPedidoModel> Pedidos
        {
            get { return pedidos; }
            set { pedidos = value; RaisePropertyChanged("Pedidos"); }
        }
        #endregion

        #region Consulta Pedido Detalhes
        private QryPedidosDet pedidoDet;
        public QryPedidosDet PedidoDet
        {
            get { return pedidoDet; }
            set { pedidoDet = value; RaisePropertyChanged("PedidoDet"); }
        }
        private ObservableCollection<QryPedidosDet> pedidoDets;
        public ObservableCollection<QryPedidosDet> PedidoDets
        {
            get { return pedidoDets; }
            set { pedidoDets = value; RaisePropertyChanged("PedidoDets"); }
        }
        #endregion

        public async Task GetPedidosAsync()
        {
            try
            {
                using DatabaseContext db = new();
                Pedidos = (new ObservableCollection<QryPedidoModel>(await db.QryPedidos.ToListAsync()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetPedidoDetalhesAsync(long? idpedido)
        {
            try
            {
                using DatabaseContext db = new();
                PedidoDets = (new ObservableCollection<QryPedidosDet>(await db.QryPedidoDetalhes.Where(p => p.idpedido == idpedido).ToListAsync()));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
