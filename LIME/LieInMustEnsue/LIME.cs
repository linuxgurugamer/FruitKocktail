using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LieInMustEnsue
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public partial class LIME : MonoBehaviour
    {
        // this is the original LIME setting, 1hr after stock sunrise
        [KSPField]
        public double sunnyTime;

        // this is the stock sunrise time
        [KSPField]
        public double sunriseTime;

        // this is the sunset time based on the sun hitting the horizon
        [KSPField]
        public double sunsetTime;

        // this is midnight, and also the base time of a day
        [KSPField]
        public double midnightTime;

        // the current setting
        [KSPField(isPersistant = true)]
        public int storedMode = 1;

        // the new setting
        public static int newMode = 1;


        public void Start()
        {
            // day starts (0.0) @ 2:45 (the game's midnight). Stock sunrise is 0.3 @ 4hr 33. Therefore, the difference in minutes
            // between these two is 108 minutes. There are 6 hrs in a day (or 360 minutes), and the timeOfDawn variable goes up
            // to 1.0. Therefore, the total minutes in a day expressed as a decimal of 1 = 1 / 360 = 0.0028. In other words, 
            // for every minute past the game's midnight ( 0.0 @ 2:45 ), timeOfDawn increases by 0.00277e. Therefore, we can calculate
            // the times for events as follows: 
            // midnight = 0.0
            // sunrise (stock) is 108 mins * 0.00277e = 0.3 (correct)
            // sunnyTime (1hr later than sunrise so 108 mins + 60 mins) is 168 * 0.00277e = 0.467
            // sunset (approx 1hr 8 mins (or 7hrs 8 mins if not modular)) = 263 mins * 0.00277e = 0.736 

            sunriseTime = 0.3;
            sunnyTime = 0.467;
            sunsetTime = 0.736;
            midnightTime = 0.0;

            KSP.UI.UIWarpToNextMorning.timeOfDawn = sunnyTime;
            StartToolbarButton();
        }

        public void Update()
        {
            // if player has changed setting then change timeOfDawn to preset

            if (newMode != storedMode)
            {
                storedMode = newMode;
                switch (newMode)
                {
                    case 0:
                        KSP.UI.UIWarpToNextMorning.timeOfDawn = sunriseTime;
                        break;
                    case 1:
                        KSP.UI.UIWarpToNextMorning.timeOfDawn = sunnyTime;
                        break;
                    case 2:
                        KSP.UI.UIWarpToNextMorning.timeOfDawn = sunsetTime;
                        break;
                    case 3:
                        KSP.UI.UIWarpToNextMorning.timeOfDawn = midnightTime;
                        break;
                }
            }
            else return;

        }
    }
}
