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

namespace TankGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D block;
        GameManager engine;
        int resolution=500;
        int gridsize = 10;
        int size;        
        bool started=false;
        SpriteFont font;      
        
        

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
            graphics.PreferredBackBufferWidth = resolution;
            graphics.PreferredBackBufferHeight = resolution;
            graphics.IsFullScreen = false;
            size = resolution / gridsize;
            engine=GameManager.getInstance();
            engine.JoinGame();              
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();            

            // TODO: Add your update logic here
            engine.test2();
            
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
            drawGrid();

           /*if(engine.getGameState() ==false)
            {
                Console.WriteLine("false");
            }
            else
            {
                Console.WriteLine("true");
            }*/

                drawStone();
                drawBricks();
                drawWater();
                //drawGridNo();
            
            drawTime(engine.gett() );
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void drawGrid()
        {
           
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {
                   spriteBatch.Draw(block, new Rectangle(i * size, j * size, size - 1, size - 1), Color.Azure);
                }
            }
        }

        public void drawWater()
        {
            if (engine.getWater().Count>0)
            {
                foreach (Point p in engine.getWater())
                {
                    spriteBatch.Draw(block, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.Blue);
                }
            }
        }

        public void drawStone()
        {
            if (engine.getStone().Count > 0)
            {
                foreach (Point p in engine.getStone())
                {
                    spriteBatch.Draw(block, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.Gray);
                }
            }
        }

        public void drawBricks()
        {
            if (engine.getBricks().Count > 0)
            {
                foreach (Brick b in engine.getBricks())
                {
                    Point p = b.location;
                    spriteBatch.Draw(block, new Rectangle(p.X * size, p.Y * size, size - 1, size - 1), Color.Brown);
                }
            }
        }

        public void drawGridNo()
        {
            for (int i = 0; i < gridsize; i++)
            {
                for (int j = 0; j < gridsize; j++)
                {

                    spriteBatch.DrawString(font, i + "," + j, new Vector2(i * size, j * size), Color.Black);

                }
            }
        }

        public void drawTime(int t)
        {
            spriteBatch.DrawString(font, t+"", new Vector2(0, 0), Color.Black);
            
        }


    }
}
