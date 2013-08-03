using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame3
{
    public class HealthPack
    {
        public int X, Y;
        public int lt;
        public bool live;
        public double startTime;
        public  HealthPack(int ax,int ay,int alt)
        {
            X = ax;
            Y = ay;
            lt = alt;
            live = true;
            startTime = 0;
        }

    }
}
