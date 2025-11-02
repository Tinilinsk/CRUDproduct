namespace CRUDproduct;
using CRUDproduct.Models;
public partial class AddProduct : ContentPage
{
	public TaskCompletionSource<Product> ResultTask { get; } = new();
	public AddProduct()
	{
		InitializeComponent();
	}

    async void OnAddClicked(object sender, EventArgs e)
	{
        if (string.IsNullOrWhiteSpace(NameProduct.Text))
        {
            await DisplayAlert("Error", "Please enter product name", "OK");
            NameProduct.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Price.Text))
        {
            await DisplayAlert("Error", "Please enter product price", "OK");
            Price.Focus();
            return;
        }

        if (!decimal.TryParse(Price.Text.Replace('.', ','), out decimal price) || price < 0)
        {
            await DisplayAlert("Error", "Please enter a valid positive number for price", "OK");
            Price.Text = "";
            Price.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Category.Text))
        {
            await DisplayAlert("Error", "Please enter category name", "OK");
            Category.Focus();
            return;
        }

        var newProduct = new Product
        {
            Name = NameProduct.Text,
            Category = Category.Text,
            Price = price
        };	

		ResultTask.TrySetResult(newProduct);
		Navigation.PopModalAsync();
	}

	void OnCancelClicked(object sender, EventArgs e)
	{
        ResultTask.TrySetResult(null);
		Navigation.PopModalAsync();

    }
}