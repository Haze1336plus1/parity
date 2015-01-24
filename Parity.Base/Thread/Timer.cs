using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Thread
{
    public class Timer
    {

        public long CurrentTime { get; private set; }
        protected System.Threading.Thread TimerThread { get; private set; }
        protected List<TimedAction> Actions { get; private set; }

        private static object _lockingObject = new Object();
        private static Timer _instance;
        public static Timer GetInstance()
        {
            if (Timer._instance == null)
            {
                lock (Timer._lockingObject)
                {
                    if (Timer._instance == null)
                        Timer._instance = new Timer();
                }
            }
            return Timer._instance;
        }

        private Timer()
        {
            this.TimerThread = new System.Threading.Thread(this.DoTiming);
            this.Actions = new List<TimedAction>();
        }

        public void Start()
        {
            if (this.TimerThread.ThreadState == System.Threading.ThreadState.Unstarted)
                this.TimerThread.Start();
            else
                throw new System.Threading.ThreadStateException("Timer can be started only once!");
        }

        public void AddAction(long TimeOffset, Action Callback)
        {
            lock (Timer._lockingObject)
            {
                this.Actions.Add(new TimedAction(this.CurrentTime + TimeOffset, Callback));
            }
        }

        private void DoTiming()
        {
            while (true)
            {
                lock (Timer._lockingObject)
                {
                    this.CurrentTime = App.Time.Get();

                    for (int i = this.Actions.Count - 1; i >= 0; i--)
                    {
                        TimedAction iAction = this.Actions[i];
                        if (this.CurrentTime >= iAction.At)
                        {
                            iAction.Callback.DynamicInvoke();
                            this.Actions.RemoveAt(i);
                        }
                    }

                    System.Threading.Thread.Sleep(5);
                }
            }
        }

    }
}
