using ScheduledRunner.Library.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScheduledRunner.Tests
{
    public class TickerMock : ITicker
    {
        private bool _executing = true;

        public event EventHandler ActionCompleted;

        private int _interval;
        public int Interval { get { return _interval; } }

        public void Tick()
        {
            _executing = false;
        }

        public void ExecuteAsync(int interval, Action executeThis)
        {
            _executing = true;
            _interval = interval;
            while (_executing)
                Task.Delay(1000).Wait();

            executeThis();
            ActionCompleted(this, null);

            _executing = false;
        }
    }
}