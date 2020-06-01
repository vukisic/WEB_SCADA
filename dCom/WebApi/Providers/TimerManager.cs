using System;
using System.Threading;

namespace WebApi.Providers
{
    /// <summary>
    /// Represents TimeManager with action
    /// witch is called on specified time
    /// </summary>
    public class TimerManager
    {
        #region Fields
        private Timer timer;
        private AutoResetEvent autoResetEvent;
        private Action action;
        #endregion

        #region Properties
        public DateTime TimerStarted { get; }
        #endregion
        public TimerManager(Action action, int dbc = 2)
        {
            this.action = action;
            autoResetEvent = new AutoResetEvent(false);
            timer = new Timer(Execute, autoResetEvent, 1000, dbc*1000);
            TimerStarted = DateTime.Now;
        }

        /// <summary>
        /// Method that executes action on specified time
        /// </summary>
        /// <param name="stateInfo"></param>
        public void Execute(object stateInfo)
        {
            action();

            if ((DateTime.Now - TimerStarted).Seconds > 60)
            {
                timer.Dispose();
            }
        }
    }
}
