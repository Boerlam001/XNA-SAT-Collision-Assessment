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

namespace XNA_SAT_Assessment
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BouncingThing[] bouncingThings;
        ContentManager content;
        GraphicsDevice device;
        Texture2D myTexture;
        Texture2D boxTexture;

        const int ELEMENTS = 20;
        const int TEXTUREWIDTH = 40;
        const int TEXTUREHEIGHT = 40;
        int maxX;
        int minX;
        int maxY;
        int minY;
        int[] boundaries;
        int[] count;
        double frameCount;
        double frameTime;
        double frameRate;

        Colliding CollisionMgr;
        QuadTree quad;

        public Game1()
        {
            this.IsFixedTimeStep = false;
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
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
            // TODO: Add your initialization logic here


            graphics.SynchronizeWithVerticalRetrace = false;
            

            frameRate = 0;
            frameCount = 0;
            frameTime = 0;
            CollisionMgr = new Colliding();
            device = graphics.GraphicsDevice;

            boundaries = new int[4];
            count = new int[4];
            bouncingThings = new BouncingThing[ELEMENTS];

            Vector2 pos = new Vector2 (5,5);
            Random rand = new Random();

            boundaries[0] = maxX = graphics.GraphicsDevice.Viewport.Width;
            boundaries[1] = minX = 0;
            boundaries[2] = maxY = graphics.GraphicsDevice.Viewport.Height;
            boundaries[3] = minY = 0;

            for (int i = 0; i < 20; i++)
            {
                if (i < 10)
                {
                    bouncingThings[i] = new BouncingTriangle(new Vector2(rand.Next(minX, maxX - TEXTUREWIDTH),
                       rand.Next(minY, maxY - TEXTUREHEIGHT)),
                       new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)),
                       rand.Next(5, 20),
                       new Vector2(rand.Next(5, 20), rand.Next(5, 20)));
                }
                else
                    bouncingThings[i] = new BouncingBox(new Vector2(rand.Next(minX, maxX - TEXTUREWIDTH),
                        rand.Next(minY, maxY - TEXTUREHEIGHT)),
                        new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)),
                         rand.Next(1, 5),
                         new Vector2(rand.Next(5, 10), rand.Next(5, 10)));
            }

            quad = new QuadTree(maxX, minX, maxY, minY, bouncingThings);
              
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
            myTexture = Content.Load<Texture2D>("triangleSprite");
            boxTexture = new Texture2D(GraphicsDevice, 1, 1);
            boxTexture.SetData<Color>(new Color[] { Color.White });
         
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
            for (int i = 0; i < ELEMENTS; ++i)
            {
                bouncingThings[i].Move(gameTime, maxX, minX, maxY, minY); 
            }


            for (int i = 0; i < ELEMENTS; ++i)
            {
                for (int j = i + 1; j < ELEMENTS; ++j)
                {
                    Vector2 distanceBetween = bouncingThings[i].origin - bouncingThings[j].origin;
                    Vector2 vel = bouncingThings[i].speed - bouncingThings[j].speed;
                    if (Vector2.Dot(distanceBetween, vel) < 0)// if not movin in the same direction
                    {
                        int radii = bouncingThings[i].radius + bouncingThings[j].radius;//bouncingBox 

                        if (distanceBetween.X < bouncingThings[i].origin.X + radii || distanceBetween.Y < bouncingThings[i].origin.Y + radii)
                        {
                            if (CollisionMgr.CheckForSATCollisions(bouncingThings[i], bouncingThings[j]))
                            {
                                //switch velocities
                                Vector2 tempVel = bouncingThings[i].speed;
                                bouncingThings[i].speed = bouncingThings[j].speed;
                                bouncingThings[j].speed = tempVel;
                            }
                        }
                    }

                }
            }

           // quad.Optimise(boundaries, count);

          /*  frameCount += 1;
            frameTime += (double)gameTime.ElapsedGameTime.Milliseconds;
            if (frameTime >= 1000)
            {
                //dont think this works 
                frameRate = frameCount;
                frameCount = 0;
                frameTime = 0;
            }*/
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
           
            for (int i = 0; i < ELEMENTS; ++i)
            {
                bouncingThings[i].Draw(gameTime, device, spriteBatch, boxTexture);
            }


            this.Window.Title = "Frame rate: " + frameRate.ToString();
            base.Draw(gameTime);
        }
    }
}
