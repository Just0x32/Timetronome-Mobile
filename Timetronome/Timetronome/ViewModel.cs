using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Timetronome.Services;

namespace Timetronome
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Model model;

        public string TempoText { get => $"Tempo ({model.TempoMinValue}...{model.TempoMaxValue})"; }
        public int SettedTempo { get => model.SettedTempo; private set => OnPropertyChanged(); }

        public string TimerText { get => $"Timer ({model.TimerMinValue}...{model.TimerMaxValue} min)"; }
        public int SettedTimer { get => model.SettedTimer; private set => OnPropertyChanged(); }

        public bool IsRunnedMetronome { get => model.IsRunningMetronome; }

        public string StateButtonText
        {
            get
            {
                if (!IsRunnedMetronome)
                {
                    return "Start";
                }
                else
                {
                    return $"Estimate time: {model.EstimateTime} min";
                }
            }
            private set => OnPropertyChanged();
        }

        internal ViewModel(IAudio audioService)
        {
            model = new Model(audioService);
            model.PropertyChanged += PropertyChangedNotifying;
        }

        internal void ToogleMetronomeState(int tempo, int timer) => model.ToogleMetronomeState(tempo, timer);

        internal void OnSleepingApp() => model.OnSleepingApp();

        internal void OnResumingApp() => model.OnResumingApp();

        private void PropertyChangedNotifying(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(model.SettedTempo))
                SettedTempo = default;

            if (e.PropertyName == nameof(model.SettedTimer))
                SettedTimer = default;

            if (e.PropertyName == nameof(model.IsRunningMetronome) || e.PropertyName == nameof(model.EstimateTime))
                StateButtonText = string.Empty;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
