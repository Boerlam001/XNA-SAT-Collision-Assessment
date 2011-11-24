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
    class BouncingTriangle: BouncingThing
    {
        /*Create an XNA Project.
          Add a class called "BouncingTriangle".
          The BouncingTriangle class will be reponsible for drawing a Trianglular shape in the on the screen.
          It is important that this class keep track of the locations of the corners of the object!
          Later on you will need to be able to get the screen coordinates of the corners.
          The sprite should be given some initial velocity and position and move across the screen. 
          The Triangle should bounce off the edges of the screen.
          Add 10 BouncingTrangles to your project with different initial conditions.
         */
       

        public BouncingTriangle( Vector2 pos, Vector2 vel, int angularVel,Vector2 spd)
        {
            
            this.origin = pos;
            this.numCorners = 3;
            this.width = 5;
            this.speed = spd;
            this.angularVelocity = MathHelper.ToRadians(angularVel);

            this.corners = new Vector2[numCorners];
            this.insideVec = new Vector2[numCorners];


            this.insideVec[0] = new Vector2(5, 5);
            this.insideVec[1] = new Vector2(5, -5);
            this.insideVec[2] = new Vector2(-5, -5);

            for (int i = 0; i < numCorners; i++)
            {
                corners[i] = origin + insideVec[i];
            }
        }

        public override void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch, Texture2D boxText)
        {
            //draw method
          
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            DrawLine(spriteBatch, boxText, corners[0], corners[1]);
            DrawLine(spriteBatch, boxText, corners[1], corners[2]);
            DrawLine(spriteBatch, boxText, corners[2], corners[0]);

            spriteBatch.End();
           
        }
        


        public override void Move(GameTime gameTime, int maxX, int minX, int maxY, int minY)
        {
          
            for (int i = 0; i < numCorners; i++)
            {

                origin.X += speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                origin.Y += speed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                corners[i] = origin + insideVec[i];

                //if reach any edges
                if (corners[i].X < minX)
                {
                    speed.X *= -1;

                }
                if (corners[i].X + width > maxX)
                {
                    speed.X *= -1;
                }
                if (corners[i].Y < minY)
                {
                    speed.Y *= -1;
                }
                if (corners[i].Y + width > maxY)
                {
                    speed.Y *= -1;
                }
            }
        }
    }
}
