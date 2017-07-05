using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledRunner.Library.Contract
{
    public interface ITimeTracker
    {
        TimeSpan TimeNow { get; }
    }
}