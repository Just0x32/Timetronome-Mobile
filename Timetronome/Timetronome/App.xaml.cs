using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timetronome
{
    public partial class App : Application
    {
        MainPage mainPage;

        public App()
        {
            InitializeComponent();
            MainPage = mainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            mainPage.OnSleep();
        }

        protected override void OnResume()
        {
            mainPage.OnResume();
        }
    }
}
