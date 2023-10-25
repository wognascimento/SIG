using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitorControladoShopping.views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeitorControladoShopping.ViewModels
{
    public partial class PrincipalViewModel : ObservableObject
    {
        public PrincipalViewModel()
        {
        }

        [RelayCommand]
        public async Task SendVolumes()
        {
            Console.WriteLine($"delete");
        }

        [RelayCommand]
        public async Task ScannerVolume()
        {
            await Shell.Current.GoToAsync(nameof(Scanner));
        }
    }
}
