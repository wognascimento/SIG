using System.Text.RegularExpressions;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace LeitorControladoShopping.views;

public partial class Scanner : ContentPage
{
	public Scanner()
	{
		InitializeComponent();

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
            Dispatcher.Dispatch(() =>
            {
                // Update BarcodeGeneratorView
                barcodeGenerator.ClearValue(BarcodeGeneratorView.ValueProperty);
                barcodeGenerator.Format = first.Format;
                barcodeGenerator.Value = first.Value;

                if (match.Success)
                {
                    //string numeroEncontrado = match.Groups[1].Value;
                    //Console.WriteLine($"Número encontrado: {numeroEncontrado}");
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