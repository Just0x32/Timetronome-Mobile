using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Timetronome.Services;

namespace Timetronome
{
    internal class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IAudio audioService;

        Thread clickerThread;

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

        private bool IsSleepingClickerThread { get; set; }

        private bool IsInterruptingClickerThread { get; set; } = false;

        private bool IsSleepingApp { get; set; } = false;

        internal Model(IAudio audioService)
        {
            this.audioService = audioService;
            audioService.Initialize("click.wav");
            SettedTempo = TempoDefaultValue;
            SettedTimer = TimerDefaultValue;
            OnResumingApp();
        }

        internal void OnResumingApp()
        {
            IsSleepingApp = false;
            clickerThread = new Thread(new ThreadStart(Clicker));
            clickerThread.Start();
        }

        internal void OnSleepingApp() => IsSleepingApp = true;

        internal void ToogleMetronomeState(int receivedTempo, int receivedTimer)
        {
            if (!IsRunningMetronome)
            {
                SettedTempo = receivedTempo;
                SettedTimer = receivedTimer;
                EstimateTime = SettedTimer;
                IsRunningMetronome = true;
                ClickerThreadInterrupt();
            }
            else
            {
                IsRunningMetronome = false;
                ClickerThreadInterrupt();
            }
        }

        private void Clicker()
        {
            Stopwatch stopwatch = new Stopwatch();

            while (!IsSleepingApp)
            {
                ClickerThreadWaiting();
                IsInterruptingClickerThread = false;
                int clickDelay = 60000 / SettedTempo;
                int clickCounter = 0;

                while (!IsSleepingApp && IsRunningMetronome && EstimateTime > 0)
                {
                    stopwatch.Start();
                    audioService.PlayTrack();
                    clickCounter++;

                    if (clickCounter >= SettedTempo)
                    {
                        EstimateTime--;
                        clickCounter = 0;
                    }

                    stopwatch.Stop();

                    if (EstimateTime > 0)
                    {
                        int estimateDelay = 0;

                        if (stopwatch.ElapsedMilliseconds < clickDelay)
                            estimateDelay = clickDelay - (int)stopwatch.ElapsedMilliseconds;

                        ClickerThreadDelay(estimateDelay);
                        IsInterruptingClickerThread = false;
                    }

                    stopwatch.Reset();
                }

                IsRunningMetronome = false;
            }
        }

        private void ClickerThreadWaiting()
        {
            try
            {
                IsSleepingClickerThread = true;
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException) { }
            finally
            {
                IsSleepingClickerThread = false;
            }
        }

        private void ClickerThreadDelay(int delay)
        {
            try
            {
                IsSleepingClickerThread = true;
                Thread.Sleep(delay);
            }
            catch (ThreadInterruptedException) { }
            finally
            {
                IsSleepingClickerThread = false;
            }
        }

        private void ClickerThreadInterrupt()
        {
            if (IsSleepingClickerThread && !IsInterruptingClickerThread)
            {
                IsInterruptingClickerThread = true;
                clickerThread.Interrupt();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
