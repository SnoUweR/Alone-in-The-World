using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AITW
{
    class Timer
    {
        private TimeSpan TimeSpan { get; set; }
        public TimeSpan CurrentTimeSpan;
        private TimeSpan _time;
        public TimeSpan Seconds { get; set; }
        private bool _init;

        public bool BTimerIsFinished;
        public bool Enabled = true;

        public delegate void SimpleEventHandler(object sender, EventArgs e);
        public event SimpleEventHandler TimerIsFinished;
        public event SimpleEventHandler TimerTick;
        public event SimpleEventHandler DoThatSecondsIsFinished;

        public enum TimerType
        {
            Millisecond,
            Second,
            FiveSeconds,
            TenSeconds,
            FifteenSeconds,
            Minute,
            Hour
        }

        public Timer(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
            CurrentTimeSpan = TimeSpan;
        }

        public Timer()
        {
            
        }

        public void DoThatSecondsInit(TimeSpan seconds)
        {
            Seconds = seconds;
            _init = true;
        }

        public bool DoThatSeconds(GameTime gameTime)
        {
            if (_init)
            {
                if (Seconds > TimeSpan.Zero)
                {
                    Seconds -= gameTime.ElapsedGameTime;
                }
                else
                {
                    if (DoThatSecondsIsFinished != null)
                    {
                        DoThatSecondsIsFinished(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                Seconds = TimeSpan.Zero;
            }

            return Seconds > TimeSpan.Zero;
        }

        public void Update(GameTime gameTime, TimerType timerType)
        {
            if (Enabled)
            {
                if (!BTimerIsFinished)
                {
                    CurrentTimeSpan -= gameTime.ElapsedGameTime;
                    _time += gameTime.ElapsedGameTime;

                    if (CurrentTimeSpan > TimeSpan.Zero)
                    {
                        switch (timerType)
                        {
                            case TimerType.Millisecond:
                                TimerTick(this, EventArgs.Empty);
                                break;
                            case TimerType.Second:
                                if (_time >= new TimeSpan(0, 0, 0, 1))
                                {
                                    TimerTick(this, EventArgs.Empty);
                                    _time = TimeSpan.Zero;
                                }
                                break;
                            case TimerType.FiveSeconds:
                                if (_time >= new TimeSpan(0, 0, 0, 5))
                                {
                                    TimerTick(this, EventArgs.Empty);
                                    _time = TimeSpan.Zero;
                                }
                                break;
                            case TimerType.TenSeconds:
                                if (_time >= new TimeSpan(0, 0, 0, 10))
                                {
                                    TimerTick(this, EventArgs.Empty);
                                    _time = TimeSpan.Zero;
                                }
                                break;
                            case TimerType.FifteenSeconds:
                                if (_time >= new TimeSpan(0, 0, 0, 15))
                                {
                                    TimerTick(this, EventArgs.Empty);
                                    _time = TimeSpan.Zero;
                                }
                                break;
                            case TimerType.Minute:
                                if (_time >= new TimeSpan(0, 0, 1, 0))
                                {
                                    TimerTick(this, EventArgs.Empty);
                                    _time = TimeSpan.Zero;
                                }
                                break;
                            case TimerType.Hour:
                                if (_time >= new TimeSpan(0, 1, 0, 0))
                                {
                                    TimerTick(this, EventArgs.Empty);
                                    _time = TimeSpan.Zero;
                                }
                                break;
                        }

                    }
                    else if (CurrentTimeSpan <= TimeSpan.Zero)
                    {
                        if(TimerIsFinished != null)
                        {
                            TimerIsFinished(this, EventArgs.Empty);
                        }
                        BTimerIsFinished = true;
                    }
                }
            }
        }
    }
}
