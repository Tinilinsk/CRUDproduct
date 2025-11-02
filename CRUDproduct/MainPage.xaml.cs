using CRUDproduct.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace CRUDproduct
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Product> _product = new();
        private int _nextId = 1;

        public ObservableCollection<Product> Products { get { return _product; } }
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void AddProductClicked(object sender, EventArgs e)
        {
            await ShowInputProduct();
        }

        public async Task ShowInputProduct()
        {
            var popup = new AddProduct();

            await Navigation.PushModalAsync(popup);

            var newProduct = await popup.ResultTask.Task;

            if (newProduct != null && !string.IsNullOrWhiteSpace(newProduct.Name))
            {
                newProduct.Id = _nextId++;
                newProduct.CreatedDate = DateTime.Now;

                _product.Add(newProduct);

            }
        }

        private async Task EditProduct(Product product)
        {
            var editPopup = new EditProductPopup(product);
            await Navigation.PushModalAsync(editPopup);

            var result = await editPopup.ResultTask.Task;
            if (result != null)
            {
                var index = _product.IndexOf(product);
                if (index != -1)
                {
                    _product[index] = result;
                    await DisplayAlert("Success", "Product updated successfully", "OK");
                }
            }
        }

        private async void OnMenuButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var product = (Product)button.CommandParameter;

            var action = await DisplayActionSheet(
                $"Actions for: {product.Name}",
                "Cancel",
                null,
                "✏️ Edit",
                "🗑️ Delete",
                "👁️ View Details",
                "📋 Copy Data");

            switch (action)
            {
                case "✏️ Edit":
                    await EditProduct(product);
                    break;
                case "🗑️ Delete":
                    await DeleteProduct(product);
                    break;
                case "👁️ View Details":
                    await ShowProductDetails(product);
                    break;
                case "📋 Copy Data":
                    await CopyProductData(product);
                    break;
            }
        }
        private async Task ShowProductDetails(Product product)
        {
            await DisplayAlert("Product Details",
                $"Name: {product.Name}\n" +
                $"Price: {product.Price:C}\n" +
                $"Category: {product.Category}\n" +
                $"ID: {product.Id}\n" +
                $"Created: {product.CreatedDate:MM/dd/yyyy}", "OK");
        }

        private async Task DeleteProduct(Product product)
        {
            bool answer = await DisplayAlert("Confirm Deletion",
                $"Are you sure you want to delete '{product.Name}'?", "Yes", "No");

            if (answer)
            {
                _product.Remove(product);
                await DisplayAlert("Success", "Product deleted", "OK");
            }
        }
        private async void OnSwipeDeleteInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var product = (Product)swipeItem.BindingContext;
            await DeleteProduct(product);
        }
        private async void OnSwipeEditInvoked(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var product = (Product)swipeItem.BindingContext;
            await EditProduct(product);
        }
        private async Task CopyProductData(Product product)
        {
            var text = $"{product.Name} - {product.Price:C} - {product.Category}";

            await Clipboard.Default.SetTextAsync(text);
            await DisplayAlert("Success", "Data copied to clipboard", "OK");
        }

    }

}
