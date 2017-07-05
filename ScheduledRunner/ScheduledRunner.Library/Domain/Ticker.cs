using ScheduledRunner.Library.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScheduledRunner.Library.Domain
{
    public class Ticker : ITicker
    {
        private int _interval;
        public int Interval => _interval;

        public event EventHandler ActionCompleted;

        public void ExecuteAsync(int interval, Action executeThis)
        {

            Console.WriteLine($"Próxima execução em: {new TimeSpan(0, 0, 0, 0, interval)}");

            Thread.Sleep(interval);
            executeThis();
            ActionCompleted?.Invoke(this, null);
        }
    }
}
