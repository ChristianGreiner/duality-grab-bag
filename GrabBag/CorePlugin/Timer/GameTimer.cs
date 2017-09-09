/*
    The MIT License (MIT)

    Copyright (c) 2015 Dylan Wilson

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Duality;
using System;

namespace ChristianGreiner.Duality.Plugins.GrabBag.Timer
{
    public abstract class GameTimer
    {
        protected GameTimer(double intervalSeconds)
            : this(TimeSpan.FromSeconds(intervalSeconds))
        {
        }

        protected GameTimer(TimeSpan interval)
        {
            Interval = interval;
            Restart();
        }

        public TimeSpan Interval { get; set; }
        public TimeSpan CurrentTime { get; protected set; }
        public TimerState State { get; protected set; }

        public void Update()
        {
            if (State != TimerState.Started)
                return;

            CurrentTime += TimeSpan.FromMilliseconds(Time.MsPFMult * Time.TimeMult);
            OnUpdate();
        }

        public event EventHandler Started;

        public event EventHandler Stopped;

        public event EventHandler Paused;

        public void Start()
        {
            State = TimerState.Started;
            Started?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            State = TimerState.Stopped;
            CurrentTime = TimeSpan.Zero;
            OnStopped();
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Pause()
        {
            State = TimerState.Paused;
            Paused?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void OnStopped();

        protected abstract void OnUpdate();
    }
}