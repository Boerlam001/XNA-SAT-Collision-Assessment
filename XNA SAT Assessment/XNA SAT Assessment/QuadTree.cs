using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_SAT_Assessment
{
    class QuadTree
    {
        //attempted quad tree - doesnt work + not finished
       
        BouncingThing[] objects;
        int[] corners;
        BouncingThing[] objA;
        BouncingThing[] objB;
        BouncingThing[] objC;
        BouncingThing[] objD;
        Cell[] cells;

        

        Colliding cldMgr;

        public QuadTree(int MaxX, int MinX, int MaxY, int MinY , BouncingThing[] things)
        {
            cldMgr = new Colliding();
            cells = new Cell[16];
            objects = things;
            corners = new int[4];
            corners[0] = MaxX;
            corners[1] = MinX;
            corners[2] = MaxY;
            corners[3] = MinY;
            int max = things.Length;
            objA = new BouncingThing[max];
            objB = new BouncingThing[max];
            objC = new BouncingThing[max];
            objD = new BouncingThing[max];

            for (int i = 0; i < max; i++)
            {
                objA[i] = new BouncingThing();
                objB[i] = new BouncingThing();
                objC[i] = new BouncingThing();
                objD[i] = new BouncingThing();
            }
            
        }

        public void Optimise(int[] crnrs, int[] count)
        {
            subDivide(crnrs,  count);
            int length = objects.Length;
            checkQuarters(objA, length);
            checkQuarters(objB, length);
            checkQuarters(objC, length); 
            checkQuarters(objD, length);

        }

        public void subDivide(int[] crnrs, int[] count)
        {
            //int[] count;
            //count = new int[4];
            count[0] =0;
            count[1] =0;
            count[2] = 0;
            count[3] = 0;

            corners[0] = crnrs[0];
            corners[1] = crnrs[1];
            corners[2] = crnrs[2];
            corners[3] = crnrs[3];

            int quarterX = corners[0]/2;
            int quarterY = corners[2]/2;
            int arrAcnt = 0;
            int arrBcnt = 0;
            int arrCcnt = 0;
            int arrDcnt = 0;

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].origin.X < quarterX)
                {
                    if (objects[i].origin.Y < quarterY)
                    {
                        count[2]++;
                        
                        objC[arrCcnt] = objects[i];
                        arrCcnt++;
                    }
                    else
                    {
                        count[0]++;
                        objC[arrAcnt] = objects[i];
                        arrAcnt++;
                    }
                }
                else
                {
                    if (objects[i].origin.Y < quarterY)
                    {
                        count[3]++;
                        objC[arrDcnt] = objects[i];
                        arrDcnt++;
                    }
                    else
                    {
                        count[1]++;
                        objC[arrBcnt] = objects[i];
                        arrBcnt++;
                    }
                }
            }
            int[] cols = new int[6];
            int[] rows = new int[6];
            int width = crnrs[1] - crnrs[0];
            int height = crnrs[3] - crnrs[2];

            for (int i = 0; i < 6; i++)
            {
                cols[i] = crnrs[1] + width * i;
                rows[i] = crnrs[3] + height * i;
            }
            int[] c = new int [4];

          /*  for (int ind = 0; ind < 16; ind++)
            {
                for(int h = 0; h<4;h++)
                {
                    c[h] = ;
                }
                cells[ind] = new Cell(
            }*/
        

        }

        public void checkQuarters(BouncingThing[] obj, int lngth)
        {
            
            for (int i = 0; i < lngth; ++i)
           {
               for (int j = i + 1; j < lngth; ++j)
               {
                   Vector2 pos = obj[i].origin - obj[j].origin;
                   Vector2 vel = obj[i].speed - obj[j].speed;
                   if (Vector2.Dot(pos, vel) < 0)// if not movin in the same direction
                   {
                       if (cldMgr.CheckForSATCollisions(obj[i], obj[j]))
                       {
                           //switch velocities
                           Vector2 tempVel = obj[i].speed;
                           obj[i].speed = obj[j].speed;
                           obj[j].speed = tempVel;
                       }
                   }

               }
           }
            
        }
    }
   
}
