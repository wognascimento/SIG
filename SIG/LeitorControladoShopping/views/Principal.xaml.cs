using LeitorControladoShopping.ViewModels;

namespace LeitorControladoShopping.views;

public partial class Principal : ContentPage
{
	public Principal(PrincipalViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}