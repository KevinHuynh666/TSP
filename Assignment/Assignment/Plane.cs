using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    class Plane
    {
        private double Range { get; set; }
        private double Speed { get; set; }
        private double TakeOff { get; set; }
        private double Land { get; set; }
        private double Fuel { get; set; }

        public void addPlane(string a, string b, string c, string d, string e)
        {
            this.Range = double.Parse(a);
            this.Speed = double.Parse(b);
            this.TakeOff = double.Parse(c);
            this.Land = double.Parse(d);
            this.Fuel = double.Parse(e);
        }
        public double getRange()
        {
            return this.Range;
        }

        public double getSpeed()
        {
            return this.Speed;
        }

        public double getTakeOff()
        {
            return this.TakeOff;
        }

        public double getLand()
        {
            return this.Land;
        }

        public double getFuel()
        {
            return this.Fuel;
        }

    }
}
