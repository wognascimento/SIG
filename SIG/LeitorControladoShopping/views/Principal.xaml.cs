using LeitorControladoShopping.Data.Local;
using LeitorControladoShopping.ViewModels;

namespace LeitorControladoShopping.views;

public partial class Principal : ContentPage
{
    private readonly VolumeScannerRepository _volumeScannerRepository;

    public Principal(PrincipalViewModel viewModel, VolumeScannerRepository volumeScannerRepository)
	{
		InitializeComponent();
        _volumeScannerRepository = volumeScannerRepository;
        BindingContext = viewModel;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		try
		{
            PrincipalViewModel vm = (PrincipalViewModel)BindingContext;
            vm.VolumeControlados = await Task.Run(vm.GetVolumesAsync);

        }
		catch (Exception ex)
		{
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}