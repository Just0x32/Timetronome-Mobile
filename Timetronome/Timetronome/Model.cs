using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Timetronome
{
    internal class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Timer oneMinuteTimer;

        internal readonly int TempoMinValue = 20;
        internal readonly int TempoMaxValue = 300;
        private readonly int TempoDefaultValue = 120;
        internal readonly int TimerMinValue = 1;
        internal readonly int TimerMaxValue = 600;
        private readonly int TimerDefaultValue = 10;

        private int settedTempo;
        internal int SettedTempo
        {
            get => settedTempo;
            set
            {
                if (value < TempoMinValue)
                {
                    settedTempo = TempoMinValue;
                }
                else if (value > TempoMaxValue)
                {
                    settedTempo = TempoMaxValue;
                }
                else
                {
                    settedTempo = value;
                }

                OnPropertyChanged();
            }
        }

        private int settedTimer;
        internal int SettedTimer
        {
            get => settedTimer;
            set
            {
                if (value < TimerMinValue)
                {
                    settedTimer = TimerMinValue;
                }
                else if (value > TimerMaxValue)
                {
                    settedTimer = TimerMaxValue;
                }
                else
                {
                    settedTimer = value;
                }

                OnPropertyChanged();
            }
        }

        private int estimateTime;
        internal int EstimateTime
        {
            get => estimateTime;
            private set
            {
                estimateTime = value;
                OnPropertyChanged();
            }
        }

        private bool isRunningMetronome = false;
        internal bool IsRunningMetronome
        {
            get => isRunningMetronome;
            private set
            {
                isRunningMetronome = value;
                OnPropertyChanged();
            }
        }

        internal Model()
        {
            SettedTempo = TempoDefaultValue;
            SettedTimer = TimerDefaultValue;
        }

        internal async void ToogleMetronomeState(int receivedTempo, int receivedTimer)
        {
            if (!IsRunningMetronome)
            {
                SettedTempo = receivedTempo;
                SettedTimer = receivedTimer;
                EstimateTime = SettedTimer;
            }

            IsRunningMetronome = !IsRunningMetronome;

            if (IsRunningMetronome)
            {
                oneMinuteTimer = new Timer(60000);
                Timer returnedTimer;
                
                while (IsRunningMetronome && EstimateTime > 0)
                {
                    returnedTimer = await oneMinuteTimer.GetTimerAfterTheDelay();

                    if (returnedTimer == oneMinuteTimer)
                        EstimateTime--;
                    else
                        break;
                }

                if (!(EstimateTime > 0))
                    IsRunningMetronome = false;
            }
            else
            {
                oneMinuteTimer = null;
            }
        }

        private class Timer
        {
            internal int Delay { get; private set; }

            internal Timer(int delay) => Delay = delay;

            internal async Task<Timer> GetTimerAfterTheDelay()
            {
                await Task.Delay(Delay);
                return this;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
