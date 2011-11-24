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
    class BouncingThing
    {
        public Vector2[] corners;
        public Vector2[] insideVec;
        public Vector2 origin;
        public Vector2 speed;
        public int numCorners;
        public float angularVelocity;
        public int width;
        public int radius;

        public BouncingThing()
        {
            //default
        }

        public BouncingThing(Vector2 pos, Vector2 vel, Vector2 spd)
        {
            //triangle
            this.origin = pos;
            this.numCorners = 3;
            this.width = 5;
            this.speed = spd;
            this.corners = new Vector2[numCorners];
            this.insideVec = new Vector2[numCorners];

            this.insideVec[0] = new Vector2(5,5);
            this.insideVec[1] = new Vector2(5,-5);
            this.insideVec[2] = new Vector2(-5,-5);

            this.radius = 5;

            for (int i = 0; i < numCorners; i++)
            {
                corners[i] = origin + insideVec[i];
            }
            

        }
        public BouncingThing(Vector2 pos, Vector2 vel, float angularVel, int spd)
        {
            //square
            this.origin = pos;
            this.numCorners = 4;
            this.width = 5;
            this.corners = new Vector2[numCorners];
            this.insideVec = new Vector2[numCorners];

            this.insideVec[0] = new Vector2(-5, 5);
            this.insideVec[1] = new Vector2(5, 5);
            this.insideVec[2] = new Vector2(5, -5);
            this.insideVec[3] = new Vector2(-5, -5);

            this.radius = 5;

            for (int i = 0; i < numCorners; i++)
            {
                corners[i] = origin + insideVec[i];
            }
        }

        public virtual void Move(GameTime gameTime, int maxX, int minX, int maxY, int minY)
        {
            origin.X += speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            origin.Y += speed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < numCorners; i++)
            {
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

        public virtual void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch, Texture2D boxText)
        {
        }
       

        public void DrawLine(SpriteBatch spritebatch, Texture2D boxTexture, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan(edge.Y / edge.X);
            if (edge.X < 0)
                angle = MathHelper.Pi + angle;

            spritebatch.Draw(boxTexture, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, Color.Red, angle, new Vector2(0, 0), SpriteEffects.None, 0);
        }
        public Vector2[] getNormals()
        {
            Vector2[] norms = new Vector2[numCorners];
            for (int i = 0; i < numCorners; ++i)
            {
                Vector2 temp3;
              
                if(i+1<numCorners)
                temp3 = corners[i] - corners[i + 1];
                else
                temp3 = corners[i] - corners[0];

                float temp = temp3.X;
                temp3.X = temp3.Y*-1;
                temp3.Y = temp;
                norms[i] = temp3;

            }
            return norms;
        }
    }
}
