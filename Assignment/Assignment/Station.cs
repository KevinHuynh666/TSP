using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment
{
    class Station
    {
      public string Name { get; set; }
      public int x { get; set; }
      public int y { get; set; }

      public void NamingStation(string Name,string x, string y)
        {
            /*Get station info*/
          this.Name = Name;
          this.x = int.Parse(x);
          this.y = int.Parse(y);
        }

      
      public double calculateDistance(Station otherStation)
        {
            double distance = Math.Sqrt( ((this.x - otherStation.x)*(this.x - otherStation.x)) + ( (this.y - otherStation.y) * (this.y - otherStation.y) ) );
          
            return distance;
        }
    
      public string printDistance(Station otherStation)
        {
            return (this.Name + " -> " + otherStation.Name + " " + calculateDistance(otherStation));
        }
       
    
    }
}
