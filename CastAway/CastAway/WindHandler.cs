using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CastAway
{
    
    public class WindHandler
    {
        
        private float windSpeed;


        public WindHandler()
        {
            windSpeed = GetWindSpeed();
        }



        public float GetWindSpeed()
        {
            // randomly generates a wind value between 5 & 12 kts

              System.Random random = new System.Random();
              return float.Parse(random.Next(5, 12).ToString());
        }



        public float GetWindSp()
        {
            return windSpeed;
        }




    }
}
