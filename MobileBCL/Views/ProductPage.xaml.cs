using System.Text.Json;
using MobileBCL.Models;

namespace MobileBCL.Views;

public partial class ProductPage : ContentPage
{
	private HttpClient httpClient;
	private List<Product> productList;
    int star = 0;
	public ProductPage()
	{
		InitializeComponent();
        httpClient = new HttpClient() { BaseAddress = new Uri("http://10.1.140.76:5254/api/") };
		SetCategories();
		SetProducts();
    }

	private async void SetProducts()
	{
		try
		{
			var response = await httpClient.GetAsync("Products");
			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				var products = JsonSerializer.Deserialize<List<Product>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				productList = products;
                ProductsCollection.ItemsSource = products;
			}
		}
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return;
        }
	}

	private async void SetCategories()
	{
		try
		{
			var response = await httpClient.GetAsync("Categories");
					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						var categories = JsonSerializer.Deserialize<List<String>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						categoryPicker.ItemsSource = categories;
					}
		}
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            return;
        }
	}

    private void OnTapSetFavorite(object sender, TappedEventArgs e)
    {
		Image img = (Image)sender;
		if (star == 0)
		{
            img.Source = "218-star-full.png";
			star++;
		}
		else
		{
			img.Source = "216-star-empty.png";
			star--;
		}
		
    }

    private void OnClickToSearch(object sender, EventArgs e)
    {
		if (!string.IsNullOrEmpty(entryProduct.Text))
		{
            var newList = productList.Where(p => p.product_name.ToLower().Contains(entryProduct.Text.ToLower())).ToList();
            ProductsCollection.ItemsSource = newList;
			return;
        }
        ProductsCollection.ItemsSource = productList;
    }

    private void OnIndexChangeToFilter(object sender, EventArgs e)
    {
		var picker = (Picker)sender;
        var newList = productList.Where(p => p.category_id == picker.SelectedIndex + 1).ToList();
        ProductsCollection.ItemsSource = newList;
    }
}