using System.ComponentModel;
using System.Diagnostics;

namespace CRUDproduct
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }

        private async void AddProductClicked(object sender, EventArgs e)
        {
            await ShowInputProduct();
        }

        public async Task ShowInputProduct()
        {
            var popup = new AddProduct();

            await Navigation.PushModalAsync(popup);

            var (_nameProduct, _price, _category) = await popup.ResultTask.Task;

            if (!string.IsNullOrEmpty(_nameProduct) && !string.IsNullOrEmpty(_price) && !string.IsNullOrEmpty(_category))
            {
                Name.Text = _nameProduct;
            }
        }
    }

}
