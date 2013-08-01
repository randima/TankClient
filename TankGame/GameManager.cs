using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using System.ComponentModel;

namespace TankGame
{
    class GameManager
    {

        private BackgroundWorker reciverThread = new BackgroundWorker();
        private List<Point> Stone;
        private List<Point> Water;
        private List<Brick> Bricks;
        private List<Coin> listOfCoins;
        private List<LifePack> listofLifePack;
        private bool started ;
        private TimeSpan gametime;
        private NetworkHandler com;
        private AI gameAI;
        private String err;
        private int maxPlayers;
        private int mapsize;
        private Player[] player;
        private String joinErr;
             
        private static GameManager engine = new GameManager();

         public int playerid{get;set;}
                
        public static GameManager getInstance()
        {
           return engine;
        }

        public GameManager()
        {
            Bricks = new List<Brick>();
            gameAI = AI.getInstance();
            Water = new List<Point>();
            Stone = new List<Point>();
            listOfCoins = new List<Coin>();
            listofLifePack=new List<LifePack>();
            mapsize = 10;
            maxPlayers = 5;
            player = new Player[maxPlayers];
            started = false;
        }       

      


        public bool JoinGame()
        {
            bool succeed = false;
            com = NetworkHandler.GetInstance();
            if (com.SendData("JOIN#"))
            {
                succeed = true;
                reciverThread.DoWork += new DoWorkEventHandler(reciverThread_DoWork);
                reciverThread.RunWorkerAsync();

            }
            return succeed;
            /*this.analyze((object)"I:P4:7,1;1,3;3,1;5,8;7,8;8,6;4,8:6,3;3,2;5,3;9,3;1,8;0,8;2,6;6,8;1,4:9,2;6,7;7,6;1,2;2,4;4,7;2,1;8,4;7,2;0,4#");
            this.analyze((object)"S:P0;0,0;0:P1;0,9;0:P2;9,0;0:P3;9,9;0:P4;5,5;0#");
            this.analyze((object)"C:3,4:5898:55541#");
            this.analyze((object)"C:3,8:7189:1545#");
            this.analyze((object)"C:2,7:10479:1082#");
            this.analyze((object)"L:2,2:18777#");
            Console.WriteLine(TimeSpan.FromMilliseconds(59479).TotalSeconds);*/
            
        }

        private void reciverThread_DoWork(object sender, DoWorkEventArgs e)
        {
            com.ReceiveData();
        }



       

        public void analyze(Object obj)
        {
            err = (String)obj;
            String msg = (String)obj;      
           // Console.WriteLine(engine.getWater().Count+"");
            msg=msg.TrimEnd('#');           
            String[] info = msg.Split(':');

            if (info[0].Equals("I"))
            {
                playerid = int.Parse(info[1].Substring(1));
                addBricks(info[2]);
               
                addStones(info[3]);
                //Console.WriteLine(info[3]);
                addWater(info[4]);
                //Console.WriteLine(info[4]);

                //Console.WriteLine("");
                //Console.WriteLine(playerid);
                //Console.WriteLine(" game started");

            }
            else if (info[0].Equals("S"))
            {
                int i = 0;
                foreach (String s in info)
                {
                    if (s.Equals("S"))
                        continue;
                    String[] token = s.Split(';');
                    player[i] = new Player(getPoint(token[1]), int.Parse(token[2]));
                    i++;

                }
                started = true;

            }
            else if (info[0].Equals("G"))
            {
                for (int i = 0; i < maxPlayers; i++)
                {
                    String[] token = info[i + 1].Split(';');
                    player[i].location = getPoint(token[1]);
                    player[i].setDirection(int.Parse(token[2]));
                    player[i].setShot(int.Parse(token[3]));
                    player[i].health = int.Parse(token[4]);
                    player[i].coins = int.Parse(token[5]);
                    player[i].points = int.Parse(token[6]);
                }
                String[] brks = info[maxPlayers + 1].Split(';');
                foreach (String b in brks)
                {
                    String[] item = b.Split(',');
                    Brick temp = Bricks.Find(br => br.location == new Point(int.Parse(item[0]), int.Parse(item[1])));
                    temp.damage = int.Parse(item[2]);
                }
            }
            else if (info[0].Equals("L"))
            {
                listofLifePack.Add(new LifePack(getPoint(info[1]), gametime, int.Parse(info[2])));
            }

            else if (info[0].Equals("C"))
            {
                listOfCoins.Add(new Coin(getPoint(info[1]), gametime, int.Parse(info[2]), int.Parse(info[3])));

            }
            else
            {
               // if (err.Equals("PLAYERS_FULL#") || err.Equals("ALREADY_ADDED#") || err.Equals("GAME_ALREADY_STARTED#"))
                    //joinErr = err;
                    
                //gameAI.reportError(err);
                Console.WriteLine(err);
            }
               



        }

        private void addBricks(String info)
        {
           
            String[] token=info.Split(';');
            foreach (String s in token)
            {
                Bricks.Add(new Brick(getPoint(s),0));
            }
        }

        private void addStones(String info)
        {
            
            String[] token = info.Split(';');
            foreach (String s in token)
            {
                Stone.Add(getPoint(s));
            }
         
        }

        private void addWater(String info)
        {            
            
            String[] token = info.Split(';');
            foreach (String s in token)
            {
                Water.Add(getPoint(s));
            }
        }

        private Point getPoint(String loc)
        {
            String[] xy = loc.Split(',');
            int x = int.Parse(xy[0]);
            int y = int.Parse(xy[1]);
            return new Point(x, y);
        }

        public List<Point> getStone()
        {
            return Stone;
        }

        public List<Point> getWater()
        {
            return Water;
        }

        public List<Brick> getBricks()
        {
            return Bricks;
        }


        public Player[] getPlayers()
        {
            return player;
        }

        public List<Coin> getCoinList()
        {
            return listOfCoins;
        }

        public List<LifePack> getLifePacks()
        {
            return listofLifePack;
        }
        

        public bool getGameState()
        {
            return started;
        }

        public void updateTime(TimeSpan t)
        {
            gametime = t;
           
        }


        public int getMapSize()
        {
            return mapsize;
        }

        public void sendAICommands(String c)
        {
            com.SendData(c);
        }

        public String getJoinErros()
        {
            return "ab";

        }
        


        /*public void testAi(String c)
        {
            int x, y;
            x = player[playerid].location.X;
            y = player[playerid].location.Y;
             if (c.Equals("UP#"))
             {
                 if (player[playerid].getDirection() == 0)
                     player[playerid].location=new Point(x,y-1);
                 else
                     player[playerid].setDirection(0);
             }

            if (c.Equals("RIGHT#"))
            {
                 if(player[playerid].getDirection()==1)
                     player[playerid].location = new Point(x+1, y);
                else
                    player[playerid].setDirection(1);
            }
             if (c.Equals("DOWN#"))
            {
                 if(player[playerid].getDirection()==2)
                     player[playerid].location = new Point(x, y + 1);
                else
                    player[playerid].setDirection(2);
            }
             if (c.Equals("LEFT#"))
            {
                 if(player[playerid].getDirection()==3)
                     player[playerid].location = new Point(x-1, y);
                else
                    player[playerid].setDirection(3);
            }

           
        }*/









      
    }
}
