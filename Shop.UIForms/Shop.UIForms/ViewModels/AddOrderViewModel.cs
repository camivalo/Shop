using GalaSoft.MvvmLight.Command;
using Shop.Common.Models;
using Shop.Common.Services;
using Shop.UIForms.Helpers;
using Shop.UIForms.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class AddOrderViewModel : BaseViewModel
    {
        //public ICommand AddOrderCommand => new RelayCommand(this.AddOrder);

        //private async void AddOrder()
        //{
        //    MainViewModel.GetInstance().SelectProductOrder = new SelectProductOrderViewModel();
        //    await App.Navigator.PushAsync(new SelectProductOrderPage());
        //}


        private readonly ApiService apiService;
        private List<Product> myProducts;
        private List<OrderDetailTemp> myOrderDetail;
        private Product product;
        private ObservableCollection<Product> products;

        private ObservableCollection<OrderDetailTemp> orderDetail;

        public Product Product
        {
            get => this.product;
            set
            {
                this.SetValue(ref this.product, value);
                //this.Cities = new ObservableCollection<City>(this.Country.Cities.OrderBy(c => c.Name));
            }
        }

        public int Quantity { get; set; }

        


        public ICommand ProductReservedCommand => new RelayCommand(this.ProductReserved);
        public ICommand ConfirmOrderCommand => new RelayCommand(this.ConfirmOrder);

        public ObservableCollection<Product> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public OrderDetailTemp orderDetail2 { get; set; }
        public ObservableCollection<OrderDetailTemp> OrderDetail
        {
            get => this.orderDetail;
            set => this.SetValue(ref this.orderDetail, value);
        }

        public AddOrderViewModel()
        {

            this.apiService = new ApiService();
            this.Quantity = 0;
            this.LoadProducts();
        }

        public void ProductReserved()
        {

            //var p = this.product;
            //var q = this.Quantity;

            if (this.product == null)
            {
                Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SelectProduct,
                    Languages.Accept);
                return;
            }

            if (this.Quantity <= 0)
            {
                 Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EnterQuantity,
                    Languages.Accept);
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            var userNow = mainViewModel.User;

            this.orderDetail2 = new OrderDetailTemp()
            {

                Product = this.product,
                Price = this.product.Price,
                Quantity = this.Quantity,
                User = userNow

            };

      
            if (this.myOrderDetail == null)
            {
                this.myOrderDetail = new List<OrderDetailTemp>();
            }

            this.myOrderDetail.Add(orderDetail2);
            //****************** REFRESCAR DOPDOWN LIST Y ENTRY
            this.LoadProducts();

           

            //var MyEntry = new Entry();
            //entryQuantity.Text;
            //this.Quantity = Int32.Parse(MyEntry.Text);
            //****************** REFRESCAR DOPDOWN LIST Y ENTRY

            this.RefresOrdersList();
          
        }


        public async void ConfirmOrder()
        {
            //if (string.IsNullOrEmpty(this.Name))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product name.", "Accept");
            //    return;
            //}

            //if (string.IsNullOrEmpty(this.Price))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "You must enter a product price.", "Accept");
            //    return;
            //}

            //var price = decimal.Parse(this.Price);
            //if (price <= 0)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "The price must be a number greather than zero.", "Accept");
            //    return;
            //}

            //this.IsRunning = true;
            //this.IsEnabled = false;

            //byte[] imageArray = null;
            //if (this.file != null)
            //{
            //    imageArray = FilesHelper.ReadFully(this.file.GetStream());
            //}


            //var product = new Product
            //{
            //    IsAvailabe = true,
            //    Name = this.Name,
            //    Price = price,
            //    User = new User { UserName = MainViewModel.GetInstance().UserEmail },
            //    ImageArray = imageArray

            //};

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutOrderAsync(
                url,
                "/api",
                "/Orders/PutOrder",
                myOrderDetail,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            
            MainViewModel.GetInstance().Orders.LOadOrdersFromNewOrder();

          
            await App.Navigator.PopAsync();

            //var mainViewModel = MainViewModel.GetInstance();
            //mainViewModel.Orders = new OrdersViewModel();
            //await App.Navigator.PushAsync(new OrdersPage());

        }

            private void RefresOrdersList()
        {
            this.OrderDetail = new ObservableCollection<OrderDetailTemp>(myOrderDetail.Select(o => new OrderDetailTemp
            {
                Id = o.Id,
               Product = o.Product,
               Price = o.Price,
               Quantity = o.Quantity,
               Value = o.Value
               
              
            })
            .OrderBy(o => o.Product.Name)
            .ToList());
        }

        private async void LoadProducts()
        {
            //this.IsRefreshing = true;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
                "bearer",
                MainViewModel.GetInstance().Token.Token);


            //this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error!!",
                    response.Message,
                    Languages.Accept);
                return;
            }

            this.myProducts = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(myProducts);
            
        }

     
    }
}
