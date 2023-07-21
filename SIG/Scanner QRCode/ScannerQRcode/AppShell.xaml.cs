using ScannerQRcode.Views;

namespace ScannerQRcode;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ReaderCargaShopping), typeof(ReaderCargaShopping));
        Routing.RegisterRoute(nameof(ReaderEnderecamento), typeof(ReaderEnderecamento));
        Routing.RegisterRoute(nameof(ReaderPreConferencia), typeof(ReaderPreConferencia));
        Routing.RegisterRoute(nameof(LookupCargaShopping), typeof(LookupCargaShopping));
    }
}
