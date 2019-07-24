namespace Shop.UIForms.ViewModels
{
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Shop.UIForms.Helpers;
    using Shop.UIForms.Views;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        private bool isRunning;
        private bool isEnabled;
        private bool isVisible;
        private readonly ApiService apiService;
        private List<OrderDetailTemp> myOrderDetail;

        public OrderDetailTemp orderDetail2 { get; set; }

        public string Quantity { get; set; }

        public Product Product { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public bool IsVisible
        {
            get => this.isVisible;
            set => this.SetValue(ref this.isVisible, value);
        }

        public ICommand SaveCommand => new RelayCommand(this.Save);

        public ICommand DeleteCommand => new RelayCommand(this.Delete);

        public ICommand ReservedCommand => new RelayCommand(this.Reserved);

       

        public EditProductViewModel(Product product)
        {
            var mainViewModel = MainViewModel.GetInstance();
            var userAdmin = mainViewModel.User.IsAdmin;
            this.IsVisible = userAdmin;

            this.Product = product;
            this.apiService = new ApiService();
            //this.Quantity = 0;
            this.IsEnabled = true;
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Product.Name))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.EnterProductName, Languages.Accept);
                return;
            }

            if (this.Product.Price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceNumber, Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api",
                "/Products",
                this.Product.Id,
                this.Product,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var modifiedProduct = (Product)response.Result;
            MainViewModel.GetInstance().Products.UpdateProductInList(modifiedProduct);
            await App.Navigator.PopAsync();
        }

        private async void Delete()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.AreYouToDeleteProduct, Languages.Yes, Languages.No);
            if (!confirm)
            {
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.DeleteAsync(
                url,
                "/api",
                "/Products",
                this.Product.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, Languages.Accept);
                return;
            }

            MainViewModel.GetInstance().Products.DeleteProductInList(this.Product.Id);
            await App.Navigator.PopAsync();
        }


        private async void Reserved()
        {
            if ((string.IsNullOrEmpty(this.Quantity)) || !(int.Parse(this.Quantity) > 0)  )
            {
                await Application.Current.MainPage.DisplayAlert(
                   "Error",
                   Languages.EnterQuantity,
                   Languages.Accept);
                return;
            }

            var confirm = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.AreYouSureSeparate, Languages.Yes, Languages.No);

            if (!confirm)
            {
                return;
            }




            var mainViewModel = MainViewModel.GetInstance();
            var userNow = mainViewModel.User;

            this.orderDetail2 = new OrderDetailTemp()
            {

                Product = this.Product,
                Price = this.Product.Price,
                Quantity = int.Parse(this.Quantity),
                User = userNow

            };


            if (this.myOrderDetail == null)
            {
                this.myOrderDetail = new List<OrderDetailTemp>();
            }

            this.myOrderDetail.Add(orderDetail2);

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutOrderTempAsync(
                url,
                "/api",
                "/Orders/PutOrderInToOrderTemp",
                myOrderDetail,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, Languages.Accept);
                return;
            }
            else
            {
                //await Application.Current.MainPage.DisplayAlert("Info", "your product was added successfully", "Accept");

                var confirm2 = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.ProductAdded, Languages.GoToList, Languages.AddMore);

                if (!confirm2)
                {

                    await App.Navigator.PopAsync();
                    //return;
                }
                else
                {
                    MainViewModel.GetInstance().ShowProductReserved = new ShowProductReservedViewModel();
                    await App.Navigator.PushAsync(new ShowProductReservedPage());
                }

                
            }

           
               
               
            

            //this.RefresOrdersList();


        }

      
    }

}
