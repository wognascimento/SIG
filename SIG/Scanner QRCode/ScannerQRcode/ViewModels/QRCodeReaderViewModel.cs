using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerQRcode.Data;
using ScannerQRcode.Views;

namespace ScannerQRcode.ViewModels
{
    public partial class QRCodeReaderViewModel : ObservableObject
    {

        private readonly VolumeScannerRepository _volumeScannerRepository;

        public QRCodeReaderViewModel(VolumeScannerRepository volumeScannerRepository)
        {
            _volumeScannerRepository = volumeScannerRepository;
        }
        

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task GoEnderecamento()
        {
            await Shell.Current.GoToAsync(nameof(ReaderEnderecamento));
        }
        
        [RelayCommand]
        async Task GoPreConferencia()
        {
            await Shell.Current.GoToAsync(nameof(ReaderPreConferencia));
        }
        
        [RelayCommand]
        async Task GoCargaShopping()
        {

            var dados = await _volumeScannerRepository.QueryAllVolumeLookup();
            //send.Text = $"Enviar {dados.Count} volume(s)";
            if (dados.Count == 0)
            {
                await Shell.Current.GoToAsync(nameof(LookupCargaShopping));
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(ReaderCargaShopping));
            }
        }

    }
}
