using GalaSoft.MvvmLight.Command;
using Shop.Common.Models;
using Shop.UIForms.Helpers;
using Shop.UIForms.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Shop.UIForms.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private static MainViewModel instance;

        private User user;

        public User User
        {
            get => this.user;
            set => this.SetValue(ref this.user, value);
        }


        //public User User { get; set; }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public TokenResponse Token { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public LoginViewModel Login { get; set; }

        public ProductsViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public RegisterViewModel Register { get; set; }

        public ProfileViewModel Profile { get; set; }

        public RememberPasswordViewModel RememberPassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        //LO NUEVO *****************************************************
        public OrdersViewModel Orders { get; set; }

        public AboutViewModel About { get; set; }

        public AddOrderViewModel AddOrder { get; set; }

        public ICommand AddOrderCommand => new RelayCommand(this.GoAddOrder);

        private async void GoAddOrder()
        {
            this.AddOrder = new AddOrderViewModel();
            await App.Navigator.PushAsync(new AddOrderPage());
        }

        public SelectProductOrderViewModel SelectProductOrder { get; set; }

        public OrderDetailViewModel OrderDetail { get; set; }

        public ShowProductReservedViewModel ShowProductReserved { get; set; }

        public ICommand ShowProductReservedCommand => new RelayCommand(this.ShowReservedProduct);

        private async void ShowReservedProduct()
        {
            this.ShowProductReserved = new ShowProductReservedViewModel();
            await App.Navigator.PushAsync(new ShowProductReservedPage());
        }

        public UserViewModel UserTotal { get; set; }

        //LO NUEVO *****************************************************

        public ICommand AddProductCommand => new RelayCommand(this.GoAddProduct);





        public MainViewModel()
        {
            instance = this;
            this.LoadMenus(false);
        }

        private async void GoAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await App.Navigator.PushAsync(new AddProductPage());
        }


        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        public void LoadMenusAdmin(bool isadmin)
        {
            this.LoadMenus(isadmin);
        }

        private void LoadMenus(bool isadmin)
        {
            var menus = new List<Menu>
    {
        new Menu
        {
            Icon = "ic_info",
            PageName = "AboutPage",
            Title = Languages.About
        },


        new Menu
        {
            Icon = "ic_person",
            PageName = "ProfilePage",
            Title = Languages.ModifyUser
        },
        //LO NUEVO *****************************************************
         new Menu
        {
            Icon = "ic_add_shopping_cart",
            PageName = "OrdersPage",
            Title = Languages.SeparateProduct
        },
        //LO NUEVO *****************************************************
        new Menu
        {
            Icon = "ic_phonelink_setup",
            PageName = "SetupPage",
            Title = Languages.Setup
        },

        
    };

            if (isadmin)
            {

                menus.Add(new Menu
                {
                    Icon = "ic_group",
                    PageName = "UserPage",
                    Title = Languages.Clients
                });

            };

            menus.Add(new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.CloseSession
            });

            this.Menus = new ObservableCollection<MenuItemViewModel>(menus.Select(m => new MenuItemViewModel
            {
                Icon = m.Icon,
                PageName = m.PageName,
                Title = m.Title
            }).ToList());
        }

    }
}
