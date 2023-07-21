using CommunityToolkit.Mvvm.ComponentModel;
using ScannerQRcode.Data;
using ScannerQRcode.Data.Api.Models;
using System.Collections.ObjectModel;

namespace ScannerQRcode.ViewModels
{
    public partial class LookupCargaShoppingViewModel : ObservableObject
    {
        private readonly VolumeScannerRepository _volumeScannerRepository;
        public LookupCargaShoppingViewModel(VolumeScannerRepository volumeScannerRepository)
        {
            _volumeScannerRepository = volumeScannerRepository;
            Aprovados = new ObservableCollection<Aprovado>();
            Lookups = new ObservableCollection<Lookup>();
            Status = "Buscando Siglas aprovadas.";
        }

        [ObservableProperty]
        ObservableCollection<Aprovado> aprovados;
        
        [ObservableProperty]
        ObservableCollection<Lookup> lookups;

        [ObservableProperty]
        ObservableCollection<Aprovado> selectedItems;

        [ObservableProperty]
        bool bloqCaminao = true;

        [ObservableProperty]
        bool isLoading = true;

        [ObservableProperty]
        string caminhao;
        [ObservableProperty]
        string placaCaminhao;

        [ObservableProperty]
        string status = "Buscando Siglas aprovadas.";



    }
}
