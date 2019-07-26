using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class OrdersViewModel : BaseViewModel
    {

        private ObservableCollection<OrderItemViewModel> orders;
        private readonly ApiService apiService;
        private List<Order> myOrders;
        private bool isRefreshing;

        public string Email { get; set; }

        public ObservableCollection<OrderItemViewModel> Orders
        {
            get => this.orders;
            set => this.SetValue(ref this.orders, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public OrdersViewModel()
        {
            this.apiService = new ApiService();
            this.LoadOrders();
        }

        public void LOadOrdersFromNewOrder()
        {
            this.LoadOrders();
        }

        private async void LoadOrders()
        {
            this.IsRefreshing = true;

            var mainViewModel = MainViewModel.GetInstance();
            var userNow = mainViewModel.UserEmail;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetOrdersByEmailAsync(
                url,
                "/api",
                "/Orders/GetOrdersByEmail",
                userNow,
                "bearer",
                MainViewModel.GetInstance().Token.Token);


            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error!!",
                    response.Message,
                    "Accept");
                return;
            }

            this.myOrders = (List<Order>)response.Result;
            this.RefresOrdersList();
        }

        public DateTime? delivery(DateTime? d)
        {


            if (d != null)
            {
                d = d.Value.ToLocalTime();
            }


            return d;
        }

        private void RefresOrdersList()
        {



            this.Orders = new ObservableCollection<OrderItemViewModel>(myOrders.Select(o => new OrderItemViewModel
            {
                Id = o.Id,
                User = o.User,
                OrderDate = o.OrderDateLocal.Value,
                DeliveryDate = delivery(o.DeliveryDate),
                Items = o.Items,
                Lines = o.Lines,
                Quantity = o.Quantity,
                Value = o.Value,

            })
            .OrderByDescending(o => o.OrderDateLocal)
            .ToList());
        }

        public void UpdateOrderInList(Order order)
        {
            var previousProduct = this.myOrders.Where(o => o.Id == order.Id).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myOrders.Remove(previousProduct);
            }

            this.myOrders.Add(order);
            this.RefresOrdersList();
        }

        public void AddOrderToList(Order order)
        {
            this.myOrders.Add(order);
            this.RefresOrdersList();
        }
    }
}
