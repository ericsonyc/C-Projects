using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimerSubject
{
    public class Timer1 : Subject.Subject
    {
        public DateTime getCurrentTime ()
        {
            return DateTime.Now;
        }
    }
}
