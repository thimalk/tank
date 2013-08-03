using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame3
{
    public class Player
    {
        public int number;
        public int x;
        public int y;
        public int direction;
        public int health;
        public int coins, points;
        public bool coinRemove;
        public Player(int anum, int ax, int ay, int aD)
        {
            number = anum;
            x = ax;
            y = ay;
            direction = aD;
            health = 100;
            coins = 0;
            points = 0;
            coinRemove=false;
        }
        public void update(int ax, int ay, int aD, int h, int c, int p)
        {
            x = ax;
            y = ay;
            direction = aD;
            health = h;
            coins = c;
            points = p;
            

        }

    }
}
