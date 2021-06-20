using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAntennaNeverGoesOffline
{
    class MangoUtility
    {
        private float electricalGenerationFactor;
        private float factoredTime = 0F;

        public MangoUtility(float _electricalGenerationFactor)
        {
            electricalGenerationFactor = _electricalGenerationFactor;
        }

        public float SetTime()
        {
            // antenna pI range from 0.6 to 1.6. (1 eGF = 0.01 pI)
            // factored time is inverse eGF within range 0.6 to 1.6


            if (electricalGenerationFactor > 100)
            {
                electricalGenerationFactor = 100;
            }
            else if (electricalGenerationFactor < 1)
            {
                electricalGenerationFactor = 1;
            }

            if (electricalGenerationFactor == 100)
            {
                factoredTime = 0.15f;
            }
            else if (electricalGenerationFactor < 100 && electricalGenerationFactor > 90)
            {
                factoredTime = 0.25f;
            }
            else if (electricalGenerationFactor <= 90 && electricalGenerationFactor > 80)
            {
                factoredTime = 0.35f;
            }
            else if (electricalGenerationFactor <= 80 && electricalGenerationFactor > 70)
            {
                factoredTime = 0.45f;
            }
            else if (electricalGenerationFactor <=70 && electricalGenerationFactor > 60)
            {
                factoredTime = 0.55f;
            }
            else if (electricalGenerationFactor <= 60 && electricalGenerationFactor > 50)
            {
                factoredTime = 0.65f;
            }
            else if (electricalGenerationFactor <= 50 && electricalGenerationFactor > 40)
            {
                factoredTime = 0.75f;
            }
            else if (electricalGenerationFactor <= 40 && electricalGenerationFactor > 30)
            {
                factoredTime = 0.85f;
            }
            else if (electricalGenerationFactor <= 30 && electricalGenerationFactor > 20)
            {
                factoredTime = 0.95f;
            }
            else if (electricalGenerationFactor <= 20 && electricalGenerationFactor > 10)
            {
                factoredTime = 1.05f;
            }
            else
            {
                factoredTime = 1.15f;
            }

            return factoredTime;


        }

      


    }
}
