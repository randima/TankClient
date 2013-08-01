using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class Cell : IComparable<Cell>
    {
        public Cell parent { get; set; }
        public int state { get; set; } //-1 discovered, 0-normal,1-brick,2-stone,3-water
        public int costofCell { get; set; }
        public Point location { get; set; }
        public int f { get; set; }
        public int g { get; set; }
        public int h { get; set; }
        public int dir { get; set; }

        public Cell(int i,int j)
        {
            this.state = 0;
            this.costofCell = 1;
            f = 0;
            g = 0;
            h = 0;
            location = new Point(i, j);
        }

        public int CompareTo(Cell other)
        {
            return this.f.CompareTo(other.f);
        }

        public void clearCell()
        {
            parent = null;
            f = 0;
            g = 0;
            h = 0;
        }

    }
}
