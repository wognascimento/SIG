using LeitorControladoShopping.views;

namespace LeitorControladoShopping;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(Principal), typeof(Principal));
        Routing.RegisterRoute(nameof(Scanner), typeof(Scanner));
    }
}