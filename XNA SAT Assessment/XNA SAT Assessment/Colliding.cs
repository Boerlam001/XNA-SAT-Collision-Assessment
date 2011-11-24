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
    class Colliding
    {
        static BouncingThing object1;
        static BouncingThing object2;
        Vector2[] obj1Norms;
        Vector2[] obj2Norms;
        float[] obj1Projections;
        float[] obj2Projections;

        public Colliding(/*BouncingThing obj1, BouncingThing obj2*/)
        {
            obj1Projections = new float [4];
            obj2Projections = new float [4];
        }

        public bool CheckForSATCollisions(BouncingThing obj1, BouncingThing obj2)
        {
            object1 = obj1;
            object2 = obj2;
           
            Normalize();

            if (CheckNormals(obj1Norms))
                return false;
            if(CheckNormals(obj2Norms))
                return false;
            else
                return true;
        }



        protected void Normalize()
        {
            obj1Norms = object1.getNormals();
            obj2Norms = object2.getNormals();
        }
        public bool CheckNormals(Vector2[] ObjNorms)
        {
            float minObj1 = 0;
            float maxObj1 = 0;
            float minObj2 = 0;
            float maxObj2 = 0;

            for (int j = 0; j < ObjNorms.Length; j++)
            {
                for (int i = 0; i < object1.numCorners; ++i)
                {
                    obj1Projections[i] = Vector2.Dot(object1.corners[i], ObjNorms[j]);
                    if (i == 0)
                        maxObj1 = obj1Projections[i];
                    if (obj1Projections[i] > maxObj1)
                        maxObj1 = obj1Projections[i];
                    if (i == 0)
                        minObj1 = obj1Projections[i];
                    else if (obj1Projections[i] < minObj1)
                        minObj1 = obj1Projections[i];

                }
                for (int i = 0; i < object2.numCorners; ++i)
                {
                    obj2Projections[i] = Vector2.Dot(object2.corners[i], ObjNorms[j]);

                    if (i == 0)
                        maxObj2 = obj2Projections[i];
                    if (obj2Projections[i] > maxObj2)
                        maxObj2 = obj2Projections[i];
                    if (i == 0)
                        minObj2 = obj2Projections[i];
                    else if (obj2Projections[i] < minObj2)
                        minObj2 = obj2Projections[i];
                }

                if (minObj1 > maxObj2)
                {
                    return true;//separating
                }
              
            }
               return false;
        }
       

    }
}
