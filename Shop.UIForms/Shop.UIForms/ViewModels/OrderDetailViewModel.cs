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
    public class OrderDetailViewModel : BaseViewModel
    {
        private ObservableCollection<OrderDetailItemViewModel> orderDetail;
        public Order Order { get; set; }
        private readonly ApiService apiService;
        private bool isEnabled;
        private bool isVisible;

        public bool IsVisible
        {
            get => this.isVisible;
            set => this.SetValue(ref this.isVisible, value);
        }

        public ICommand DispatchCommand => new RelayCommand(this.Dispatch);

        public ObservableCollection<OrderDetailItemViewModel> OrderDetail
        {
            get => this.orderDetail;
            set => this.SetValue(ref this.orderDetail, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }
        public OrderDetailViewModel(Order order)
        {
            var mainViewModel = MainViewModel.GetInstance();
            var userAdmin = mainViewModel.User.IsAdmin;
            this.IsVisible = userAdmin;
            this.Order = order;
            //this.LoadDetails(Order.Items);
            this.LoadDetails(Order.Id);
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        public async void LoadDetails(int id)
        {
            var api = new ApiService();
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await api.GetOrderDetailById(
                url,
                "/api",
                "/Orders",
                id,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var items = (List<OrderDetail>)response.Result;






            this.OrderDetail = new ObservableCollection<OrderDetailItemViewModel>(items.Select(o => new OrderDetailItemViewModel
            {
                Id = o.Id,
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity,
                Value = o.Value
                //User = o.User,
                //OrderDate = o.OrderDate,
                //Items = o.Items,
                //Lines = o.Lines,
                //Quantity = o.Quantity,
                //Value = o.Value,

            })
             .ToList());
        }

        public async void Dispatch()
        {
            var idOrder = this.Order.Id;
            var deliveryDate = DateTime.Now;
            Order.DeliveryDate = deliveryDate;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.PutAsync(
                url,
                "/api",
                "/Orders",
                idOrder,
                this.Order,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var modifiedOrder = (Order)response.Result;
            MainViewModel.GetInstance().Orders.UpdateOrderInList(modifiedOrder);
            await App.Navigator.PopAsync();
           
        }
    }
}
