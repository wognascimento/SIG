
using BarcodeScanner.Mobile;
using Plugin.Maui.Audio;
using ScannerQRcode.Data;
using ScannerQRcode.Data.Api.Models;
using ScannerQRcode.Models;
using ScannerQRcode.ViewModels;
using System.Text;
using System.Text.Json;

namespace ScannerQRcode.Views;

public partial class ReaderCargaShopping : ContentPage
{
    private readonly VolumeScannerRepository _volumeScannerRepository;
    private readonly IAudioManager audioManager;
    private LookupCarregamento lookupCarregamento;

    public ReaderCargaShopping(VolumeScannerRepository volumeScannerRepository, ReaderCargaShoppingViewModel vm, IAudioManager audioManager)
	{
        InitializeComponent();
        _volumeScannerRepository = volumeScannerRepository;
        this.audioManager = audioManager;
        //BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Code39 | BarcodeFormats.QRCode | BarcodeFormats.Code128);
#if ANDROID
        //Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode | BarcodeFormats.Code39);
        Methods.AskForRequiredPermission();
#endif
        BindingContext = vm;
        /*
        cameraView.BarCodeOptions = new()
        {
            PossibleFormats = { ZXing.BarcodeFormat.QR_CODE }
        };
        */

        new Action(async () =>
        {
            var dados = await _volumeScannerRepository.GetVolumeScanners();
            send.Text = $"Enviar {dados.Count} volume(s)";
            lookupCarregamento = await _volumeScannerRepository.GetLookupCarregamento();
        }).Invoke();

        
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        /*
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
        */
    }

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlert(args.Result[0].BarcodeFormat.ToString(), args.Result[0].Text, "OK");
        });
    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        ReaderCargaShoppingViewModel vm = (ReaderCargaShoppingViewModel)BindingContext;

        vm.IsLoading = true;

        var volumes = await _volumeScannerRepository.GetVolumeScanners();
        JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        //volumes.Add(new VolumeScanner { Volume = "04567890023398", Tipo = "CARGA-SHOPPING", Created = DateTime.Now });
        foreach (var volume in volumes)
        {
            try
            {
                vm.Status = "Buscando Siglas aprovadas.";
                var httpClient = new HttpClient();
                var cargaGeral = new ConfCargaGeral()
                {
                    Barcode = volume.Volume,
                    DocaOrigem = "JACAREÍ",
                    Data = DateOnly.FromDateTime(DateTime.Now),
                    Shopp = lookupCarregamento.Sigla,
                    Resp = "APP ANDROID",
                    Caminhao = lookupCarregamento.PlacaCaminhao,
                };
                string json = JsonSerializer.Serialize<ConfCargaGeral>(cargaGeral, _serializerOptions);
                StringContent content = new(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://mail.cipolatti.com.br:8080/api/ConfCargaGeral/GravarVolume", content);
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

        vm.IsLoading = false;
        await DisplayAlert("Envio", "Volumes enviados com sucesso!", "OK");
    }

    private void Camera_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        List<BarcodeResult> obj = e.BarcodeResults;

        string result = string.Empty;
        string type = string.Empty;
        VolumeLookup lookup = null;
        for (int i = 0; i < obj.Count; i++)
        {
            //result += $"{obj[i].DisplayValue}{Environment.NewLine}";
            result += $"{obj[i].DisplayValue}";
            type += $"{obj[i].BarcodeFormat}";
        }

        Dispatcher.Dispatch(async () =>
        {
            //var volume = result.Replace("\n", null);
            //BarcodeFormats.Code39 | BarcodeFormats.QRCode | BarcodeFormats.Code128
            //if (type == BarcodeFormats.Code39)
            //await DisplayAlert("type", $"Tipo {type} ", "OK");
            //return;

            if (type == BarcodeFormats.Code39.ToString() || type == BarcodeFormats.Code128.ToString())
                lookup = await _volumeScannerRepository.GetVolumeLookupByCode39(result);
            else if (type == BarcodeFormats.QRCode.ToString())
                lookup = await _volumeScannerRepository.GetVolumeLookupByQrCode(result);
           

            if (lookup != null)
            {
                await _volumeScannerRepository.CreateVolumeScanner(new VolumeScanner { Volume = lookup.Volume, Created = DateTime.Now ,Tipo = "CARGA-SHOPPING" });
                var dados = await _volumeScannerRepository.GetVolumeScanners();
                send.Text = $"Enviar {dados.Count} volume(s)";

                var pSucess = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("sucess.mp3"));
                pSucess.Play();
            }
            else
            {
                Camera.IsScanning = false;
                int secondsToVibrate = Random.Shared.Next(1, 4);
                TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
                Vibration.Default.Vibrate(vibrationLength);

                var pError = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("error.mp3"));
                pError.Play();

                await DisplayAlert("Volume", $"Volume {result} Não presente no Lookup", "OK");
                Camera.IsScanning = true;
            }

            Camera.IsScanning = true;

        });
    }
}