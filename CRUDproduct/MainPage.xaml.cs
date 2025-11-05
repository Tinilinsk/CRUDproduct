using CRUDproduct.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using CRUDproduct.DB;


namespace CRUDproduct
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Product> _product = new();
        private int _nextId = 1;
        private readonly CsvExportService _csvExportService;
        ProductDatabase database;

        public ObservableCollection<Product> Products { get { return _product; } }
        public int ProductsCount => _product.Count;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            _csvExportService = new CsvExportService();
            database = new ProductDatabase();

            _ = LoadProductsAsync();
        }

        public async Task<List<Product>> ShowProductFromDb()
        {
            return await database.GetItemsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var items = await database.GetItemsAsync();

                Debug.WriteLine($"📊 Получено записей: {items?.Count}");
                if (items != null && items.Count > 0)
                {
                    foreach (var item in items.Take(5))
                    {
                        Debug.WriteLine($"📦 Товар: {item.Name}, Цена: {item.Price}");
                    }
                } 
                else
                {
                    Debug.WriteLine("ℹ️ В базе нет записей");
                }
                foreach (Product item in items)
                {
                    _product.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Get products failed: {ex.Message}", "OK");
            }
        }
        private async void OnExportCsvClicked(object sender, EventArgs e)
        {
            try
            {

                var exportService = new CsvExportService();
                await exportService.ExportToCsvAsync(_product);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
            }
        }

        private async void AddProductClicked(object sender, EventArgs e)
        {
            await ShowInputProduct();
            
        }

        public async Task ShowInputProduct()
        {
            try
            {
                var popup = new AddProduct();
                await Navigation.PushModalAsync(popup);

                var newProduct = await popup.ResultTask.Task;

                if (newProduct != null && !string.IsNullOrWhiteSpace(newProduct.Name))
                {
                    newProduct.CreatedDate = DateTime.Now;

                    Debug.WriteLine($"ShowInputProduct: Saving product to database - {newProduct.Name}");

                    var saveResult = await database.SaveItemAsync(newProduct);

                    Debug.WriteLine($"ShowInputProduct: Database save result - {saveResult}");

                    if (saveResult > 0)
                    {
                        _product.Add(newProduct);

                        Debug.WriteLine($"ShowInputProduct: Product added successfully. ID: {newProduct.Id}");
                    }
                    else
                    {
                        Debug.WriteLine("ShowInputProduct: Failed to save product to database");
                    }
                }
                else
                {
                    Debug.WriteLine("ShowInputProduct: Product was not added (empty name or null)");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShowInputProduct: Error - {ex.Message}");
            }
        }

        private async Task ReloadProductsFromDatabase()
        {
            try
            {
                Debug.WriteLine("ReloadProductsFromDatabase: Refreshing list from database");

                _product.Clear();

                var productsFromDb = await database.GetItemsAsync();

                Debug.WriteLine($"ReloadProductsFromDatabase: Loaded {productsFromDb.Count} products");

                foreach (var product in productsFromDb)
                {
                    _product.Add(product);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ReloadProductsFromDatabase: Error - {ex.Message}");
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
                    try
                    {
                        _product[index] = result;
                        await database.SaveItemAsync(result);
                        await DisplayAlert("Success", "Product updated successfully", "OK");
                    } catch (Exception ex) {
                        Debug.WriteLine("Error with edit product: " + ex); 
                    }
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
                try
                {
                    _product.Remove(product);
                    await database.DeleteItemAsync(product);
                    await DisplayAlert("Success", "Product deleted", "OK");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error with delete: " + ex);
                }
                
                
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
