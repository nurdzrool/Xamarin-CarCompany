using System;

using Xamarin.Forms;

namespace CarCompany
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new CarColorPage());
        }
    }
}
