using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Timetronome
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Model model = new Model();

        public string TempoText { get => $"Tempo ({model.TempoMinValue}...{model.TempoMaxValue})"; }
        public int TempoValue { get => model.TempoValue; private set => OnPropertyChanged(); }

        public string TimerText { get => $"Timer ({model.TimerMinValue}...{model.TimerMaxValue} min)"; }
        public int TimerValue { get => model.TimerValue; private set => OnPropertyChanged(); }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
