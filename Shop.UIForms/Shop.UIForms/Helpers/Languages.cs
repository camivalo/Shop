using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UIForms.Helpers
{
    using Interfaces;
    using Resources;
    using Xamarin.Forms;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept => Resource.Accept;

        public static string Error => Resource.Error;

        public static string EmailError => Resource.EmailError;

        public static string PasswordError => Resource.PasswordError;

        public static string Confirm => Resource.Confirm;

        public static string ProductAdded => Resource.ProductAdded;

        public static string AddMore => Resource.AddMore;

        public static string GoToList => Resource.GoToList;

        public static string AreYouSureSeparate => Resource.AreYouSureSeparate;

        public static string Yes => Resource.Yes;

        public static string No => Resource.No;

        public static string EnterQuantity => Resource.EnterQuantity;

        public static string AreYouToDeleteProduct => Resource.AreYouToDeleteProduct;

        public static string EnterProductName => Resource.EnterProductName;

        public static string PriceNumber => Resource.PriceNumber;

        public static string SelectProduct => Resource.SelectProduct;

        public static string About => Resource.About;

        public static string CloseSession => Resource.CloseSession;

        public static string ModifyUser => Resource.ModifyUser;

        public static string SeparateProduct => Resource.SeparateProduct;

        public static string Setup => Resource.Setup;

        public static string EnterAddress => Resource.EnterAddress;

        public static string EnterFirstName => Resource.EnterFirstName;

        public static string EnterLastName => Resource.EnterLastName;

        public static string EnterPhone => Resource.EnterPhone;

        public static string SelectCountry => Resource.SelectCountry;

        public static string SelectCity => Resource.SelectCity;

        public static string UserUpdated => Resource.UserUpdated;

        public static string ConfirmOrder => Resource.ConfirmOrder;

        public static string Info => Resource.Info;

        public static string OrderAddedSuccessfully => Resource.OrderAddedSuccessfully;

        public static string CurrentPasswordIncorrect => Resource.CurrentPasswordIncorrect;

        public static string EnterCurrentPassword => Resource.EnterCurrentPassword;

        public static string EnterNewPassword => Resource.EnterNewPassword;

        public static string LengthPassword => Resource.LengthPassword;

        public static string PasswordConfirm => Resource.PasswordConfirm;

        public static string PasswordConfirmNoMatch => Resource.PasswordConfirmNoMatch;

        public static string EnterEmail => Resource.EnterEmail;

        public static string EnterValidEmail => Resource.EnterValidEmail;

        public static string AddOrders => Resource.AddOrders;

        public static string Product => Resource.Product;

        public static string EmailOrPasswordIncorrect => Resource.EmailOrPasswordIncorrect;

        public static string SelectAProduct => Resource.SelectAProduct;

        public static string Quantity => Resource.Quantity;

        public static string AddProductToList => Resource.AddProductToList;

        public static string ConfirmOrderXaml => Resource.ConfirmOrderXaml;

        public static string Price => Resource.Price;

        public static string Value => Resource.Value;

        public static string EnterQuantityXaml => Resource.EnterQuantityXaml;

        public static string ChangePassword => Resource.ChangePassword;

        public static string CurrentPassword => Resource.CurrentPassword;

        public static string EnterYourCurrentPassword => Resource.EnterYourCurrentPassword;

        public static string NewPassword => Resource.NewPassword;

        public static string EnterTheNewPassword => Resource.EnterTheNewPassword;

        public static string ConfirmNewPassword => Resource.ConfirmNewPassword;

        public static string RenterTheNewPassword => Resource.RenterTheNewPassword;

        public static string BookProducts => Resource.BookProducts;

        public static string Login => Resource.Login;

        public static string Email => Resource.Email;

        public static string EnterYourEmail => Resource.EnterYourEmail;

        public static string EnterYourPassword => Resource.EnterYourPassword;

        public static string RememberInThisDevice => Resource.RememberInThisDevice;

        public static string ForgotMyPassword => Resource.ForgotMyPassword;

        public static string Register => Resource.Register;

        public static string Password => Resource.Password;
        public static string ReservationDetail => Resource.ReservationDetail;
        public static string Reservations => Resource.Reservations;
        public static string NameUser => Resource.NameUser;
        public static string OrderDate => Resource.OrderDate;
        public static string Lines => Resource.Lines;
        public static string Products => Resource.Products;
        public static string FirstName => Resource.FirstName;
        public static string EnterYourFirstName => Resource.EnterYourFirstName;
        public static string LastName => Resource.LastName;
        public static string EnterYourLastName => Resource.EnterYourLastName;
        public static string Country => Resource.Country;
        public static string SelectACountry => Resource.SelectACountry;
        public static string City => Resource.City;
        public static string SelectACity => Resource.SelectACity;
        public static string Address => Resource.Address;
        public static string EnterYourAddress => Resource.EnterYourAddress;
        public static string Phone => Resource.Phone;
        public static string EnterYourPhoneNumber => Resource.EnterYourPhoneNumber;
        public static string Save => Resource.Save;
        public static string ModifyPassword => Resource.ModifyPassword;
        public static string RegisterNewUser => Resource.RegisterNewUser;
        public static string PasswordConfirmXaml => Resource.PasswordConfirmXaml;
        public static string EnterYourPasswordConfirm => Resource.EnterYourPasswordConfirm;
        public static string SelectProductXaml => Resource.SelectProductXaml;
        public static string ProductsReserved => Resource.ProductsReserved;
        public static string AddMoreProducts => Resource.AddMoreProducts;

        public static string Clients => Resource.Clients;


    }

}
