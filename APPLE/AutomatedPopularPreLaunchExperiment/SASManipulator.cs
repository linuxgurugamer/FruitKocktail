using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AutomatedPopularPreLaunchExperiment
{
    public class SASManipulator
    {
        private Vector3d storedNode;
        private Vector3d node;

        public SASManipulator()
        {
            storedNode = LoadNode();
        }


        private Vector3d LoadNode()
        {
            Vector3d toReturn = new Vector3d(0, 0, 0);

            try
            {
                if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0)
                {
                    toReturn = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[0].DeltaV;   
                }

                return toReturn;
                
            }
            catch
            {
                return toReturn;
            }

        }

        public bool ClearedToProceed()
        {
            // if node premade (ie MechJeb or KAC restore node)

            if (storedNode.x != 0 || storedNode.y != 0 || storedNode.z != 0)
            {
                return true;
            }
            else
            {
                // wait till player adjusts handles before trying to execute autoSAS

                node = LoadNode();

                if (node != storedNode)
                {
                    return true;
                }
                else return false;
            }
        }

    }
}
