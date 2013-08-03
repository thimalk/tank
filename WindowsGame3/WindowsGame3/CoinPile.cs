using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame3
{
    public class CoinPile
    {
        public int X, Y;
        public int lt;
        public bool live;
        public int val;
        public double startTime;
        public  CoinPile(int ax, int ay, int alt, int aval)
        {
            X = ax;
            Y = ay;
            lt = alt;
            val = aval;
            live = true;
            startTime = 0;
        }
    }
}
