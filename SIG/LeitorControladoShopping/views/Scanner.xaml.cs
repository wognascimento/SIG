using LeitorControladoShopping.Data.Local;
using LeitorControladoShopping.Data.Local.Model;
using System.Text.RegularExpressions;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace LeitorControladoShopping.views;

public partial class Scanner : ContentPage
{
    private readonly VolumeScannerRepository _volumeScannerRepository;

    public Scanner(VolumeScannerRepository volumeScannerRepository)
	{
		InitializeComponent();
        _volumeScannerRepository = volumeScannerRepository;
        barcodeView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = true
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        /*
        foreach (var barcode in e.Results)
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
        */
        string pattern = @"\|(\d+)";
        var first = e.Results?.FirstOrDefault();
        

        if (first is not null)
        {
            Match match = Regex.Match(first.Value, pattern);
            string[] volume = first.Value.Split('|');
            Dispatcher.Dispatch(async () =>
            {
                // Update BarcodeGeneratorView
                //barcodeGenerator.ClearValue(BarcodeGeneratorView.ValueProperty);
                //barcodeGenerator.Format = first.Format;
                //barcodeGenerator.Value = first.Value;

                if (match.Success)
                {
                    //string numeroEncontrado = match.Groups[1].Value;
                    //Console.WriteLine($"Número encontrado: {numeroEncontrado}");

                    try
                    {
                        var controlado = await Task.Run(() => _volumeScannerRepository.GetVolume(volume[0], long.Parse(volume[1])));
                        if (controlado == null)
                            await Task.Run(() => _volumeScannerRepository.CreateVolumeControlado(new VolumeControlado { Sigla = volume[0], Volume = long.Parse(volume[1]) }));
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                    ResultLabel.Text = $"Barcodes: {first.Format} -> {first.Value}";
                }
                else
                {
                    //Console.WriteLine("Nenhuma sequência de números encontrada após o '|'");
                    ResultLabel.Text = $"QrCode não é de shopping";
                }

                // Update Label
                //ResultLabel.Text = $"Barcodes: {first.Format} -> {first.Value}";
            });
        }
    }

    void SwitchCameraButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.CameraLocation = barcodeView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
    }

    void TorchButton_Clicked(object sender, EventArgs e)
    {
        barcodeView.IsTorchOn = !barcodeView.IsTorchOn;
    }
}