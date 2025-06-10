using System.Text;
using System.Text.Json;
using MobileBCL.Models;

namespace MobileBCL.Views;

public partial class RegisterPage : ContentPage
{
    private User_Login user;
    private HttpClient httpClient;
	public RegisterPage()
	{
		InitializeComponent();
        user = new User_Login();
        httpClient = new HttpClient() { BaseAddress = new Uri("http://10.1.140.76:5254/api/") };
    }

    private void OnClickToLoginPage(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Login");
    }

    private async void OnClickToProductsPage(object sender, EventArgs e)
    {
        bool emptyFields = HaveEmptyField();
        if (emptyFields)
        {
            var content = new StringContent(JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNameCaseInsensitive = true}), Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync("Register", content);
                if (response.IsSuccessStatusCode)
                {
                    if (check.IsChecked)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<User_Login>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        Mailing_Subscription mailing = new Mailing_Subscription
                        {
                            login_id = result.login_id,
                            is_subscribed = true
                        };
                        var mailingContent = new StringContent(JsonSerializer.Serialize(mailing, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }), Encoding.UTF8, "application/json");
                        var mailingResponse = await httpClient.PostAsync("Subscribe", mailingContent);
                    }
                    await DisplayAlert("User save", "User successfully created", "OK");
                    Shell.Current.GoToAsync("//Products");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                return;
            }
        }
    }

    private bool HaveEmptyField()
    {
        if (string.IsNullOrEmpty(firstName.Text) || string.IsNullOrEmpty(lastName.Text))
        {
            DisplayAlert("Error", "Please fill in name field", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(email.Text))
        {
            DisplayAlert("Error", "Please fill in email field", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(pswd.Text) || string.IsNullOrEmpty(pswdConfirm.Text))
        {
            DisplayAlert("Error", "Please fill in password fields", "OK");
            return false;
        }
        else if (pswd.Text != pswdConfirm.Text)
        {
            DisplayAlert("Error", "Passwords do not match", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(question.Text) || string.IsNullOrEmpty(answer.Text))
        {
            DisplayAlert("Error", "Please fill in security question and answer fields", "OK");
            return false;
        }
        else
        {
            user.first_name = firstName.Text;
            user.last_name = lastName.Text;
            user.email = email.Text;
            user.password = pswd.Text;
            user.security_question = question.Text;
            user.security_answer = answer.Text;
            return true;
        }
    }
}