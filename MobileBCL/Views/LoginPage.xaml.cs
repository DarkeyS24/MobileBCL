using System.Text.Json;

namespace MobileBCL.Views;

public partial class LoginPage : ContentPage
{
    private HttpClient httpClient;
	public LoginPage()
	{
		InitializeComponent();
        httpClient = new HttpClient() { BaseAddress = new Uri("http://10.1.140.76:5254/api/") };
	}

    private void OnTapGoToForgotThePasswordPage(object sender, TappedEventArgs e)
    {
		Shell.Current.GoToAsync("//ForgotPassword");
        link.TextColor = Colors.Purple;
    }

    private async void OnClickToRegisterPage(object sender, EventArgs e)
    {
                Shell.Current.GoToAsync("//Register");

    }

    private async void OnClickToProductsPage(object sender, EventArgs e)
    {
        //Shell.Current.GoToAsync("//Products");
        ValidateUser user = new ValidateUser();
        if (!string.IsNullOrEmpty(email.Text) || !string.IsNullOrEmpty(password.Text))
        {
            user.email = email.Text;
            user.password = password.Text;

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var json = JsonSerializer.Serialize(user, options);
            try
            {
                var response = await httpClient.PostAsync("Login", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    Shell.Current.GoToAsync("//Products");
                }
                else
                {
                    DisplayAlert("Invalid User", "Invalid login", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            DisplayAlert("Empty field", "Please enter email and password", "OK");
        }
    }

    public class ValidateUser
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}