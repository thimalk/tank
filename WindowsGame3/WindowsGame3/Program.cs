using System;
using AI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame3
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

           
          //  if (com.reply[0] == 'I')
                //.WriteLine("dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");


            //Console.Title = "gona";
            //Console.WriteLine("/////////////");
           
            using (Game1 game = new Game1())
            {
                game.Run();
            }
           
           
            //com.ReceiveData();
        }
    }
#endif
}

