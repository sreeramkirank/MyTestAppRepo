using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Example_17_3____Delegates_and_Events
{
    //change_1_Branch commit 1
    //change_1_Branch commit 2
    //Change_3_branch -- commit 1
    //Change_3_branch -- commit 2
    //master change commit 1
    //NewBranch 1 changes
    //Branch_1 changes
    //Branch_1.1 changes
    // a class to hold the information about the event
    // in this case it will hold only information
    // available in the clock class, but could hold
    // additional state information
    public class TimeInfoEventArgs : EventArgs
    {
        public int hour;
        public int minute;
        public int second;

        public TimeInfoEventArgs(int hour, int minute, int second)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }
    }

    // The publisher: the class that other classes
    // will observe. This class publishes one delegate:
    // SecondChanged.
    public class Clock
    {
        private int hour;
        private int minute;
        private int second;

        // the delegate the subscribers must implement
        public delegate void SecondChangeHandler(object clock, 
                        TimeInfoEventArgs timeInformation);
        // an instance of the delegate with event keyword added
        public event SecondChangeHandler SecondChanged;
        // set the clock running
        // it will raise an event for each new second
        public void Run( )
        {
            for (; ; )
            {
                // sleep 10 milliseconds
                Thread.Sleep(100);

                // get the current time
                System.DateTime dt = System.DateTime.Now;
                // if the second has changed
                // notify the subscribers
                if (dt.Second != second)
                {
                    // create the TimeInfoEventArgs object
                    // to pass to the subscriber
                    TimeInfoEventArgs timeInformation = 
                        new TimeInfoEventArgs(dt.Hour, dt.Minute, dt.Second);

                    // if anyone has subscribed, notify them
                    if (SecondChanged != null)
                    {
                        SecondChanged(this, timeInformation);
                    }
                }

                // update the state
                this.second = dt.Second;
                this.minute = dt.Minute;
                this.hour = dt.Hour;
            }
        }
    }

    // A subscriber: DisplayClock subscribes to the
    // clock's events. The job of DisplayClock is
    // to display the current time
    public class DisplayClock
    {

        // given a clock, subscribe to
        // its SecondChangeHandler event
        public void Subscribe(Clock theClock)
        {

            //theClock.SecondChanged += new Clock.SecondChangeHandler(TimeHasChanged);
            // (         OR       )
            theClock.SecondChanged += TimeHasChanged;
            //OR combining above and below two methods together for ananymous methos

            /*
             * theClock.SecondChanged += delegate(object Clock, TimeInfoEventArgs ti)
            {
                Console.WriteLine("Current Time: {0}:{1}:{2}",
                  ti.hour.ToString(), ti.minute.ToString(), ti.second.ToString());
            };*/
            
        }        


        
        // the method that implements the
        // delegated functionality
        public void TimeHasChanged(object Clock, TimeInfoEventArgs ti)
        {
            Console.WriteLine("Current Time to write: {0}:{1}:{2}", 
              ti.hour.ToString( ), ti.minute.ToString( ), ti.second.ToString( ));
        }
    }
    // a second subscriber whose job is to write to a file
    public class LogCurrentTime
    {
        public void Subscribe(Clock theClock)
        {
            /*theClock.SecondChanged += 
                     new Clock.SecondChangeHandler(WriteLogEntry);*/

            theClock.SecondChanged += delegate(object Clock, TimeInfoEventArgs ti)
            {
                Console.WriteLine("Logging time to file(First change- Branch 1 changes): {0}:{1}:{2}",
                    ti.hour.ToString(), ti.minute.ToString(), ti.second.ToString());
            };
        }
        /*
        // this method should write to a file
        // we write to the console to see the effect
        // this object keeps no state
        public void WriteLogEntry(object theClock, TimeInfoEventArgs ti)
        {
            Console.WriteLine("Logging to file: {0}:{1}:{2}", 
              ti.hour.ToString( ), ti.minute.ToString( ), ti.second.ToString( ));
        }
         */
    }

    public class Tester
    {
        public void Run( )
        {
            // create a new clock
            Clock theClock = new Clock( );

            // create the display and tell it to
            // subscribe to the clock just created
            DisplayClock dc = new DisplayClock( );
            dc.Subscribe(theClock);

            // create a Log object and tell it
            // to subscribe to the clock
            LogCurrentTime lct = new LogCurrentTime( );
            lct.Subscribe(theClock);

            // Get the clock started
            theClock.Run( );
        }
    }

    public class Program
    {
        public static void Main( )
        {
            Tester t = new Tester( );
            t.Run( );
        }
    }
}