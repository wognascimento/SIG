using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Producao.Views.CentralModelos
{
    /// <summary>
    /// Lógica interna para ModeloFiada.xaml
    /// </summary>
    public partial class ModeloFiada : Window
    {
        private QryModeloModel? modelo;

        public ModeloFiada(QryModeloModel? modelo)
        {
            InitializeComponent();
            this.modelo = modelo;
            this.DataContext = new ModeloFiadaViewModel();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ModeloFiadaViewModel vm = (ModeloFiadaViewModel)DataContext;
            vm.Modelo = this.modelo;
            vm.Modelos = new ObservableCollection<string> { "MOD. 01", "MOD. 02", "MOD. 03", "MOD. 04", "MOD. 05", "MOD. 06", "MOD. 07", "MOD. 08", "MOD. 09", "MOD. 10" };

            try
            {
                vm.ModeloFiada = await Task.Run(() => vm.GetModelosFiadaAsync(modelo));
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class ModeloFiadaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private ObservableCollection<string>? modelos;
        public ObservableCollection<string> Modelos
        {
            get { return modelos; }
            set { modelos = value; RaisePropertyChanged("Modelos"); }
        }

        private ObservableCollection<ModeloFiadaModel>? modeloFiada;
        public ObservableCollection<ModeloFiadaModel> ModeloFiada
        {
            get { return modeloFiada; }
            set { modeloFiada = value; RaisePropertyChanged("ModeloFiada"); }
        }

        private QryModeloModel? modelo;
        public QryModeloModel Modelo
        {
            get { return modelo; }
            set { modelo = value; RaisePropertyChanged("Modelo"); }
        }

        public async Task<ObservableCollection<ModeloFiadaModel>> GetModelosFiadaAsync(QryModeloModel? modelo)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ModelosFiada
                    .OrderBy(c => c.modelofiada)
                    .Where(c => c.id_modelo == modelo.id_modelo)
                    .ToListAsync();
                return new ObservableCollection<ModeloFiadaModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
