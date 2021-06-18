using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CastAway
{
    public class CaRaft : PartModule
    {
        // Button to toggle anchor (and sailing ability)

        [KSPEvent(isPersistent = false, active = true, guiActive = true, guiActiveEditor = false, guiName = "Raise Anchor")]
        public void AnchorToggle()
        {
            if (anchorIsDown)
            {
                anchorIsDown = false;
                Events["AnchorToggle"].guiName = "Lower Anchor";
                
            }
            else
            {
                anchorIsDown = true;
                Events["AnchorToggle"].guiName = "Raise Anchor";
                
            }

        }

        // Current windspeed, visible in PAW

        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Wind Speed")]
        public float windSp;

        // fixed windMultiplier, possible to customise value depending on situation
        [KSPField]
        public float windMultiplier;

        // is the anchor down
        public bool anchorIsDown;

        // reference for boat position for applying wind force
        public Vector3 boatPos;

        // boat's rigidbody
        public Rigidbody rB;

        // key to turn left (a)
        public KeyBinding leftKey;

        // key to turn right (d)
        public KeyBinding rightKey;

        // key to slow down (s)
        public KeyBinding sKey;

        // class to handle wind speed generation
        public WindHandler windHandler;

        // is the wind active
        private bool windIsActive;

        // the windspeed
        private float windSpeed;


        // the vessel needs the raft, sail and wilson parts in order to work. This checks they exist.
        public bool LookForSail()
        {
            int logicTest = 0;

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.HasModuleImplementing<CaSail>())
                {
                    logicTest += 1;
                    break;
                }
            }

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.HasModuleImplementing<CaWilson>())
                {
                    logicTest += 1;
                }
            }

            if (logicTest == 2)
            {
                return true;
            }
            else return false;
        }

        public void SetWindFields()
        {
            // local wind control

            windSp = windSpeed;
            windIsActive = true;
        }

       

        public void SetBuoyancy()
        {
            // stops sinking when adding parts such as command chair

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                part.buoyancy = 5f;
            }
        }

        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                anchorIsDown = true;
                windIsActive = false;
                bool g2g = LookForSail();

                if (!g2g)
                {
                    Events["AnchorToggle"].active = false;
                }
                else
                {
                    Events["AnchorToggle"].active = true;
                    windMultiplier = 20;
                    windHandler = new WindHandler();                    
                    windSpeed = windHandler.GetWindSp();      
                    SetWindFields();
                    leftKey = GameSettings.YAW_LEFT;
                    rightKey = GameSettings.YAW_RIGHT;
                    sKey = GameSettings.PITCH_UP;
                    SetBuoyancy();
                }

            }


        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!anchorIsDown && windIsActive && FlightGlobals.ActiveVessel.Splashed)
                {
                    boatPos = FlightGlobals.ActiveVessel.transform.forward.normalized;  // actually backwards for this model!
                   
                    rB = FlightGlobals.ActiveVessel.GetComponent<Rigidbody>();      // assign RB
                    rB.AddForce(-boatPos * windMultiplier);                         // add wind force

                    if (leftKey.GetKey(false))  // left key pressed
                    {
                        FlightGlobals.ActiveVessel.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 100);
                    }
                    
                    if (rightKey.GetKey(false))   // right key pressed
                    {
                        FlightGlobals.ActiveVessel.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 100);
                    }

                    if (sKey.GetKey(false))     // s key pressed (acts like a brake)
                    {
                        if (rB.velocity != Vector3.zero)
                        {
                            rB.AddForce(boatPos * windMultiplier);
                        }
                    }
 

                }

                else if (anchorIsDown && FlightGlobals.ActiveVessel.checkSplashed())
                {
                    rB = FlightGlobals.ActiveVessel.GetComponent<Rigidbody>();  // if anchor down cancel wind force
                    rB.velocity = Vector3.zero;
                }

            }

        }








    }
}
