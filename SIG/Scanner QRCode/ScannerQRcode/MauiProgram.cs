using BarcodeScanner.Mobile;
using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using ScannerQRcode.Data;
using ScannerQRcode.ViewModels;
using ScannerQRcode.Views;
using Syncfusion.Maui.Core.Hosting;
using Telerik.Maui.Controls.Compatibility;

namespace ScannerQRcode;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCameraView()
            .UseTelerik()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .ConfigureMauiHandlers(handlers =>
            {
                // Add the handlers
                handlers.AddBarcodeScannerHandler();
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "coletor.db");

        //builder.Services.AddSingleton(s => ActivatorUtilities.CreateInstance<VolumeScannerRepository>(s, dbPath));

        //builder.Services.AddSingleton<DatabaseContext>();
        //builder.Services.AddSingleton<ReaderEnderecamentoViewModel>();
        //builder.Services.AddSingleton<ReaderEnderecamento>();

        builder.Services.AddSingleton(AudioManager.Current);

        builder.Services.AddSingleton(new VolumeScannerRepository("coletor.db"));

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<LookupCargaShopping>();
        builder.Services.AddTransient<QRCodeReader>();
        builder.Services.AddTransient<ReaderCargaShopping>();
        builder.Services.AddTransient<ReaderEnderecamento>();
        builder.Services.AddTransient<ReaderPreConferencia>();
        builder.Services.AddTransient<LookupCargaShoppingViewModel>();
        builder.Services.AddTransient<QRCodeReaderViewModel>();
        builder.Services.AddTransient<ReaderCargaShoppingViewModel>();
        builder.Services.AddTransient<ReaderEnderecamentoViewModel>();
        builder.Services.AddTransient<ReaderPreConferenciaViewModel>();

        //builder.Services.AddSingleton<QRCodeReaderViewModel>();

        //builder.Services.AddSingleton<ReaderEnderecamento>();
        //builder.Services.AddSingleton<LookupCargaShopping>();
        //builder.Services.AddSingleton<QRCodeReader>();

        /*
        builder.Services.AddScoped<ReaderEnderecamento>();
        builder.Services.AddScoped<LookupCargaShopping>();
        builder.Services.AddScoped<QRCodeReader>();
        */

        return builder.Build();
	}
}
