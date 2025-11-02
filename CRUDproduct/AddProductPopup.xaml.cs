namespace CRUDproduct;

public partial class AddProduct : ContentPage
{
	public TaskCompletionSource<(string, string, string)> ResultTask { get; } = new();
	public AddProduct()
	{
		InitializeComponent();
	}

	void OnAddClicked(object sender, EventArgs e)
	{
		var result = (NameProduct.Text, Price.Text, Category.Text);
		ResultTask.TrySetResult(result);
		Navigation.PopModalAsync();
	}

	void OnCancelClicked(object sender, EventArgs e)
	{
        ResultTask.TrySetResult((null, null, null));
		Navigation.PopModalAsync();

    }
}