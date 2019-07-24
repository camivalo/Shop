using Shop.Common.Models;
using Shop.UIForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shop.UIForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        private void UsuariosSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var mainViewModel = MainViewModel.GetInstance();
            var users = mainViewModel.UserTotal;

            List<User> userList = users.userList();

            var userSearched = userList.Where(c => c.FullName.ToLower().Contains(UsuariosSearchBar.Text));
            UsuariosSearchList.ItemsSource = userSearched;
        }
    }
}