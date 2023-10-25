using LeitorControladoShopping.ViewModels;
using LeitorControladoShopping.views;
using Telerik.Maui.Controls.Compatibility;

namespace LeitorControladoShopping
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseTelerik()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<Principal>();
            builder.Services.AddTransient<Scanner>();

            builder.Services.AddTransient<PrincipalViewModel>();

            return builder.Build();
        }
    }
}