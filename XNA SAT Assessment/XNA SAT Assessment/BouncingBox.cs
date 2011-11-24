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
    class BouncingBox: BouncingThing
    {
 
        public BouncingBox(Vector2 pos, Vector2 vel, float angularVel, Vector2 spd)
        {
            this.origin = pos;
            this.numCorners = 4;
            this.width = 5;
            this.corners = new Vector2[numCorners];
            this.insideVec = new Vector2[numCorners];

            this.insideVec[0] = new Vector2(-5, 5);
            this.insideVec[1] = new Vector2(5, 5);
            this.insideVec[2] = new Vector2(5, -5);
            this.insideVec[3] = new Vector2(-5, -5);

            for (int i = 0; i < numCorners; i++)
            {
                corners[i] = origin + insideVec[i];
            }
        
            this.angularVelocity = MathHelper.ToRadians(angularVel);
            this.speed = spd;

        }
     
        public override void Move(GameTime gameTime, int maxX, int minX, int maxY, int minY)
        {
            //move and rotate
            //basic movement
            Matrix m = Matrix.CreateRotationZ(angularVelocity);

            for (int i = 0; i < 4; i++)
            {


                origin.X += speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                origin.Y += speed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                insideVec[i] = Vector2.Transform(insideVec[i], m);
                
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
        public void Update()
        {
            
        }
       public override void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch, Texture2D boxText)
        {
            spriteBatch.Begin();

            DrawLine(spriteBatch, boxText, corners[0], corners[1]);
            DrawLine(spriteBatch, boxText, corners[1], corners[2]);
            DrawLine(spriteBatch, boxText, corners[2], corners[3]);
            DrawLine(spriteBatch, boxText, corners[3], corners[0]);

            spriteBatch.End();

        }
       
    }
}
