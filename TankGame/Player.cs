using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class Player
    {
        private int dir;
        public Player(Point loc,int direction)
        {
            location = loc;
            health = 100;
            coins = 0;
            bulletFired = false;
            dead = false;
            points = 0;
            setDirection(direction);
            

        }

        
        public Point location { get; set; }
        public int health { get; set; }
        public int points { get; set; }
        public Vector2 direction { get; set; }
        public int coins { get; set; }
        public bool bulletFired { get; set; }
        public bool dead { get; set; }

        public void setDirection(int d)
        {
            dir = d;
            if (dir == 0)
            {
                direction = new Vector2(0, 1);
            }
            else if (dir == 1)
            {
                direction = new Vector2(1, 0);
            }
            else if (dir == 2)
            {
                direction = new Vector2(0, -1);
            }
            else
            {
                direction = new Vector2(-1, 0);
            }
        }

        public void setShot(int i)
        {
            if (i == 1)
                bulletFired = true;
            if (i == 0)
                bulletFired = false;
        }

        public int getDirection()
        {
            return dir;
        }

        public float rotation()
        {
            if (dir == 0)
            {
                return MathHelper.Pi;
            }
            else if (dir == 1)
            {
                return -MathHelper.PiOver2;
            }
            else if (dir == 2)
            {
                return 0;
            }
            else
            {
                return MathHelper.PiOver2;
            }
        }
       

    }
}
