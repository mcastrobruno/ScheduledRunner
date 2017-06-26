using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScheduledRunner.Library;
using ScheduledRunner.Library.Contract;
using System.Threading.Tasks;
using Moq;

namespace ScheduledRunner.Tests
{
    [TestClass]
    public class SchedulerTest
    {
        //Sut
        private Scheduler _scheduler;

        //Mocks
        private ITicker _tickerMock;

        private Mock<ITimeTracker> _timeTrackerMock;

        [TestInitialize]
        public void Init()
        {
            _timeTrackerMock = new Mock<ITimeTracker>();
            _tickerMock = new TickerMock();
            _scheduler = new Scheduler(_timeTrackerMock.Object, _tickerMock);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_exception_when_times_is_null()
        {
            //act
            _scheduler.RunAsync(null, () => { Console.WriteLine("Print this"); });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_throw_exception_when_times_is_empty()
        {
            //act
            _scheduler.RunAsync(new TimeSpan[] { }, () => { Console.WriteLine("Print this"); });
        }

        /// <summary>
        /// Deve agendar a execução como sendo a próxima quando informado apenas um horário agendado.
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public async Task Should_schedule_action_to_be_executedAsync()
        {
            //arrange
            _timeTrackerMock.SetupGet(x => x.TimeNow).Returns(new TimeSpan(10, 0, 0));

            //act
            await _scheduler.RunAsync(new TimeSpan[] { new TimeSpan(10, 10, 0) }, () => { Console.WriteLine("Print this"); });

            //assert
            //Verifica se foi programado para após 10 minutos
            Assert.AreEqual(10 * 60 * 1000, _tickerMock.Interval);
        }

        /// <summary>
        /// Deve agendar a execução mais próxima do horário atual quando houver mais de um horário agendado.
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public void Should_schedule_action_to_be_executed_with_2_scheduled_times()
        {
            //arrange
            _timeTrackerMock.SetupGet(x => x.TimeNow).Returns(new TimeSpan(10, 0, 0));

            //act
            _scheduler.RunAsync(new TimeSpan[] { new TimeSpan(10, 10, 0), new TimeSpan(10, 20, 0) }, () => { Console.WriteLine("Print this"); });

            //assert
            //Verifica se foi programado para após 10 minutos
            Assert.AreEqual(10 * 60 * 1000, _tickerMock.Interval);
        }

        /// <summary>
        /// Deve agendar a execução mais próxima do horário atual quando houver mais de um horário agendado.
        /// Obs: Um horário será para o dia posterior
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public void Should_schedule_action_to_be_executed_in_the_next_day()
        {
            //arrange
            _timeTrackerMock.SetupGet(x => x.TimeNow).Returns(new TimeSpan(10, 0, 0));

            //act
            _scheduler.RunAsync(new TimeSpan[] { new TimeSpan(9, 0, 0) }, () => { Console.WriteLine("Print this"); });

            //assert
            //Verifica se foi programado para após 10 minutos
            Assert.AreEqual(23 * 60 * 60 * 1000, _tickerMock.Interval);
        }

        /// <summary>
        /// Deve agendar a execução mais próxima do horário atual quando houver mais de um horário agendado.
        /// Obs: Um horário será para o dia posterior
        /// </summary>
        [TestMethod]
        [Timeout(3000)]
        public void Should_scheduled_to_the_next_time_when_the_prior_scheduled_task_was_executed()
        {
            //arrange
            _timeTrackerMock.SetupGet(x => x.TimeNow).Returns(new TimeSpan(10, 0, 0));

            //act
            _scheduler.RunAsync(new TimeSpan[] { new TimeSpan(10, 30, 0), new TimeSpan(11, 30, 0) }, () => { Console.WriteLine("Print this"); });

            //assert
            Assert.AreEqual(30 * 60 * 1000, _tickerMock.Interval);
            Assert.AreEqual(60 * 60 * 1000, _tickerMock.Interval);
        }
    }
}