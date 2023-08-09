using ScannerQRcode.Data;
using ScannerQRcode.Views;

namespace ScannerQRcode;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("OTcwMkAzMjMwMkUzNDJFMzBPS3JpdmtTUjlQSmZldndUek5rRHdkSUFpaEtJc296dXdJM3pCdUhzNVpjPQ==");

        InitializeComponent();

        MainPage = new AppShell();
        //MainPage = serviceProvider.GetRequiredService<QRCodeReader>();


    }
}
