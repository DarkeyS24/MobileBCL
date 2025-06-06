using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using MobileBCL.Models;

namespace MobileBCL.Views;

public partial class ForgotPasswordPage : ContentPage
{
	private HttpClient httpClient;
	private User_Login newUser;
	public ForgotPasswordPage()
	{
		InitializeComponent();
		httpClient = new HttpClient() { BaseAddress = new Uri("http://192.168.35.111:5254/api/") };
		newUser = new User_Login();
	}

    private async void OnUnfocusGetSecurityQuestion(object sender, FocusEventArgs e)
    {
		if (EmailValidation(email.Text))
		{
			JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
			User_Login user = new User_Login();
			user.email = email.Text;
			var json = JsonSerializer.Serialize(user,options);
			try
			{
                var response = await httpClient.PostAsync("Forgot-password", new StringContent(json, Encoding.UTF8, "application/json"));
				Console.WriteLine(response.ToString());
				
                if (response.IsSuccessStatusCode)
				{ 
                    string newUser1 = await response.Content.ReadAsStringAsync();
					var UserLogin = JsonSerializer.Deserialize<User_Login>(newUser1, options);
                    newUser = UserLogin;
                    if (UserLogin != null && !string.IsNullOrEmpty(UserLogin.security_question))
                    {
                        question.Text = UserLogin.security_question;
                    }
                }
                else
                {
                    DisplayAlert("Invalid email", "Email not found.", "OK");
                }
            }catch(Exception ex)
			{
                Console.WriteLine($"Error: {ex.Message}");
            }
			
        }
    }

	private bool EmailValidation(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return false;
		}

		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email;
		}
		catch
		{
			return false;
		}
	}

    private void OnClickToLoginPage(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Login");
	}

    private async void OnClickToVerifyAnswer(object sender, EventArgs e)
    {
		if (!string.IsNullOrEmpty(answer.Text))
		{
            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			User_Login user = new User_Login();
			user.login_id = newUser.login_id;
			user.security_answer = answer.Text;
            var json = JsonSerializer.Serialize(user, options);
            var response = await httpClient.PostAsync("Reset-password", new StringContent(json, Encoding.UTF8,"application/json"));
			if (response.IsSuccessStatusCode)
			{
				buttons1.IsVisible = false;
				email.IsReadOnly = true;
				answer.IsReadOnly = true;
				settingPassword.IsVisible = true;
			}
		}
    }

    private void OnClickToSavePassword(object sender, EventArgs e)
    {

    }
}