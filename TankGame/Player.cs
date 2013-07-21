using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class Player
    {
        public Player(Point loc,int life,int mony,bool shot)
        {
            location = loc;
            health = life;
            coins = mony;
            bulletFired = shot;
            dead = false;

        }

        public Point location { get; set; }
        public int health { get; set; }
        public int direction { get; set; }
        public int coins { get; set; }
        public bool bulletFired { get; set; }
        public bool dead { get; set; }

    }
}
