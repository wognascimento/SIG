using BarcodeScanner.Mobile;
using Plugin.LocalNotification;
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

        var dados = _volumeScannerRepository.GetVolumeScanners();
        send.Text = $"Enviar {dados.Count} volume(s)";
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
        for (int i = 0; i < obj.Count; i++)
        {
            //result += $"Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine}";
            result += $"{obj[i].DisplayValue}{Environment.NewLine}";
        }

        Dispatcher.Dispatch(async () =>
        {
            //await DisplayAlert("Result", result, "OK");
            // If you want to start scanning again

            //App.VolumeScannerRepository.Add(new VolumeScanner { Volume = result });
            
            var lookup = _volumeScannerRepository.GetVolumeLookup(result);
            if (lookup != null)
            {
                _volumeScannerRepository.CreateVolumeScanner(new VolumeScanner { Volume = result, Tipo = "ENDEREÇAMENTO" });
                var dados = _volumeScannerRepository.GetVolumeScanners();
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