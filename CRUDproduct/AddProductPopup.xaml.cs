namespace CRUDproduct;
using CRUDproduct.Models;
public partial class AddProduct : ContentPage
{
	public TaskCompletionSource<Product> ResultTask { get; } = new();
	public AddProduct()
	{
		InitializeComponent();
	}

	void OnAddClicked(object sender, EventArgs e)
	{
        var newProduct = new Product
        {
            Name = NameProduct.Text,
            Category = Category.Text,
            Price = decimal.TryParse(Price.Text, out decimal price) ? price : 0
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