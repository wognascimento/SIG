
namespace ScannerQRcode.Views;

public partial class ReaderCargaShopping : ContentPage
{
	public ReaderCargaShopping()
	{
		InitializeComponent();

        cameraView.BarCodeOptions = new()
        {
            PossibleFormats = { ZXing.BarcodeFormat.QR_CODE }
        };

    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlert(args.Result[0].BarcodeFormat.ToString(), args.Result[0].Text, "OK");
        });
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        //myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
    }
}