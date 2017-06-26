using System;
using System.Threading.Tasks;

namespace ScheduledRunner.Contract.Library
{
    public interface IScheduler
    {
        Task RunAsync(TimeSpan[] times, Action executeThis);
    }
}