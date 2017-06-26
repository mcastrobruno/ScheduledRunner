using ScheduledRunner.Contract.Library;
using ScheduledRunner.Library.Contract;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace ScheduledRunner.Library
{
    public class Scheduler : IScheduler
    {
        #region "Dependencies"

        private readonly ITimeTracker _timeTracker;
        private readonly ITicker _ticker;

        #endregion "Dependencies"

        private TimeSpan[] _times;
        private Action _executeThis;

        public Scheduler(ITimeTracker timeTracker, ITicker ticker)
        {
            if (timeTracker == null) throw new ArgumentNullException(nameof(ITimeTracker));
            if (ticker == null) throw new ArgumentNullException(nameof(ITicker));

            _timeTracker = timeTracker;
            _ticker = ticker;
            _ticker.ActionCompleted += _ticker_ActionCompleted;
        }

        private void _ticker_ActionCompleted(object sender, EventArgs e)
        {
            RunAsync(_times, _executeThis);
        }

        /// <summary>
        /// Executa uma ação em horários programados
        /// </summary>
        /// <param name="times">Horários de execução</param>
        /// <param name="executeThis">Ação à ser executada</param>
        /// <returns></returns>
        public Task RunAsync(TimeSpan[] times, Action executeThis)
        {
            if (times == null) throw new ArgumentNullException();
            if (times.Length == 0) throw new ArgumentException("Deve ser informado ao menos um horário de execução.");

            _times = times;
            _executeThis = executeThis;

            return Task.Run(() =>
            {
                TimeSpan next;

                //Obtém o próximo intervalo no mesmo dia (até a meia noite)
                var futureScheduled = times.Where(t => t.Subtract(_timeTracker.TimeNow).TotalMilliseconds >= 0);

                if (futureScheduled.Count() > 0)
                    next = futureScheduled.Min(x => x);
                else //caso não tenha intervalo para o dia, agenda o mais próximo do próximo dia
                {
                    next = times.Where(t => t.Subtract(_timeTracker.TimeNow).TotalMilliseconds <= 0).Min(x => x);
                    if (next != null)
                        next = next.Add(new TimeSpan(24, 0, 0));
                    else
                        throw new InvalidOperationException("Não foi possível determinar o intervalo de execução.");
                }

                next = next.Subtract(_timeTracker.TimeNow);

                _ticker.ExecuteAsync(Convert.ToInt32(next.TotalMilliseconds), executeThis);
            });
        }
    }
}