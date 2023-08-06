using BarcodeScanner.Mobile;
using ScannerQRcode.Data;
using ScannerQRcode.Data.Api.Models;
using ScannerQRcode.Models;
using ScannerQRcode.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ScannerQRcode.Views;

public partial class ReaderEnderecamento : ContentPage
{

    private readonly VolumeScannerRepository _volumeScannerRepository;
    private EnderecoGalpao _endereco;

    public ReaderEnderecamento(VolumeScannerRepository volumeScannerRepository, ReaderEnderecamentoViewModel vm)
	{
        _volumeScannerRepository = volumeScannerRepository;
        InitializeComponent();
        //BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Code39 | BarcodeFormats.QRCode | BarcodeFormats.Code128);
        //BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Code39 | BarcodeFormats.Code39 | BarcodeFormats.Code128);

        //LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        //var dados = await _volumeScannerRepository.GetVolumeScanners();
        //send.Text = $"Enviar {dados.Count} volume(s)";


#if ANDROID
        Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode | BarcodeFormats.Code39 | BarcodeFormats.Code128);
        Methods.AskForRequiredPermission();
#endif

        BindingContext = vm;
        this.Loaded += ReaderEnderecamento_Loaded;

        /*
        new Action(async () =>
        {
            var dados = await _volumeScannerRepository.QueryAllVolumeEnderecados();
            send.Text = $"Enviar {dados.Count} volume(s)";
        }).Invoke();
        */
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
            /*
            if (volumeEndereco == null || volumeEndereco == "")
            {
                volumeEndereco = result;
            }
            else
            {
                await _volumeScannerRepository.CreateVolumeEnderecamento(new VolumeEnderecamento { Endereco = volumeEndereco, Volume = result, Created = DateTime.Now });
                var dados = await _volumeScannerRepository.QueryAllVolumeEnderecados();
                send.Text = $"Enviar {dados.Count} volume(s)";
            }
            */

            if (_endereco == null)
            {
                var endereco = await Task.Run(() => _volumeScannerRepository.GetEndereco(result));
                if (endereco != null) 
                {
                    _endereco = endereco;
                }
                else
                {
                    await DisplayAlert("Endereço", "Código não corresponde há um endereço", "OK");
                }
            }

            Camera.IsScanning = true;
        });



    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        ReaderEnderecamentoViewModel vm = (ReaderEnderecamentoViewModel)BindingContext;
        var dados = await Task.Run(_volumeScannerRepository.QueryAllVolumeEnderecados);

        vm.Status = "ENVIANDO VOLUMES...";
        vm.IsLoading = true;

        /*
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
                    vm.Status = "Buscando Siglas aprovadas.";
                    var httpClient = new HttpClient();
                    var movimentacaoVolumeShopping = new MovimentacaoVolumeShopping()
                    {
                        barcode_volume = volume.Volume,
                        barcode_endereco = volume.Endereco,
                        inserido_por = "APP ANDROID",
                        inserido_em = DateTime.Now,

                    };
                    string json = JsonSerializer.Serialize<MovimentacaoVolumeShopping>(movimentacaoVolumeShopping, _serializerOptions);
                    StringContent content = new(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("http://mail.cipolatti.com.br:8080/api/MovimentacaoVolumeShopping/GravarVolume", content);
                    if (response.IsSuccessStatusCode)
                    {
                        //var content = await response.Content.ReadAsStringAsync();
                        //vm.Aprovados = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Aprovado>>(content);
                        //vm.IsLoading = false;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
                    vm.IsLoading = false;
                }
            }

        }
        */
    }
}