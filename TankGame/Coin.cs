using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class Coin
    {
        public Point location{get;set;}
        public TimeSpan spawnTime { get; set; }
        public int cooldown { get; set; }
        public int value { get; set; }

        public Coin(Point p,TimeSpan st,int ct,int val)
        {
            spawnTime = st;
            cooldown = ct;
            value = val;
            location = p;
        }
    }
}
