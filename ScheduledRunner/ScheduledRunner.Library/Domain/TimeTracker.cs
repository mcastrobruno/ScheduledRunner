using ScheduledRunner.Library.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledRunner.Library.Domain
{
    public class TimeTracker : ITimeTracker
    {
        public TimeSpan TimeNow { get => DateTime.Now.TimeOfDay; }
    }
}
