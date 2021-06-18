using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AutomatedPopularPreLaunchExperiment
{
    public class LandingGearProcessor
    {

        private Vessel activeVes;
        private VesselType vesType;
        private List<Part> depList;
        private bool landed;
        private bool vesCode;


        public LandingGearProcessor(Vessel _activeVes, VesselType _vesType, List<Part> _depList, bool _landed)
        {
            activeVes = _activeVes;
            vesType = _vesType;
            depList = _depList;
            landed = _landed;
        }

        // check not something that shouldn't deploy
        public bool ProcessOutput()
        {
            if (vesType != VesselType.Debris || vesType != VesselType.Unknown)
            {
                vesCode = true;
            }
            else vesCode = false;

            return vesCode;

        }

        // checks if landed and disarms if so
        public bool StartStateOk()
        {

            switch (landed)
            {
                case true:
                    return true;
                    break;
                case false:
                    return false;
                    break;
                default:
                    return false;
                    break;
            }   

        }

        // get vessel type parameters
        public float[] VesHeight()
        {
            float[] toReturn;

            if (vesType == VesselType.Plane)
            {
                toReturn = new float[3] { 2000.0F, 1000.0F, 200.0F };
            }
            else
            {
                toReturn = new float[3] { 1500.0F, 1000.0F, 50.0F };
            }
            
            return toReturn;

        }

       


    }
}
