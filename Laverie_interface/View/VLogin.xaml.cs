using Laverie_interface.ViewModel;

namespace Laverie_interface.View;

public partial class VLogin : ContentPage
{
	public VLogin()
	{
		InitializeComponent();
		BindingContext = new VMLogin();
	}
}