using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitorControladoShopping.Data.Local;
using LeitorControladoShopping.Data.Local.Model;
using LeitorControladoShopping.views;
using System.Collections.ObjectModel;

namespace LeitorControladoShopping.ViewModels
{
    public partial class PrincipalViewModel : ObservableObject
    {

        private readonly VolumeScannerRepository _volumeScannerRepository;
        public PrincipalViewModel(VolumeScannerRepository volumeScannerRepository)
        {
            _volumeScannerRepository = volumeScannerRepository;
        }

        [ObservableProperty]
        ObservableCollection<VolumeControlado> volumeControlados;


        public async Task<ObservableCollection<VolumeControlado>> GetVolumesAsync()
        {
            try
            {
                var dados  = await _volumeScannerRepository.GetAllVolumeScanners();
                return new ObservableCollection<VolumeControlado>(dados);
            }
            catch (Exception)
            {
                throw;
            }
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
