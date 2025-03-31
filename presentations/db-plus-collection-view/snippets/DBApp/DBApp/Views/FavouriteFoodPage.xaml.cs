using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DBApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteFoodPage : ContentPage
    {
        public FavouriteFoodPage()
        {
            InitializeComponent();
            BindingContext = new FavouriteFoodViewModel();
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing();
            if (BindingContext is FavouriteFoodViewModel viewModel)
            {
                viewModel.LoadDataCommand.Execute(null);
            }
        }
    }
}