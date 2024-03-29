﻿namespace Shop.UIForms.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Shop.Common.Models;
    using Shop.Common.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;


    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRefreshing;
        private List<Product> myProducts;
        private ObservableCollection<ProductItemViewModel> products;
        


        //public ICommand SeparateCommand => new RelayCommand(Separate);

        //private async void Separate()
        //{
        //    await Application.Current.MainPage.DisplayAlert(
        //          "Error",
        //          "You must enter an email",
        //          "Accept");
        //}

        public ObservableCollection<ProductItemViewModel> Products
        {
            get => this.products;
            set => this.SetValue(ref this.products, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            //******** SE HACE ACA LA VALIDACION DEL USUARIO YA QUE COMO LA 1ra PAG QUE SE CARGA ES LA DE PRODUCTOS NECESITAMOS QUE SE VALIDE SI EL USUARIO ES O NO ADMIN
            var mainViewModel = MainViewModel.GetInstance();
            var userAdmin = mainViewModel.User.IsAdmin;
            mainViewModel.LoadMenusAdmin(userAdmin);
            //******** SE HACE ACA LA VALIDACION DEL USUARIO YA QUE COMO LA 1ra PAG QUE SE CARGA ES LA DE PRODUCTOS NECESITAMOS QUE SE VALIDE SI EL USUARIO ES O NO ADMIN


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<Product>(
                url,
                "/api",
                "/Products",
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

            this.myProducts = (List<Product>)response.Result;
            this.RefresProductsList();
        }

        public void AddProductToList(Product product)
        {
            this.myProducts.Add(product);
            this.RefresProductsList();
        }

        public void UpdateProductInList(Product product)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == product.Id).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.myProducts.Add(product);
            this.RefresProductsList();
        }

        public void DeleteProductInList(int productId)
        {
            var previousProduct = this.myProducts.Where(p => p.Id == productId).FirstOrDefault();
            if (previousProduct != null)
            {
                this.myProducts.Remove(previousProduct);
            }

            this.RefresProductsList();
        }

        private void RefresProductsList()
        {
            this.Products = new ObservableCollection<ProductItemViewModel>(myProducts.Where(p => p.IsAvailabe).Select(p => new ProductItemViewModel
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                ImageFullPath = p.ImageFullPath,
                IsAvailabe = p.IsAvailabe,
                LastPurchase = p.LastPurchase,
                LastSale = p.LastSale,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                User = p.User
            })
            .OrderBy(p => p.Name)
            .ToList());
        }

        public List<ProductItemViewModel> productList()
        {
            return this.Products.ToList();
        }

    }
}
