using GalaSoft.MvvmLight.Command;
using Shop.Common.Models;
using Shop.Common.Services;
using Shop.UIForms.Helpers;
using Shop.UIForms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class OrderDetailItemViewModel : OrderDetailTemp
    {
        public ICommand DeleteOrderItemCommand => new RelayCommand(this.DeleteOrderItem);
        //private bool isRunning;
        //private bool isEnabled;
        private readonly ApiService apiService;

        //public bool IsRunning
        //{
        //    get => this.isRunning;
        //    set => this.SetValue(ref this.isRunning, value);
        //}

        //public bool IsEnabled
        //{
        //    get => this.isEnabled;
        //    set => this.SetValue(ref this.isEnabled, value);
        //}

        public OrderDetailItemViewModel()
        {
            this.apiService = new ApiService();
        }

        public async void DeleteOrderItem()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert(Languages.Confirm, Languages.AreYouToDeleteProduct, Languages.Yes, Languages.No);
            if (!confirm)
            {
                return;
            }


            ////this.IsRunning = true;
            ////this.IsEnabled = false;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.DeleteOrderItemAsync(
                url,
                "/api",
                "/Orders",
                this.Id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            //this.IsRunning = false;
            //this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, Languages.Accept);
                return;
            }

            MainViewModel.GetInstance().ShowProductReserved.DeleteOrderInList(this.Id);
            await App.Navigator.PushAsync(new ShowProductReservedPage());
            //await App.Navigator.PopAsync();

            //var d = (OrderDetailTemp)this;
            //var e = d.Id;
            //var i = this.Id;
            //var d = (OrderDetailTemp)this;
            //var f1 = d.Product.Id;
            //var n1 = d.Product.Name;
            //var f = this.Product.Id;
            //var n = this.Product.Name;
        }
    }
}
