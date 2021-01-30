using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using uFrogio.Services;
using uFrogio.Views;

namespace uFrogio
{
    public partial class App : Application
    {

        public App()
        {
            Device.SetFlags(new[] { "SwipeView_Experimental" });

            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
