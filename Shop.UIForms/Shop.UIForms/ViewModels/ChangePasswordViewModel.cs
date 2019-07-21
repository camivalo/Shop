using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UIForms.ViewModels
{
    using System.Windows.Input;
    using Common.Models;
    using Common.Services;
    using GalaSoft.MvvmLight.Command;
    using Shop.Common.Helpers;
    using Shop.UIForms.Helpers;
    using Xamarin.Forms;

    public class ChangePasswordViewModel : BaseViewModel
    {
        private readonly ApiService apiService;
        private bool isRunning;
        private bool isEnabled;

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirm { get; set; }

        public bool IsRunning
        {
            get => this.isRunning;
            set => this.SetValue(ref this.isRunning, value);
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetValue(ref this.isEnabled, value);
        }

        public ICommand ChangePasswordCommand => new RelayCommand(this.ChangePassword);

        public ChangePasswordViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(this.CurrentPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EnterCurrentPassword,
                    Languages.Accept);
                return;
            }

            if (!MainViewModel.GetInstance().UserPassword.Equals(this.CurrentPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CurrentPasswordIncorrect,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EnterNewPassword,
                    Languages.Accept);
                return;
            }

            if (this.NewPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LengthPassword,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordConfirm,
                    Languages.Accept);
                return;
            }

            if (!this.NewPassword.Equals(this.PasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordConfirmNoMatch,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var request = new ChangePasswordRequest
            {
                Email = MainViewModel.GetInstance().UserEmail,
                NewPassword = this.NewPassword,
                OldPassword = this.CurrentPassword
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.ChangePasswordAsync(
                url,
                "/api",
                "/Account/ChangePassword",
                request,
                "bearer",
                MainViewModel.GetInstance().Token.Token);

            this.IsRunning = false;
            this.IsEnabled = true;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    Languages.Accept);
                return;
            }

            MainViewModel.GetInstance().UserPassword = this.NewPassword;
            Settings.UserPassword = this.NewPassword;

            await Application.Current.MainPage.DisplayAlert(
                "Ok",
                response.Message,
                Languages.Accept);

            await App.Navigator.PopAsync();
        }
    }

}
