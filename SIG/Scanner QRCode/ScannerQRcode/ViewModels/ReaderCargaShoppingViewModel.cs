using CommunityToolkit.Mvvm.ComponentModel;
using ScannerQRcode.Data;

namespace ScannerQRcode.ViewModels
{
    public partial class ReaderCargaShoppingViewModel : ObservableObject
    {
        private readonly VolumeScannerRepository _volumeScannerRepository;

        public ReaderCargaShoppingViewModel(VolumeScannerRepository volumeScannerRepository)
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
