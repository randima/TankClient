using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class LifePack
    {
        public Point location{get;set;}
        public TimeSpan spawnTime { get; set; }
        public int cooldown { get; set; }
       

        public LifePack(Point p,TimeSpan st,int ct)
        {
            spawnTime = st;
            cooldown = ct;            
            location = p;
        }
    }
}
