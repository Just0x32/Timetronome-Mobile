using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Timetronome
{
    public partial class MainPage : ContentPage
    {
        private ViewModel viewModel = new ViewModel();

        public MainPage()
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}
