using MonoEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSocCar.Utilities
{
    public class TimeKeeper
    {
        public class Timer
        {
            private float timePerFrame;
            private Action onComplete;

            public int FramesRemaining { get; private set; }

            public float SecondsRemaining
            {
                get
                {
                    return FramesRemaining * timePerFrame;
                }
            }

            public bool Active
            {
                get
                {
                    return FramesRemaining > 0;
                }
            }

            public Func<bool> CancelIf { get; private set; }

            public Timer(float seconds, Action onComplete, Func<bool> cancelIf)
            {
                timePerFrame = App.Instance.TargetElapsedTime.Milliseconds / 1000.0f;
                this.onComplete = onComplete;

                FramesRemaining = (int)Math.Round(seconds / timePerFrame);
                CancelIf = cancelIf;
            }

            public Timer() : this(0.0f, null, null)
            {
            }

            public void Update()
            {
                if (FramesRemaining == 0)
                {
                    onComplete?.Invoke();
                    return;
                }

                FramesRemaining--;
            }

            public void Cancel()
            {
                FramesRemaining = 0;
            }
        }

        private List<Timer> activeTimers;
        private List<Timer> removedTimers;

        public TimeKeeper()
        {
            activeTimers = new List<Timer>();
            removedTimers = new List<Timer>();
        }

        public void Update()
        {
            foreach (Timer t in activeTimers)
            {
                if (t.CancelIf != null && t.CancelIf() == true)
                {
                    t.Cancel();
                    removedTimers.Add(t);
                }
                else
                {
                    if (t.FramesRemaining == 0)
                        removedTimers.Add(t);

                    t.Update();
                }
            }

            foreach (Timer t in removedTimers)
                activeTimers.Remove(t);

            removedTimers.Clear();
        }

        public Timer StartTimer(float seconds, Action onComplete = null, Func<bool> cancelIf = null)
        {
            Timer timer = new Timer(seconds, onComplete, cancelIf);
            activeTimers.Add(timer);

            return timer;
        }
    }
}
