using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    class Tour
    {
        public List<Station> TourList { get; set; }
        private static Plane plane { get; set; }
        private int n = 0;

        public TimeSpan startTime { get; set; }
        public TimeSpan sTime { get; set; }
        List<double> timeStamp1 = new List<double>();


        public void getTour(List<Station> aTour)
        {
            int m = aTour.Count;
            this.TourList = new List<Station>();
            this.n = m;
            for (int i = 0; i < n; i++)
            {
                this.TourList.Add(aTour[i]);
            }

        }

        public void checkTime(Plane testPlane)
        {
            /*Get plane info*/
            double rangeTime = testPlane.getRange() * 60;
            double remain = rangeTime;
            double speed = testPlane.getSpeed();
            double prepTime = testPlane.getTakeOff() + testPlane.getLand();
            double fuelTime = testPlane.getFuel();


            for (int i = 0; i < n; i++)
            {
                double distance = 0;
                double time = prepTime;
                /*Calculate distance from 2 stations*/
                if (i == n - 1)
                {
                    distance = TourList[i].calculateDistance(TourList[0]);
                }
                else
                {
                    distance = TourList[i].calculateDistance(TourList[i + 1]);
                }

                /*Calculate the time to get from one station to another, plus landing and take off time*/
                time = prepTime + Math.Round((distance / speed) * 60, MidpointRounding.AwayFromZero);

                /*Check if the remaining fuel is enough to fly from A to B*/
                if (time <= remain)
                {
                    this.timeStamp1.Add(time);
                    remain -= time;

                }
                else if (time > remain)
                {
                    this.timeStamp1.Add(-fuelTime);
                    this.timeStamp1.Add(time);
                    remain = rangeTime - time;
                }
            }
        }

        /*Calculate the total time for whole tour*/
        public double totalTime()
        {
            double total = 0;
            for (int i = 0; i < timeStamp1.Count; i++)
            {
                if (timeStamp1[i] < 0)
                {
                    total += -this.timeStamp1[i];
                }
                else
                {
                    total += this.timeStamp1[i];
                }
            }
            return total;

        }
        /*Print out the details tour like the format*/
        public void printTime()
        {
            int j = 0;
            TimeSpan t = TimeSpan.FromMinutes(0);
            for (int i = 0; i < timeStamp1.Count; i++)
            {
                if (timeStamp1[i] < 0)
                {
                    Console.WriteLine("*** refuel {0} minutes ***", timeStamp1[i] * -1);
                    this.startTime += TimeSpan.FromMinutes(-timeStamp1[i]);
                }
                else
                {

                    if (j == this.n - 1)
                    {
                        Console.Write("{0}\t->\t{1}", this.TourList[j].Name, this.TourList[0].Name);
                        Console.WriteLine("\t{0}\t{1}", this.startTime.ToString("hh':'mm"), (this.startTime + TimeSpan.FromMinutes(timeStamp1[i])).ToString("hh':'mm"));

                    }
                    else
                    {
                        Console.Write("{0}\t->\t{1}", this.TourList[j].Name, this.TourList[j + 1].Name);
                        Console.WriteLine("\t{0}\t{1}", this.startTime.ToString("hh':'mm"), (this.startTime + TimeSpan.FromMinutes(timeStamp1[i])).ToString("hh':'mm"));
                    }
                    j++;
                    this.startTime += TimeSpan.FromMinutes(timeStamp1[i]);
                }
            }
        }

        /*Write to file the details tour like the format*/
        public void writeTime(StreamWriter fo)
        {
            int j = 0;
            TimeSpan t = TimeSpan.FromMinutes(0);
            this.startTime = this.sTime;
            for (int i = 0; i < timeStamp1.Count; i++)
            {
                if (timeStamp1[i] < 0)
                {
                    fo.WriteLine("*** refuel {0} minutes ***", timeStamp1[i] * -1);
                    this.startTime += TimeSpan.FromMinutes(-timeStamp1[i]);
                }
                else
                {

                    if (j == this.n - 1)
                    {
                        fo.Write("{0}\t->\t{1}", this.TourList[j].Name, this.TourList[0].Name);
                        fo.WriteLine("\t{0}\t{1}", this.startTime.ToString("hh':'mm"), (this.startTime + TimeSpan.FromMinutes(timeStamp1[i])).ToString("hh':'mm"));

                    }
                    else
                    {
                        fo.Write("{0}\t->\t{1}", this.TourList[j].Name, this.TourList[j + 1].Name);
                        fo.WriteLine("\t{0}\t{1}", this.startTime.ToString("hh':'mm"), (this.startTime + TimeSpan.FromMinutes(timeStamp1[i])).ToString("hh':'mm"));
                    }
                    j++;
                    this.startTime += TimeSpan.FromMinutes(timeStamp1[i]);

                }
            }
        }
    }
}
