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

            string result = await DisplayPromptAsync("Question 1", "What's your name?");
            Name.Text = result;
        }
    }

}
