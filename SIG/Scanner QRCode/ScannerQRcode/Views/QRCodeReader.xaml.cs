using ScannerQRcode.ViewModels;

namespace ScannerQRcode.Views;

public partial class QRCodeReader : ContentPage
{
	public QRCodeReader(QRCodeReaderViewModel vm)
	{
		InitializeComponent();
        //BindingContext = new QRCodeReaderViewModel();
        BindingContext = vm;
    }

	/*
    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
		cameraView.Camera = cameraView.Cameras.First();

		MainThread.BeginInvokeOnMainThread( async () => 
		{
			await cameraView.StopCameraAsync();
			await cameraView.StartCameraAsync();
		});
    }
	*/
}