using System;
using Timetronome.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Timetronome
{
    public partial class MainPage : ContentPage
    {
        private ViewModel viewModel;
        private IAudio audioService;

        public MainPage()
        {
            audioService = DependencyService.Get<IAudio>();
            viewModel = new ViewModel(audioService);
            BindingContext = viewModel;
            InitializeComponent();
            DeviceDisplay.KeepScreenOn = true;
        }

        internal void OnSleep() => viewModel.OnSleepingApp();

        internal void OnResume() => viewModel.OnResumingApp();

        private void MetronomeStateButton_Clicked(object sender, EventArgs e)
        {
            int.TryParse(TempoEntry.Text, out int tempo);
            int.TryParse(TimerEntry.Text, out int timer);

            viewModel.ToogleMetronomeState(tempo, timer);
        }
    }
}
