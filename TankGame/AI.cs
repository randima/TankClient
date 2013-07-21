using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGame
{
    class AI
    {


       /* public void AStar()
        {
            List<Cell> openlist = new List<Cell>();
            List<Cell> closelist = new List<Cell>();
            openlist.Add(map[(int)s.X, (int)s.Y]);
            bool done = false;
            int cost;
            //bool skipdiag = true;

            while (!done || openlist.Count == 0)
            {
                Cell current = openlist.Min();
                openlist.Remove(current);
                closelist.Add(current);
                int x = current.x, y = current.y;
                //check adjacent squares
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i * j) == 1)
                            continue;
                        else if (x + i < 0 || x + i > mapsize - 1 || y + j < 0 || y + j > mapsize - 1)
                            continue;
                        else if (closelist.Contains(map[x + i, y + j]))
                            continue;
                        else if (openlist.Contains(map[x + i, y + j]))
                        {
                            continue;
                        }
                        else if (map[x + i, y + j].state > 1)
                            continue;
                        else if (x + i == (int)d.X && y + j == (int)d.Y)
                        {
                            map[x + i, y + j].parent = map[x, y];
                            done = true;
                            break;
                        }
                        else
                        {
                            if (map[x + i, y + j].state == 1)
                                cost = 4;
                            else
                            {
                                cost = 0;
                                map[x + i, y + j].state = -1;
                            }

                            if (current.dir == 0)
                                map[x + i, y + j].g = map[x, y].g + (j + 2) * 1 + cost;
                            if (current.dir == 1)
                                map[x + i, y + j].g = map[x, y].g + Math.Abs(i - 2) * 1 + cost;
                            if (current.dir == 2)
                                map[x + i, y + j].g = map[x, y].g + Math.Abs(j - 2) * 1 + cost;
                            if (current.dir == 3)
                                map[x + i, y + j].g = map[x, y].g + (i + 2) * 1 + cost;

                            if (j == -1)
                                map[x + i, y + j].dir = 0;
                            if (i == 1)
                                map[x + i, y + j].dir = 1;
                            if (j == 1)
                                map[x + i, y + j].dir = 2;
                            if (i == -1)
                                map[x + i, y + j].dir = 3;

                            int h = (Math.Abs((int)d.X - (x + i)) + Math.Abs((int)d.Y - (y + j)));
                            map[x + i, y + j].h = h;
                            map[x + i, y + j].f = map[x + i, y + j].g + h;
                            map[x + i, y + j].parent = map[x, y];
                            grid[x + i, y + j] = 1;
                            openlist.Add(map[x + i, y + j]);
                        }



                    }

                }
            }

        }*/
    }
}
