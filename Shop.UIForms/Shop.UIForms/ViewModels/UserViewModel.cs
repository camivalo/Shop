using Shop.Common.Models;
using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Shop.UIForms.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRefreshing;
        private List<User> myUsers;
        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get => this.users;
            set => this.SetValue(ref this.users, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetValue(ref this.isRefreshing, value);
        }

        public UserViewModel()
        {
            this.apiService = new ApiService();
            this.LoadUsers();
        }

        private async void LoadUsers()
        {
            this.IsRefreshing = true;

            //var mainViewModel = MainViewModel.GetInstance();
            //var userNow = mainViewModel.UserEmail;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetAllUserAsync(
                url,
                "/api",
                "/Account/GetAllUsersAsync",
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

            this.myUsers = (List<User>)response.Result;
            this.RefresUsersList();
        }

        private void RefresUsersList()
        {
            this.Users = new ObservableCollection<User>(myUsers.Select(u => new User
            {
                Id = u.Id,
              FirstName = u.FullName,
              Email = u.Email,
              Address = u.Address,
              PhoneNumber = u.PhoneNumber

            })
            .OrderBy(u => u.FirstName)
            .ToList());
        }
    }
}
