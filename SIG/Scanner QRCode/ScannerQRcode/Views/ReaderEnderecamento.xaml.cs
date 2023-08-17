using BarcodeScanner.Mobile;
using Plugin.Maui.Audio;
using ScannerQRcode.Data;
using ScannerQRcode.Data.Api.Models;
using ScannerQRcode.Models;
using ScannerQRcode.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static SQLite.TableMapping;

namespace ScannerQRcode.Views;

public partial class ReaderEnderecamento : ContentPage
{

    private readonly VolumeScannerRepository _volumeScannerRepository;
    private readonly IAudioManager audioManager;
    private EnderecoGalpao _endereco;

    public ReaderEnderecamento(VolumeScannerRepository volumeScannerRepository, ReaderEnderecamentoViewModel vm, IAudioManager audioManager)
	{
        InitializeComponent();
        _volumeScannerRepository = volumeScannerRepository;
        this.audioManager = audioManager;
#if ANDROID
        Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode | BarcodeFormats.Code39 | BarcodeFormats.Code128);
        Methods.AskForRequiredPermission();
#endif
        BindingContext = vm;
        this.Loaded += ReaderEnderecamento_Loaded;
    }

    private async void ReaderEnderecamento_Loaded(object sender, EventArgs e)
    {
        ReaderEnderecamentoViewModel vm = (ReaderEnderecamentoViewModel)BindingContext;
        var dados = await Task.Run(_volumeScannerRepository.QueryAllVolumeEnderecados);
        send.Text = $"Enviar {dados.Count} volume(s)";
        var enderecos = await Task.Run(_volumeScannerRepository.QueryAllEnderecos);
        if (enderecos.Count == 0) 
        {
            try
            {
                vm.IsLoading = true;
                vm.Status = "Buscando endereços.";
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("http://mail.cipolatti.com.br:8080/api/EnderecamentoGalpao/Enderecos");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    vm.Enderecos = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<EnderecoGalpao>>(content);
                    vm.Status = "Salvando endereços no didpositivo.";
                    foreach (EnderecoGalpao endereco in vm.Enderecos)
                    {
                        await Task.Run(() => _volumeScannerRepository.CreateEndereco(endereco));
                    }
                    vm.IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
                vm.IsLoading = false;
            }
        }
        //await _volumeScannerRepository.CreateVolumeEnderecamento(new VolumeEnderecamento { Endereco = "01234567000275", Volume = "04567890038293", Created = DateTime.Now });
    }

    private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
    {
        if (e.IsDismissed)
        {

        }
        else if (e.IsTapped)
        {

        }
    }

    private void Camera_OnDetected(object sender, OnDetectedEventArg e)
    {
        List<BarcodeResult> obj = e.BarcodeResults;

        string result = string.Empty;
        string type = string.Empty;

        for (int i = 0; i < obj.Count; i++)
        {
            //result += $"Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine}";
            result += $"{obj[i].DisplayValue}";
            type += $"{obj[i].BarcodeFormat}";
        }

        Dispatcher.Dispatch(async () =>
        {
            if (_endereco == null)
            {
                var endereco = await Task.Run(() => _volumeScannerRepository.GetEndereco(result));
                if (endereco != null) 
                    _endereco = endereco;
                else
                {
                    Camera.IsScanning = false;
                    int secondsToVibrate = Random.Shared.Next(1, 4);
                    TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
                    Vibration.Default.Vibrate(vibrationLength);

                    var pError = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.mp3"));
                    pError.Play();

                    await DisplayAlert("Endereço", "Código não corresponde há um endereço", "OK");
                    Camera.IsScanning = true;
                }
            }
            else
            {
                if (result.Length != 14 || !result.Contains("0456789"))
                {
                    Camera.IsScanning = false;
                    int secondsToVibrate = Random.Shared.Next(1, 4);
                    TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
                    Vibration.Default.Vibrate(vibrationLength);

                    var pError = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.mp3"));
                    pError.Play();

                    await DisplayAlert("Volume", "Código não corresponde há um volume de shopping", "OK");
                    Camera.IsScanning = true;
                }
                else
                {
                    await _volumeScannerRepository.CreateVolumeEnderecamento(new VolumeEnderecamento { Endereco = _endereco.Barcode, Volume = result, Created = DateTime.Now });
                    var dados = await _volumeScannerRepository.QueryAllVolumeEnderecados();
                    //var groupedCustomerList = (dados.GroupBy(u => u.Volume).Select(grp => grp.ToList()).ToList()).Count;
                    send.Text = $"Enviar {(dados.GroupBy(u => u.Volume).Select(grp => grp.ToList()).ToList()).Count} volume(s)";

                    var pSucess = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("sucess.mp3"));
                    pSucess.Play();
                }
            }

            Camera.IsScanning = true;
        });

    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        ReaderEnderecamentoViewModel vm = (ReaderEnderecamentoViewModel)BindingContext;

        vm.Status = "BUSCANDO VOLUMES ENDEREÇADOS NO DISPOSITIVO.";
        vm.IsLoading = true;

        var dados = await Task.Run(_volumeScannerRepository.QueryAllVolumeEnderecados);
        int tot = dados.Count();

        if (tot == 0)
        {
            await DisplayAlert("Enviar ", "Não tem volumes para serem enviados", "OK");
            vm.IsLoading = false;
            return;
        }

        JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        if (dados.Count > 0)
        {
            foreach (var volume in dados)
            {
                try
                {
                    vm.Status = $"ENVIANDO VOLUMES ENDEREÇADOS.";
                    var httpClient = new HttpClient();
                    /*var movimentacaoVolumeShopping = new MovimentacaoVolumeShopping()
                    {
                        barcode_volume = volume.Volume,
                        barcode_endereco = volume.Endereco,
                        inserido_por = "APP ANDROID",
                        inserido_em = DateTime.Now,

                    };*/
                    //string json = JsonSerializer.Serialize<MovimentacaoVolumeShopping>(movimentacaoVolumeShopping, _serializerOptions);
                    //StringContent content = new(json, Encoding.UTF8, "application/json");
                    //var response = await httpClient.PostAsync("http://mail.cipolatti.com.br:8080/api/MovimentacaoVolumeShopping/GravarVolume", content);
                    using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            //idLinhaInserida = 0,
                            barcodeVolume = volume.Volume,
                            barcodeEndereco = volume.Endereco,
                            inseridoPor = "APP ANDROID",
                            inseridoEm = DateTime.Now
                        }),
                        Encoding.UTF8,
                        "application/json");

                    using HttpResponseMessage response = await httpClient.PostAsync("http://mail.cipolatti.com.br:8080/api/MovimentacaoVolumeShopping/GravarVolume", jsonContent);
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        if (jsonResponse == "Volume enviado com sucesso!")
                        {
                            //await Task.Run(()=>_volumeScannerRepository.DeleteVolumeEnderecados(volume));
                            //var ende = await Task.Run(_volumeScannerRepository.QueryAllVolumeEnderecados);
                            tot--;
                            send.Text = $"Enviar {tot} volume(s)";

                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
                    vm.IsLoading = false;
                }
            }
            vm.Status = $"DELETANDO VOLUMES ENDEREÇADOS";
            await Task.Run(_volumeScannerRepository.DeleteAllVolumeEnderecados);

            send.Text = $"Enviar 0 volume(s)";
            vm.IsLoading = false;
            _endereco = null;
        }
        
    }
}