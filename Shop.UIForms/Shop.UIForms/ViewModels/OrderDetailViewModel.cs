using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Shop.UIForms.ViewModels
{
    public class OrderDetailViewModel : BaseViewModel
    {
        private ObservableCollection<OrderDetailItemViewModel> orderDetail;
        public Order Order { get; set; }
        private readonly ApiService apiService;
        private bool isEnabled;

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
            this.Order = order;
            this.LoadDetails(Order.Items);
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        public void LoadDetails(List<OrderDetail> Items)
        {
            this.OrderDetail = new ObservableCollection<OrderDetailItemViewModel>(Items.Select(o => new OrderDetailItemViewModel
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
    }
}
