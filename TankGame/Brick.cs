using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class Brick
    {
        public Point location {get;set;}
        public int damage { get; set; }

        public Brick(Point loc,int dmg)
        {
            location = loc;
            damage = dmg;
        }

    }
}
