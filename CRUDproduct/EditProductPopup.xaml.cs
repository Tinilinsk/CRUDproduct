using CRUDproduct.Models;

namespace CRUDproduct;

public partial class EditProductPopup : ContentPage
{
    public TaskCompletionSource<Product> ResultTask { get; } = new();
    private Product _editingProduct;


    public EditProductPopup(Product product)
	{
		InitializeComponent();
        _editingProduct = product;

        NameEntry.Text = product.Name;
        PriceEntry.Text = product.Price.ToString("F2"); 
        CategoryEntry.Text = product.Category;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (!await ValidateInputs())
            return;

        var updatedProduct = new Product
        {
            Id = _editingProduct.Id,
            Name = NameEntry.Text,
            Price = decimal.Parse(PriceEntry.Text),
            Category = CategoryEntry.Text,
            CreatedDate = _editingProduct.CreatedDate
        };

        ResultTask.TrySetResult(updatedProduct);
        await Navigation.PopModalAsync();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        ResultTask.TrySetResult(null);
        Navigation.PopModalAsync();
    }

    private async Task<bool> ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Error", "Please enter product name", "OK");
            NameEntry.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(PriceEntry.Text))
        {
            await DisplayAlert("Error", "Please enter product price", "OK");
            PriceEntry.Focus();
            return false;
        }

        if (!decimal.TryParse(PriceEntry.Text, out decimal price))
        {
            await DisplayAlert("Error", "Price must be a valid number", "OK");
            PriceEntry.Focus();
            return false;
        }

        if (price < 0)
        {
            await DisplayAlert("Error", "Price cannot be negative", "OK");
            PriceEntry.Focus();
            return false;
        }

        return true;
    }
}