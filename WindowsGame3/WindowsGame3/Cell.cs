using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame3
{
    public class Cell
    {
        //0-empty 1-brick 2-stone 3-player
        public int health = 100;
        public int X, Y;
        public Cell(int ax, int ay)
        {
            X = ax;
            Y = ay;
            health = 100;


        }
        public void update(int h)
        {

            health = 100 - h * 25;



        }


    }
}
