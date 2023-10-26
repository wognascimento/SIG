using LeitorControladoShopping.Data.Local;
using LeitorControladoShopping.ViewModels;
using LeitorControladoShopping.views;
using Microsoft.Extensions.Logging;
using Telerik.Maui.Controls.Compatibility;
using ZXing.Net.Maui.Controls;

#if ANDROID
[assembly: Android.App.UsesPermission(Android.Manifest.Permission.Camera)]
#endif

namespace LeitorControladoShopping
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseTelerik()
                .UseBarcodeReader()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton(new VolumeScannerRepository("ColetorSQLite.db3"));

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<Principal>();
            builder.Services.AddTransient<Scanner>();

            builder.Services.AddTransient<PrincipalViewModel>();

            return builder.Build();
        }
    }
}