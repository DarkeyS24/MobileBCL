namespace MobileBCL.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private void OnClickToLoginPage(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Login");
    }

    private void OnClickToProductsPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Products");
    }
}