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
using AI;
//using test;
using System.Threading;


namespace WindowsGame3
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D brickWall;
        Texture2D stoneWall;
        Texture2D water;
        Texture2D coins;
        Texture2D healthPack;
        Texture2D [] tank=new Texture2D[5];
        Texture2D backgroundTexture;
        Texture2D detailTexutre;
        Texture2D bullet;
        int screenWidth;
        int screenHeight;
        SpriteFont spfont;
        SpriteFont startfont;
        SpriteFont playerDfont;

        int border = 20;
        String[] direct = { "UP#", "RIGHT#", "DOWN#", "LEFT#" };
        String[] color = { "Yellow", "Blue", "Red", "Black", "Green" };
        double pastGametime = 0;
        //SearchNode nextPath;
        Communicator com = Communicator.GetInstance();
        bool start = false;

        //size of world
        public int sizeWorld=20;
        //ai
        World world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            
            System.Console.WriteLine("////");
            Communicator com = Communicator.GetInstance();
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            //buttontexture = Content.Load<Texture2D>("button");
            detailTexutre = Content.Load<Texture2D>("detail");
            background = Content.Load<Texture2D>("Bitmap1");
            brickWall = Content.Load<Texture2D>("Bitmap2");
            stoneWall = Content.Load<Texture2D>("Bitmap3");
            water = Content.Load<Texture2D>("Bitmap4");
            coins = Content.Load<Texture2D>("Bitmap5");
            healthPack = Content.Load<Texture2D>("health");
            spfont = Content.Load<SpriteFont>("Arial");
            startfont = Content.Load<SpriteFont>("start");
            playerDfont = Content.Load<SpriteFont>("playerDetail");
            backgroundTexture = Content.Load<Texture2D>("background");
            tank[0] = Content.Load<Texture2D>("p1");
            tank[1] = Content.Load<Texture2D>("p2");
            tank[2] = Content.Load<Texture2D>("p3");
            tank[3] = Content.Load<Texture2D>("p4");
            tank[4] = Content.Load<Texture2D>("p5");
            bullet = Content.Load<Texture2D>("bullet1");
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight =700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Window.Title = "Tank Game-Panzer Challenge";
            backgroundTexture = Content.Load<Texture2D>("background");

            
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
           

            // ai
            world = new World(sizeWorld,1, sizeWorld);
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
            pressEnter();
            // TODO: Add your update logic here
           
            
            if (start)
            {
                System.Console.WriteLine("start receiving data");
                com.ReceiveData();
                com.divide();
                System.Console.WriteLine("received data");
            }
            
            if (com.startG)
            {
                updateWorld();
                updatePilse(gameTime);
                double ptime = gameTime.TotalGameTime.TotalSeconds;
                Player nplayer = GetNextMove();
                double ntime = gameTime.TotalGameTime.TotalSeconds;

                if (nplayer != com.me)
                
                if (pastGametime +.4-(ntime-ptime)<= gameTime.TotalGameTime.TotalSeconds)
                {

                  
                    com.SendData(direct[nplayer.direction]);
                    Console.Out.WriteLine("send direction :"+direct[nplayer.direction]);
                    pastGametime = gameTime.TotalGameTime.TotalSeconds;
                }
            }
           
            
          
            

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
             DrawScenery();
             spriteBatch.End();
             showPlayerDetail();
             spriteBatch.Begin();
            
             spriteBatch.End();
            
            // TODO: Add your drawing code here

            if (com.startG && !com.finishG)
            {
                spriteBatch.Begin();

                
                
                DrawEmptyCell();
                DrawObstacles();
                DrawPlayers();

                DrawPiles();
                spriteBatch.End();
            }
            else if (com.finishG)
                finishGame();

           
            base.Draw(gameTime);



        }
        private void DrawScenery()
        {
            Console.WriteLine("draw background");

            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            spriteBatch.DrawString(startfont,"Press Enter to Start", new Vector2(screenWidth/2-160, screenHeight / 2), Color.White);
            
        }
        private void DrawEmptyCell()
        {
            Console.WriteLine("Draw empty cell");
            for (int i = 0; i < sizeWorld; i++)
            {
                for (int j = 0; j < sizeWorld; j++)
                {
                    spriteBatch.Draw(background, new Rectangle(i * 30+border, j * 30+border, 30, 30), Color.White);
                    spriteBatch.DrawString(spfont, (i*sizeWorld + j).ToString(), new Vector2((i * 30+border + 8), (j * 30+border + 10)), Color.Black);

                }
            }
        }
        private void DrawObstacles() 
        {
            Console.WriteLine("draw obstacles");
            foreach (Cell acell in com.stoneWallList)
            {
                spriteBatch.Draw(stoneWall, new Rectangle(acell.X * 30+border, acell.Y * 30+border, 30, 30), Color.White);
            }
            foreach (Cell acell in com.waterList)
            {
                spriteBatch.Draw(water, new Rectangle(acell.X * 30+border, acell.Y * 30+border, 30, 30), Color.White);
            }
            foreach (Cell acell in com.brickWallList)
            {
                if(acell.health>0)
                spriteBatch.Draw(brickWall, new Rectangle(acell.X * 30+border, acell.Y * 30+border, 30, 30), Color.White);
            }
        }
        private void DrawPlayers()
        {
            Console.Out.WriteLine("Draw players");
            Vector2 origin;
            Vector2 screenpos;
            int ti = 0;
            float rotation = 0;
            
            foreach (Player aplayer in com.playerList)
            {
                rotation = aplayer.direction * MathHelper.Pi / 2;
                origin.X = tank[2].Width / 2;
                origin.Y = tank[2].Height / 2;
                screenpos.X = 30 * (aplayer.x) + 15+border;
                screenpos.Y = 30 * (aplayer.y) + 15+border;
                // spriteBatch.Draw(tank[ti], new Rectangle(aplayer.x * 30, aplayer.y * 30, 30, 30),Color.White);
                spriteBatch.Draw(tank[ti], screenpos, null, Color.White, rotation, origin, .6f, SpriteEffects.None, 0f);
                ti++;
            }
        }
        private void DrawPiles()
        {
            Console.WriteLine("Draw coin piles and health packs");
            if (com.coinPileList != null)
                foreach (CoinPile apile in com.coinPileList)
                {
                    if (apile.live)
                        foreach (Player aplayer in com.playerList)
                        {
                            if (aplayer.x == apile.X && aplayer.y == apile.Y)
                                apile.live = false;
                        }
                    if (apile.live)
                        spriteBatch.Draw(coins, new Rectangle(apile.X * 30+border, apile.Y * 30+border, 30, 30), Color.White);

                }
            if (com.healthPackList != null)
                foreach (HealthPack apack in com.healthPackList)
                {
                    if (apack.live)
                        foreach (Player aplayer in com.playerList)
                        {
                            if (aplayer.x == apack.X && aplayer.y == apack.Y)
                                apack.live = false;
                        }
                    if (apack.live)
                        spriteBatch.Draw(healthPack, new Rectangle(apack.X * 30+border, apack.Y * 30+border, 30, 30), Color.White);
                }
        }
        private void pressEnter()
        {
            Console.WriteLine("enter the game");
            Console.Out.WriteLine("send JOIN# message");
            KeyboardState keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Enter))
            {
                if(!start)
                com.SendData("JOIN#");
                start = true;
            }
        }
        private void updatePilse(GameTime gameTime)
        {
            Console.Out.WriteLine("update health packs and coin piles");
            foreach (CoinPile apile in com.coinPileList)
            {
                if (apile.startTime == 0 && apile.live)
                    apile.startTime = gameTime.TotalGameTime.TotalMilliseconds;
                if (apile.startTime + apile.lt <= gameTime.TotalGameTime.TotalMilliseconds)
                    apile.live = false;

            }
            foreach (HealthPack apack in com.healthPackList)
            {
                if (apack.startTime == 0 && apack.live)
                    apack.startTime = gameTime.TotalGameTime.TotalMilliseconds;
                if (apack.startTime + apack.lt <= gameTime.TotalGameTime.TotalMilliseconds)
                    apack.live = false;

            }
        }
        private void updateWorld()
        {
            Console.Out.WriteLine("update data for the AI");
            world = new World(sizeWorld,1, sizeWorld);
            foreach (Cell acell in com.stoneWallList)
            {
                if(acell!=null)
                world.MarkPosition(new Point3D(acell.X, 0,acell.Y), true);
            }
            foreach (Cell acell in com.waterList)
            {
                if (acell != null)
                world.MarkPosition(new Point3D(acell.X, 0,acell.Y), true);
            }
            
            foreach (Cell acell in com.brickWallList)
            {
                if (acell != null)
                world.MarkPosition(new Point3D(acell.X, 0,acell.Y), true);
            }
            foreach (Player aplayer in com.playerList)
            {
                if(aplayer!=null)
                world.MarkPosition(new Point3D(aplayer.x, 0,aplayer.y), true);
            }

        }
        private Player GetNextMove()
        {
            Console.Out.WriteLine("Get next move");
            int mincost = int.MaxValue;
            SearchNode cpath= new SearchNode(new Point3D(com.me.x, 0,com.me.y), 0, 0,null);
            SearchNode path = new SearchNode(new Point3D(com.me.x, 0,com.me.y), 0, 0, null);
            foreach (CoinPile apile in com.coinPileList)
            {

                if (apile.live)
                {
                    path = PathFinder.FindPath(world, new Point3D(com.me.x,0, com.me.y), new Point3D(apile.X,0, apile.Y));
                    if (path != null)
                        if (path.cost < mincost)
                        {
                            mincost=path.cost;
                            cpath = path;
                        }

                }
            }
            if (cpath == null||cpath.next==null)
                return com.me;
            else
                return new Player(com.meNumber, cpath.next.position.X, cpath.next.position.Z, nextDirection(cpath));
            

            

        }
        private int nextDirection(SearchNode sn)
        {
            int ax = sn.next.position.X;
            int ay = sn.next.position.Z;
            int ad = 0;
            if ((ax - com.me.x) > 0)
                ad = 1;
            else if ((ax - com.me.x) < 0)
                ad = 3;
            else
            {
                if ((ay - com.me.y) < 0)
                    ad = 0;
                else
                    ad = 2;

            }
            return ad;

        }
        private void showPlayerDetail()
        {
            Console.Out.WriteLine("show player details");
            spriteBatch.Begin();
            if (com.playerList!=null)
            {
                spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30, 70 + 0 * 30, 70, 30), Color.White);
                spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30 + 70, 70, 70, 30), Color.White);
                spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30 + 140, 70, 70, 30), Color.White);
                spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30 + 210, 70, 70, 30), Color.White);
                spriteBatch.DrawString(playerDfont, "Player", new Vector2((20 + 2) * 30, 70), Color.Yellow);
                spriteBatch.DrawString(playerDfont, "Coins", new Vector2((20 + 2) * 30 + 70, 70), Color.Yellow);
                spriteBatch.DrawString(playerDfont, "Points", new Vector2((20 + 2) * 30 + 140, 70), Color.Yellow);
                spriteBatch.DrawString(playerDfont, "Health", new Vector2((20 + 2) * 30 + 210, 70), Color.Yellow);

                spriteBatch.Draw(detailTexutre, new Rectangle((20 + 3) * 30 , 400, 250, 50), Color.White);
                spriteBatch.DrawString(startfont, "My Number : "+com.me.number.ToString(), new Vector2((20 + 3) * 30+30, 400), Color.Yellow);
            }
            int pn = 0;
            if (com.playerList != null)
                foreach (Player aplayer in com.playerList)
                {

                     spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30, 100 + pn * 30, 70, 30), Color.White);
                    spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30 + 70, 100 + pn * 30, 70, 30), Color.White);
                    spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30 + 140, 100 + pn * 30, 70, 30), Color.White);
                    spriteBatch.Draw(detailTexutre, new Rectangle((20 + 2) * 30 + 210, 100 + pn * 30, 70, 30), Color.White);
                    spriteBatch.DrawString(spfont,aplayer.number.ToString()+" ("+color[aplayer.number]+")",new Vector2((20 + 2) * 30+10, 100 + pn * 30),Color.Yellow);
                    spriteBatch.DrawString(playerDfont, aplayer.coins.ToString(), new Vector2((20 + 2) * 30+70, 100 + pn * 30), Color.Yellow);
                    spriteBatch.DrawString(playerDfont, aplayer.points.ToString(), new Vector2((20 + 2) * 30+140, 100 + pn * 30), Color.Yellow);
                    spriteBatch.DrawString(playerDfont, aplayer.health.ToString(), new Vector2((20 + 2) * 30+210, 100 + pn * 30), Color.Yellow);
                    //spriteBatch.Draw(background, new Rectangle(i * 30, j * 30, 30, 30), Color.White);
                    //spriteBatch.DrawString(spfont, (i * sizeWorld + j).ToString(), new Vector2((i * 30 + 8), (j * 30 + 10)), Color.Black);

                    pn++;
                }
           
                spriteBatch.End();
        }
        private void finishGame()
        {

            Console.Out.WriteLine("game finsih");
           // spriteBatch.Draw(backgroundTexture, new Rectangle(screenWidth / 2 - 160,, 100 + pn * 30, 70, 30), Color.White);
            spriteBatch.DrawString(startfont, "Game Finish", new Vector2(screenWidth / 2 - 100, screenHeight / 2), Color.White);
            

        }

        

       

    }
}
