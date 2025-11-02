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
    }

}
