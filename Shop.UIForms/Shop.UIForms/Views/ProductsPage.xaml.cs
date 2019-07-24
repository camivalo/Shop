using Shop.UIForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shop.UIForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();
        }

        private void ProductSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            var products = mainViewModel.Products;

            List<ProductItemViewModel> productList = products.productList();

            var productSearched = productList.Where(p => p.Name.ToLower().Contains(ProductSearchBar.Text));
            ProductSearchList.ItemsSource = productSearched;
        }
    }
}