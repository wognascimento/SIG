using BarcodeScanner.Mobile;
using ScannerQRcode.Data;
using ScannerQRcode.Models;

namespace ScannerQRcode.Views;

public partial class ReaderEnderecamento : ContentPage
{

    private readonly VolumeScannerRepository _volumeScannerRepository;

    public ReaderEnderecamento(VolumeScannerRepository volumeScannerRepository)
	{
        _volumeScannerRepository = volumeScannerRepository;
        InitializeComponent();
        //BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeFormats.Code39 | BarcodeFormats.QRCode | BarcodeFormats.Code128);

        //LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;

        //var dados = await _volumeScannerRepository.GetVolumeScanners();
        //send.Text = $"Enviar {dados.Count} volume(s)";

        new Action(async () =>
        {
            var dados = await _volumeScannerRepository.GetVolumeScanners();
            send.Text = $"Enviar {dados.Count} volume(s)";
        }).Invoke();

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
        VolumeLookup lookup = null;
        for (int i = 0; i < obj.Count; i++)
        {
            //result += $"Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine}";
            result += $"{obj[i].DisplayValue}";
            type += $"{obj[i].BarcodeFormat}";
        }

        Dispatcher.Dispatch(async () =>
        {
            //await DisplayAlert("Result", result, "OK");
            // If you want to start scanning again

            //App.VolumeScannerRepository.Add(new VolumeScanner { Volume = result });

            if (type == BarcodeFormats.Code39.ToString())
                lookup = await _volumeScannerRepository.GetVolumeLookupByCode39(result);
            else if (type == BarcodeFormats.QRCode.ToString())
                lookup = await _volumeScannerRepository.GetVolumeLookupByQrCode(result);

            if (lookup != null)
            {
                await _volumeScannerRepository.CreateVolumeScanner(new VolumeScanner { Volume = result, Tipo = "ENDEREÇAMENTO" });
                var dados = await _volumeScannerRepository.GetVolumeScanners();
                send.Text = $"Enviar {dados.Count} volume(s)";
            }
            else
            {
                Camera.IsScanning = false;
                //Vibration.Default.Vibrate(TimeSpan.FromSeconds(2));
                int secondsToVibrate = Random.Shared.Next(1, 4);
                TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

                Vibration.Default.Vibrate(vibrationLength);
                await DisplayAlert("Volume", $"Volume {result} Não presente no Lookup", "OK");
                Camera.IsScanning = true;
            }
            
            Camera.IsScanning = true;

        });
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var dados = _volumeScannerRepository.GetVolumeScanners();
    }
}