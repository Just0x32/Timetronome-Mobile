using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Timetronome
{
    internal class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal readonly int TempoMinValue = 20;
        internal readonly int TempoMaxValue = 300;
        private readonly int TempoDefaultValue = 120;
        internal readonly int TimerMinValue = 1;
        internal readonly int TimerMaxValue = 600;
        private readonly int TimerDefaultValue = 10;

        private int tempoValue;
        internal int TempoValue
        {
            get => tempoValue;
            set
            {
                if (value < TempoMinValue)
                {
                    tempoValue = TempoMinValue;
                }
                else if (value > TempoMaxValue)
                {
                    tempoValue = TempoMaxValue;
                }
                else
                {
                    tempoValue = value;
                }

                OnPropertyChanged();
            }
        }

        private int timerValue;
        internal int TimerValue
        {
            get => timerValue;
            set
            {
                if (value < TimerMinValue)
                {
                    timerValue = TimerMinValue;
                }
                else if (value > TimerMaxValue)
                {
                    timerValue = TimerMaxValue;
                }
                else
                {
                    timerValue = value;
                }

                OnPropertyChanged();
            }
        }

        internal Model()
        {
            TempoValue = TempoDefaultValue;
            TimerValue = TimerDefaultValue;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
