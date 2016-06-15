using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalClock
{
    public class DigitalClock1 : Observer.Observer
    {
        public DateTime date;
        public override void Update()
        {
            base.Update();
            TimerSubject.Timer1 timer = (TimerSubject.Timer1)Activator.GetObject(typeof(TimerSubject.Timer1), "tcp://114.212.85.35:5656/getCurrentTime",null);
            if (timer != null)
            {
                date = timer.getCurrentTime();
            }
        }
        public DateTime DisplayTime(DateTime date)
        {
            return date;
        }
    }
}
