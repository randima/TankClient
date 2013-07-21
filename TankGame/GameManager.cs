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
        private int t;       
        private bool started ;
        private NetworkHandler com;
        private Player[] player;
             
        private static GameManager engine = new GameManager();
        
                
        public static GameManager getInstance()
        {
            //if (engine == null)
                //engine = new GameManager();
            return engine;
        }

        public GameManager()
        {
            Bricks = new List<Brick>();
            Water = new List<Point>();
            Stone = new List<Point>();
            player = new Player[5];
            started = false;
        }       

       public int playerid{get;set;}


        public void JoinGame()
        {    
            com = NetworkHandler.GetInstance();                           
            com.SendData("JOIN#");          
            reciverThread.DoWork += new DoWorkEventHandler(reciverThread_DoWork);
            reciverThread.RunWorkerAsync();
            
        }

        private void reciverThread_DoWork(object sender, DoWorkEventArgs e)
        {
            com.ReceiveData();
        }



       

        public void analyze(Object obj)
        {
           
            String msg = (String)obj;      
           // Console.WriteLine(engine.getWater().Count+"");
            msg=msg.TrimEnd('#');           
            String[] info = msg.Split(':');

            if (info[0].Equals("I"))
            {
                playerid = int.Parse(info[1].Substring(1));
                addBricks(info[2]);
                Console.WriteLine(info[2]);
                addStones(info[3]);
                Console.WriteLine(info[3]);
                addWater(info[4]);
                Console.WriteLine(info[4]);
                started = true;
                Console.WriteLine("");
                Console.WriteLine(playerid);
                Console.WriteLine(" game started");

            }
            /*else if (info[0].Equals("S"))
            {            
              
                
            }
            else if (info[0].Equals("G"))
            {
            }
            else if (info[0].Equals("L"))
            {
            }
            else if (info[0].Equals("C"))
            {
            }
            else
                Console.WriteLine("Error wrong info");*/



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

        public bool getGameState()
        {
            return started;
        }

        public void Test()
        {
            analyze((object)"I:P4:9,3;1,3;3,2;0,4;8,6;0,8;2,3;3,1:5,3;3,8;2,4;4,8;6,8;8,3;7,1;8,4:8,1;4,3;3,6;1,4;4,7;7,6;9,8;7,2;2,1;6,7#");
        }

        public void test2()
        {
            t++;
        }

        public int gett()
        {
            return t;
        }
    }
}
