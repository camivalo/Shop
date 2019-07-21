using GalaSoft.MvvmLight.Command;
using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class SelectProductOrderViewModel : BaseViewModel
    {

        private readonly ApiService apiService;
        private List<Product> myProducts;
        private List<OrderDetail> myOrderDetail;
        private Product product;
        private ObservableCollection<Product> products;

        private ObservableCollection<OrderDetail> orderDetail;

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

        public ObservableCollection<Product> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public OrderDetail orderDetail2 { get; set; }
        public ObservableCollection<OrderDetail> OrderDetail
        {
            get => this.orderDetail;
            set => this.SetValue(ref this.orderDetail, value);
        }

        public SelectProductOrderViewModel()
        {
            
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        public  void ProductReserved()
        {
            //var p = this.product;
            //var q = this.Quantity;

            this.orderDetail2 = new OrderDetail()
            {
                
                Product = this.product,
                Price = this.product.Price,
                Quantity = this.Quantity
            };

            // ********VALDAR ESTA LINEA PARA VER SI VA GUARDANDO CADA REGISTRO
            //this.OrderDetail == null ? new ObservableCollection<OrderDetail>() : this.OrderDetail;
            //this.OrderDetail.Add(orderDetail2);
            if(this.myOrderDetail == null)
            {
                this.myOrderDetail = new List<OrderDetail>();
            }
           
            this.myOrderDetail.Add(orderDetail2);


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
                    "Accept");
                return;
            }

            this.myProducts = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(myProducts);
            //this.RefresProductsList();
        }

        //private void RefresProductsList()
        //{
        //    this.Products = new ObservableCollection<ProductItemViewModel>(myProducts.Select(p => new ProductItemViewModel
        //    {
        //        Id = p.Id,
        //        ImageUrl = p.ImageUrl,
        //        ImageFullPath = p.ImageFullPath,
        //        IsAvailabe = p.IsAvailabe,
        //        LastPurchase = p.LastPurchase,
        //        LastSale = p.LastSale,
        //        Name = p.Name,
        //        Price = p.Price,
        //        Stock = p.Stock,
        //        User = p.User
        //    })
        //    .OrderBy(p => p.Name)
        //    .ToList());
        //}
    }
}
