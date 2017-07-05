using ScheduledRunner.Library;
using ScheduledRunner.Library.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScheduledRunner.UI
{
    public partial class Form1 : Form
    {

        Scheduler _scheduler = new Scheduler(new TimeTracker(), new Ticker());

        public Form1()
        {
            InitializeComponent();
        }



        private void btnRun_Click(object sender, EventArgs e)
        {
            Action _action = new Action(() =>
            {
                Console.WriteLine($"Printou... {DateTime.Now.TimeOfDay}");
            });

            _scheduler.RunAsync(
                new TimeSpan[]
                {
                    new TimeSpan(23,24,0),
                    new TimeSpan(23,26,0)
                },
                _action);
        }
    }
}
