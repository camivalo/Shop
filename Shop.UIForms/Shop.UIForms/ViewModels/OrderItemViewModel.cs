using GalaSoft.MvvmLight.Command;
using Shop.Common.Models;
using Shop.UIForms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Shop.UIForms.ViewModels
{
    public class OrderItemViewModel : Order
    {
        public ICommand SelectOrderCommand => new RelayCommand(this.SelectOrder);

        private async void SelectOrder()
        {
            MainViewModel.GetInstance().OrderDetail = new OrderDetailViewModel((Order)this);
            await App.Navigator.PushAsync(new OrderDetailPage());
        }
    }
}
