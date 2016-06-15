using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subject
{
    public class Subject
    {
        private List<Observer.Observer> listObserver = new List<Observer.Observer>();
        private void Attach(Observer.Observer obser)
        {

        }
        private void Detach(Observer.Observer obser)
        {

        }
        public void Notify()
        {
            Observer.Observer obs = (Observer.Observer)Activator.GetObject(typeof(Observer.Observer), "tcp://114.212.85.35:8089/Update", null);
            if(obs != null)
                obs.Update();
        }
    }
}
