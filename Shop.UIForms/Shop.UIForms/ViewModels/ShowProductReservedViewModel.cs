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
    public class ShowProductReservedViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private ObservableCollection<OrderDetailItemViewModel> orderDetailtemp;
        private List<OrderDetailTemp> myOrdersDetailTemp;
        private bool isRefreshing;


        public ObservableCollection<OrderDetailItemViewModel> OrderDetailtemp
        {
            get => this.orderDetailtemp;
            set => this.SetValue(ref this.orderDetailtemp, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public ShowProductReservedViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProductsReserved();
        }

        public ICommand BackToProductsCommand => new RelayCommand(this.BackToProducts);

        public ICommand ConfirmOrderCommand => new RelayCommand(this.ConfirmOrder);

        //public ICommand DeleteOrderItemCommand => new RelayCommand(this.DeleteOrderItem);


        private async void LoadProductsReserved()
        {
            this.IsRefreshing = true;
            var mainViewModel = MainViewModel.GetInstance();
            var userNow = mainViewModel.UserEmail;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetOrdersDetailTempByEmailAsync(
                url,
                "/api",
                "/Orders/GetOrdersDetailTempByEmail",
                userNow,
                "bearer",
                MainViewModel.GetInstance().Token.Token);


            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error!!",
                    response.Message,
                    Languages.Accept);
                return;
            }

            this.myOrdersDetailTemp = (List<OrderDetailTemp>)response.Result;
            this.RefresOrdersDetailTempList();
        }

        private void RefresOrdersDetailTempList()
        {
            this.OrderDetailtemp = new ObservableCollection<OrderDetailItemViewModel>(myOrdersDetailTemp.Select(o => new OrderDetailItemViewModel
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

        public async void BackToProducts()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel();
            await App.Navigator.PushAsync(new ProductsPage());
        }

        public async void ConfirmOrder()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.ConfirmOrder, Languages.Yes, Languages.No);
            if (!confirm)
            {
                return;
            }


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutOrderAsync(
                url,
                "/api",
                "/Orders/PutOrder",
                myOrdersDetailTemp,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, Languages.Accept);
                return;
            }

            await Application.Current.MainPage.DisplayAlert(Languages.Info, Languages.OrderAddedSuccessfully, Languages.Accept);

            MainViewModel.GetInstance().ShowProductReserved.LOadOrdersDetailTemp();


            await App.Navigator.PopAsync();

            
        }

        public void LOadOrdersDetailTemp()
        {
            LoadProductsReserved();
        }

        public void DeleteOrderInList(int orderId)
        {
            var previousOrder = this.myOrdersDetailTemp.Where(o => o.Id == orderId).FirstOrDefault();
            if (previousOrder != null)
            {
                this.myOrdersDetailTemp.Remove(previousOrder);
            }

            this.RefresOrdersDetailTempList();
        }

        //public async void DeleteOrderItem()
        //{
        //    var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure to delete this item", "Yes", "No");
        //    if (!confirm)
        //    {
        //        return;
        //    }
        //}


    }
}
