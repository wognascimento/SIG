using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LeitorControladoShopping.Data.Api.Model;
using LeitorControladoShopping.Data.Local;
using LeitorControladoShopping.Data.Local.Model;
using LeitorControladoShopping.views;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace LeitorControladoShopping.ViewModels
{
    public partial class PrincipalViewModel : ObservableObject
    {

        private readonly VolumeScannerRepository _volumeScannerRepository;
        public PrincipalViewModel(VolumeScannerRepository volumeScannerRepository)
        {
            _volumeScannerRepository = volumeScannerRepository;
            //Status = "Enviando Volumes";
        }

        [ObservableProperty]
        ObservableCollection<VolumeControlado> volumeControlados;

        [ObservableProperty]
        string status = "Enviando volumes controlado para Cipolatti.";

        [ObservableProperty]
        bool isLoading = false;

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
            //Console.WriteLine($"delete");
            Status = "BUSCANDO VOLUMES CONTROLADO NÃO ENVIADOS NO DISPOSITIVO.";
            IsLoading = true;
            var dados = await Task.Run(_volumeScannerRepository.GetVolumesNotSenders);
            int tot = dados.Count();
            if (tot == 0)
            {
                //await Page.DisplayAlert("Enviar ", "Não tem volumes para serem enviados", "OK");
                await Application.Current.MainPage.DisplayAlert("ENVIO CONTROLADO", "NÃO EXISTE VOLUMES HA SER ENVIADOS.", "OK");
                IsLoading = false;
                return;
            }

            JsonSerializerOptions _serializerOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            foreach (var volume in dados)
            {
                try
                {
                    Status = $"ENVIANDO VOLUME CONTROLADO {volume.Sigla} - {volume.Volume}";
                    var httpClient = new HttpClient();
                    using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            //id = null,
                            sigla = volume.Sigla,
                            volume = volume.Volume,
                            conferido = DateOnly.FromDateTime(volume.Created.Value),
                            //recebido = DateTime.Now

                        }),
                        Encoding.UTF8, "application/json");

                    using HttpResponseMessage response = await httpClient.PostAsync("http://api.cipolatti.com.br:44366/api/VolumeControlado/ReceberControlado", jsonContent);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        if (jsonResponse == "Volume controlado enviado com sucesso!" || jsonResponse == "Nada a fazer.")
                        {
                            volume.IsEnviado = true;
                            await Task.Run(() => _volumeScannerRepository.UpdateVolumeScanner(volume));
                        }
                    }
                    IsLoading = false;
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
                    IsLoading = false;
                }
            }


        }

        [RelayCommand]
        public async Task ScannerVolume()
        {
            await Shell.Current.GoToAsync(nameof(Scanner));
        }
    }
}
