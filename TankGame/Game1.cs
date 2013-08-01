using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//using System.Windows.Forms;

namespace TankGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TimeSpan time;
        Double gDelay=TimeSpan.FromMilliseconds(1010).TotalMilliseconds;
        GraphicsDevice device;
        Texture2D block,player;
        Texture2D sand;
        Texture2D tankcol;
        Texture2D tank;
        Texture2D water;
        Texture2D rock;
        Texture2D grass;
        Texture2D trunk;
        Texture2D life;
        Texture2D coin;
        Texture2D back,join,text;
        Color[] playerColr;
        GameManager engine;
        AI gAI;
        float playerScale;
        int mapResolution=500;
        int panelResolution=200;
        int gridsize = 10;
        int scrn_h, scrn_w, count=0;
        int size;        
        bool started=false,joinfailed = true,blink=true;
        SpriteFont font,fontStart;      
        
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = mapResolution+panelResolution;
            graphics.PreferredBackBufferHeight = mapResolution;
            graphics.IsFullScreen = false;
            playerColr = new Color[5];
            playerColr[0] = Color.Blue;
            playerColr[1] = Color.Red;
            playerColr[2] = Color.Orange;
            playerColr[3] = Color.Green;
            playerColr[4] = Color.Yellow;         
            size = mapResolution / gridsize;
            engine=GameManager.getInstance();
           
            //engine.JoinGame();            
            
            /*while (!engine.getGameState()) ;
            gAI.initAI();    */        
            //gAI.work();
            //engine.analyze((object)"I:P4:9,3;1,3;3,2;0,4;8,6;0,8;2,3;3,1:5,3;3,8;2,4;4,8;6,8;8,3;7,1;8,4:8,1;4,3;3,6;1,4;4,7;7,6;9,8;7,2;2,1;6,7#"); ;
            graphics.ApplyChanges();
            base.Initialize(); 
           
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            device = graphics.GraphicsDevice;
            block = Content.Load<Texture2D>("cell");
            font = Content.Load<SpriteFont>("GameFont1");
            player = Content.Load<Texture2D>("triangle");
            tank= Content.Load<Texture2D>("tank");
            tankcol = Content.Load<Texture2D>("tankcol");
            sand = Content.Load<Texture2D>("sand");
            rock = Content.Load<Texture2D>("stone");
            grass =  Content.Load<Texture2D>("grass");
            water = Content.Load<Texture2D>("water2");
            trunk= Content.Load<Texture2D>("brick");
            life = Content.Load<Texture2D>("life");
            coin = Content.Load<Texture2D>("gold");
            back = Content.Load<Texture2D>("tank-back");
            join = Content.Load<Texture2D>("join");
            fontStart = Content.Load<SpriteFont>("start");
            text = Content.Load<Texture2D>("Enter");
           
            playerScale = (float)size / tank.Width;
           


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            if (joinfailed)
            {
               KeyboardState keybState = Keyboard.GetState();
                if (keybState.IsKeyDown(Keys.Enter))
                {                   
                        Console.WriteLine("Enter press");
                        if (engine.JoinGame())
                        {
                            startGame();
                        }
                        else
                            joinfailed = true;
                    

                }
            }
           

            if (started)
            {
                Double diff = gameTime.TotalGameTime.TotalMilliseconds - time.TotalMilliseconds;
                int c, l;
                c = engine.getCoinList().Count;
                l = engine.getLifePacks().Count;
                engine.updateTime(gameTime.TotalGameTime);
                if (diff > gDelay)
                {
                    gAI.sendCommands();
                    time = gameTime.TotalGameTime;
                    
                    foreach (Player p in engine.getPlayers())
                    {
                        engine.getCoinList().RemoveAll(c => c.location.Equals(p.location));
                        engine.getLifePacks().RemoveAll(l => l.location.Equals(p.location));
                    }
                    
                    engine.getCoinList().RemoveAll(c => c.spawnTime.TotalMilliseconds + TimeSpan.FromMilliseconds(c.cooldown).TotalMilliseconds <= time.TotalMilliseconds);
                    engine.getLifePacks().RemoveAll(l => l.spawnTime.TotalMilliseconds + TimeSpan.FromMilliseconds(l.cooldown).TotalMilliseconds <= time.TotalMilliseconds);
                    if( engine.getCoinList().Count<c)//work only when coin list change                     
                        gAI.work();

                }
            }

          
                
            //Console.WriteLine(gameTime..TotalMilliseconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();            
            

            drawBack();
            if (started)
            {

                drawBack2();
                drawGrid();
                drawStone();
                drawBricks();
                drawWater();
                drawpath();
                drawCoins();
                drawPacks();                
                if (engine.getGameState())
                    drawPlayers();
            }
            else
                drawMainMenu();           
                
                   
                      
            spriteBatch.End();

            base.Draw(gameTime);
        }



        private void startGame()
        {          
            joinfailed = false;
            started = true;
            gAI = AI.getInstance();
            while (!engine.getGameState()) ;
            gAI.initAI();

        }


        private void drawMainMenu()
        {
            drawJoin();
            if (blink)
            {
                if (count > 70)
                {
                    count++;
                    if (count > 100)
                        count = 0;
                }
                else
                {

                    drawStart();
                    count++;
                }
            }
        }

        private void drawGrid()
        {
           
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                   spriteBatch.Draw(sand, new Rectangle(i * size, j * size, size - 1, size - 1), Color.White);
                }
            }
        }

        private void drawWater()
        {
            if (engine.getWater().Count>0)
            {
                foreach (Point p in engine.getWater())
                {
                    spriteBatch.Draw(water, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.White);
                }
            }
        }

        private void drawStone()
        {
            if (engine.getStone().Count > 0)
            {
                foreach (Point p in engine.getStone())
                {
                    spriteBatch.Draw(rock, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.White);
                }
            }
        }

        private void drawBricks()
        {
               foreach (Brick b in engine.getBricks())
                {
                    Point p = b.location;
                   if(b.damage!=4)
                    spriteBatch.Draw(trunk, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.White);
                }
            
        }

        private void drawPlayers()
        {
            int i=0;
                foreach (Player p in engine.getPlayers())
                {
                    Vector2 orgin = new Vector2(60f, 50f);
                    Vector2 position = new Vector2(p.location.X * size + size / 2, p.location.Y * size + size / 2);
                    if (!p.dead)
                    {
                        spriteBatch.Draw(tank, position, null, Color.White, p.rotation(), orgin, playerScale, SpriteEffects.None, 0);
                        spriteBatch.Draw(tankcol, position, null, playerColr[i], p.rotation(), orgin, playerScale, SpriteEffects.None, 0);
                    }
                    i++;
                
                }
                       
        }

        public void drawCoins()
        {
            if (engine.getCoinList().Count > 0)
            {
                foreach (Coin c in engine.getCoinList())
                {
                    spriteBatch.Draw(coin, new Rectangle(c.location.X * size, c.location.Y * size, size - 1, size - 1), Color.White);                    
                }
            }
        }

        public void drawPacks()
        {
            if (engine.getLifePacks().Count > 0)
            {
                foreach (LifePack lp in engine.getLifePacks())
                {
                    spriteBatch.Draw(life, new Rectangle(lp.location.X * size, lp.location.Y * size, size - 1, size - 1), Color.White);
                    //Console.WriteLine(c.spawnTime.TotalMilliseconds);
                }
            }
        }


        public void drawBack()
        {
            spriteBatch.Draw(back, new Rectangle(0, 0, mapResolution+panelResolution, mapResolution), Color.White);
        }

        public void drawBack2()
        {
            spriteBatch.Draw(block, new Rectangle(0, 0, mapResolution , mapResolution), Color.White);
        }

        public void drawJoin() 
        {
            spriteBatch.Draw(join, new Rectangle(0, 0, mapResolution + panelResolution, mapResolution), Color.White);

        }


        public void drawStart()
        {
           /* float i, j;
            i = 548 * (mapResolution + panelResolution) / 1280;
            j=376*mapResolution/720;
            spriteBatch.DrawString(fontStart,"Press Enter", new Vector2(i,j), Color.White);*/

            spriteBatch.Draw(text, new Rectangle(0, 0, mapResolution + panelResolution, mapResolution), Color.White);
        }

        public void drawpath()
        {
            if (gAI.getpath().Count > 0)
            {
                foreach (Point p in gAI.getpath())
                {
                    spriteBatch.Draw(sand, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.Green);
                }
            }
        }












       private void drawGridNo()
        {
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {

                    spriteBatch.DrawString(font, i + "," + j, new Vector2(i * size, j * size), Color.Black);

                }
            }
        }

        private void drawTime(int t)
        {
            spriteBatch.DrawString(font, t+"", new Vector2(0, 0), Color.Black);
            
        }


    }
}
