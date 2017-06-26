using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledRunner.Library.Contract
{
    public interface ITicker
    {
        /// <summary>
        /// Executa a ação após interval milliseconds
        /// </summary>
        /// <param name="interval">Intervalo em milisegundos</param>
        /// <param name="executeThis">Ação à ser executada</param>
        /// <returns></returns>
        void ExecuteAsync(int interval, Action executeThis);

        /// <summary>
        /// Evento que avisa que uma tarefa foi executada
        /// </summary>
        event EventHandler ActionCompleted;

        int Interval { get; }
    }

    public class ScheduledEventArgs
    {
        public TimeSpan[] Times { get; set; }
        public Action ActionToExecute { get; set; }
    }
}