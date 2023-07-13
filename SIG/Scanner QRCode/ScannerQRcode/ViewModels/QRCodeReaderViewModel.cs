using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScannerQRcode.Views;

namespace ScannerQRcode.ViewModels
{
    public partial class QRCodeReaderViewModel : ObservableObject
    {

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
            await Shell.Current.GoToAsync(nameof(ReaderCargaShopping));
        }

    }
}
