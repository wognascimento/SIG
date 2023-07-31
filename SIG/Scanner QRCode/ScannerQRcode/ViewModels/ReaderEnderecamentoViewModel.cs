using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerQRcode.Data;
using ScannerQRcode.Models;
using System.Collections.ObjectModel;

namespace ScannerQRcode.ViewModels
{
    public partial class ReaderEnderecamentoViewModel : ObservableObject
    {
        private readonly VolumeScannerRepository _volumeScannerRepository;

        public ReaderEnderecamentoViewModel(VolumeScannerRepository volumeScannerRepository)
        {
            _volumeScannerRepository = volumeScannerRepository;
            Status = "Enviando Volumes";
        }

        [ObservableProperty]
        string status;

        [ObservableProperty]
        bool isLoading;
    }
}
