using BarcodeScanner.Mobile;
using ScannerQRcode.Data;
using ScannerQRcode.Data.Api.Models;
using ScannerQRcode.Models;
using ScannerQRcode.ViewModels;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ScannerQRcode.Views;

public partial class ReaderEnderecamento : ContentPage
{

    private readonly VolumeScannerRepository _volumeScannerRepository;
    private string volumeEndereco;

    public ReaderEnderecamento(VolumeScannerRepository volumeScannerRepository, ReaderEnderecamentoViewModel vm)
	{
        _volumeScannerRepository = volumeScannerRepository;
        InitializeComponent();
        //BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Code39 | BarcodeFormats.QRCode | BarcodeFormats.Code128);
        BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Code39 | BarcodeFormats.Code128);

        //LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        //var dados = await _volumeScannerRepository.GetVolumeScanners();
        //send.Text = $"Enviar {dados.Count} volume(s)";


        #if ANDROID
        //Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode | BarcodeFormats.Code39);
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
        var dados = await Task.Run(_volumeScannerRepository.QueryAllVolumeEnderecados);  //_volumeScannerRepository.QueryAllVolumeEnderecados();

        JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        send.Text = $"Enviar {dados.Count} volume(s)";
        if (dados.Count > 0)
        {
            bool answer = await DisplayAlert("Volumes", "Foram encontrados volumes não enviados nesseste aparelho! \n Deseja enviar agora?", "Sim", "Não");
            if (answer) 
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
            else
            {
                await Shell.Current.GoToAsync("..");
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

            Camera.IsScanning = true;
        });



    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        ReaderEnderecamentoViewModel vm = (ReaderEnderecamentoViewModel)BindingContext;
        var dados = await Task.Run(_volumeScannerRepository.QueryAllVolumeEnderecados); 

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
    }
}