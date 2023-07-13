using BarcodeScanner.Mobile;
using Camera.MAUI;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using ScannerQRcode.Data;
using ScannerQRcode.Views;
using Syncfusion.Maui.Core.Hosting;

namespace ScannerQRcode;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCameraView()
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

        builder.Services.AddSingleton(new VolumeScannerRepository("accounts.db"));
        builder.Services.AddScoped<ReaderEnderecamento>();

        return builder.Build();
	}
}
