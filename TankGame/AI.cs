using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankGame
{
    class AI
    {

        private Cell[,] map;
        private Stack<String> commands;
        private int size;
        private int playerID;
        private Player[] playrs;
        private bool serverError=false;        
        String lastCommand;
        private GameManager engine;
        private static AI gameAI = new AI();

        public static AI getInstance()
        {
            return gameAI;
        }

        public AI()
        {
            commands = new Stack<string>();
            path = new List<Point>();
            
        }

        public void initAI()
        {
            engine = GameManager.getInstance();
            playerID = engine.playerid;
            size=engine.getMapSize();
            map=new Cell[size,size];
            playrs= engine.getPlayers();
            initMap();
        }

        public void work()
        {
           int g=90000,current;
           Point goal = playrs[playerID].location;
           updateBricks();           
           if (!playrs[playerID].dead)
           {
               if (engine.getCoinList().Count > 0)
               {
                   foreach (Coin c in engine.getCoinList())
                   {
                      
                       current = AStar(playrs[playerID].location, c.location, playrs[playerID].getDirection());
                       if (current < g)
                       {
                           g = current;
                           goal = c.location;
                       }


                   }
                   
                   SetCommands(playrs[playerID].location, goal, playrs[playerID].getDirection());
               }
           }
            
        }

        public void initMap()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j] = new Cell(i,j);
                }
            }

            foreach (Brick b in engine.getBricks())
            {
                map[b.location.X, b.location.Y].state = 1;
                map[b.location.X, b.location.Y].costofCell = 5;
            }

            foreach (Point p in engine.getStone())
            {
                map[p.X, p.Y].state = 2;
            }

            foreach (Point p in engine.getWater())
            {
                map[p.X, p.Y].state = 3;
            }
        }




        public int AStar(Point start,Point goal,int direction)
        {
            int totalcost=8000;
            clear();
            List<Cell> openlist = new List<Cell>();
            List<Cell> closelist = new List<Cell>();
            map[start.X, start.Y].dir = direction;
            openlist.Add(map[start.X, start.Y]);
            bool done = false;
            int cost;
            //bool skipdiag = true;

            while (!done || openlist.Count > 0)
            {
                Cell current = openlist.Min();
                openlist.Remove(current);
                closelist.Add(current);
                int x = current.location.X, y = current.location.Y;
                //check adjacent squares
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i * j) == 1)
                            continue;
                        else if (x + i < 0 || x + i > size - 1 || y + j < 0 || y + j > size - 1)
                            continue;
                        else if (closelist.Contains(map[x + i, y + j]))
                            continue;
                        else if (openlist.Contains(map[x + i, y + j]))
                        {
                            continue;
                        }
                        else if (map[x + i, y + j].state > 1)
                            continue;
                        
                        else
                        {
                            if (map[x + i, y + j].state == 1)
                                cost = map[x + i, y + j].costofCell;
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

                            int h = (Math.Abs(goal.X - (x + i)) + Math.Abs(goal.Y - (y + j)));
                            map[x + i, y + j].h = h;
                            map[x + i, y + j].f = map[x + i, y + j].g + h;
                            map[x + i, y + j].parent = map[x, y];
                            //grid[x + i, y + j] = 1;

                            if (x + i == goal.X && y + j == goal.Y)
                            {
                                totalcost = map[x + i, y + j].g;
                                done = true;
                                break;
                            }
                            
                            
                            openlist.Add(map[x + i, y + j]);
                        }

                    }
                }
            }
            return totalcost;
        }

        public void updateBricks()
        {
            foreach (Brick b in engine.getBricks())
            {
                map[b.location.X, b.location.Y].costofCell = 5-b.damage;
            }
        }

        private void SetCommands(Point start, Point goal, int direction)
        {
            AStar(start,goal,direction);
            Point current =goal ;
            while (map[current.X, current.Y].parent != null)
            {
                
                int cost = map[current.X, current.Y].g - map[current.X, current.Y].parent.g;
                if (cost > 1)
                {
                    if (map[current.X, current.Y].dir != map[current.X, current.Y].parent.dir)
                    {
                        insertDirectionCommand(map[current.X, current.Y].dir);
                        cost = cost - 1;
                    }
                    if (map[current.X, current.Y].state == 1)
                    {
                        for (int i = 0; i < cost - 1; i++) 
                        {
                            commands.Push("SHOOT#");
                        }
                    }
                    
                }
                insertDirectionCommand(map[current.X, current.Y].dir);             

            }
            clear();             
        }

        private void insertDirectionCommand(int dir)
        {
            if (dir == 0)
                commands.Push("UP#");
            if (dir == 1)
                commands.Push("RIGHT#");
            if (dir == 2)
                commands.Push("DOWN#");
            if (dir == 3)
                commands.Push("LEFT#");
        }

        public void sendCommands()
        {

            if (commands.Count > 0)
            {                
                engine.sendAICommands(lastCommand = commands.Pop());    
                          

            }
            else
                work();
        }

        public void clearCommands()
        {
            commands.Clear();
        }

        public void reportError(String err)
        {
            //serverError = true;
            /*if (err.Equals("OBSTACLE#"))
            {
                commands.Clear();
                work();
            }
            if (err.Equals("CELL_OCCUPIED#"))
            {
                commands.Clear();
                work();
            }*/
            //commands.Clear();
            //work();                

        }

        



        public void clear()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j].clearCell();
                }
            }
        }
    }
}
