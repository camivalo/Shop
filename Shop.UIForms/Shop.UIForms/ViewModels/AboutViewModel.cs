using Shop.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UIForms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {

        private readonly ApiService apiService;
        public AboutViewModel()
        {
            this.apiService = new ApiService();
        }
    }
}
