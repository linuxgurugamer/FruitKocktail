using ModuleWheels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AutomatedPopularPreLaunchExperiment
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class AutomatedPopularPreLaunchExperiment : MonoBehaviour
    {
        // KSPFields set

        public VesselType currentVesselType;
        public AppleOptions appleOptions;
        public KerbalEVA.VisorStates visorStates;
        public SASManipulator sm;
        public bool sasDone = false;
        public bool sasSet;
        public bool dispDone = false;
        public bool dispSet;
        public bool brakesDone = false;
        public bool brakesSet;
        public bool shipLightsAreOn = false;
        public bool sLSet;
        public bool visorSet;
        public bool visorIsDown = false;
        public bool lightSet;
        public bool helmetLightOn = false;
        public bool remHelmetSet;
        public bool helmetRemoved = false;
        public bool autoSAS;
        public bool warpRateSet10;
        public bool gearSet250;
        public bool gearPermit = false;
        public bool startStatus = false;
        public bool armStatus = false;
        public bool gearIsDeployed = false;
        public bool smIsCalled = false;
        public bool activateSASNow = false;
        public float[] dataArr = new float[3];
        public float armHeight;
        public float depHeight;
        public float retHeight;
        public float currentHeight;
        public List<Part> listOfDeployables = new List<Part>();

        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                try
                {
                    appleOptions = HighLogic.CurrentGame.Parameters.CustomParams<AppleOptions>();
                    currentVesselType = FlightGlobals.ActiveVessel.vesselType;
                    sasSet = appleOptions.sasOn;
                    dispSet = appleOptions.manNodeModeOn;
                    brakesSet = appleOptions.brakesOn;
                    sLSet = appleOptions.shipLightsOn;
                    visorSet = appleOptions.visorOn;
                    lightSet = appleOptions.kerbalLightsOn;
                    autoSAS = appleOptions.autoSetSAS;
                    warpRateSet10 = appleOptions.warp10;
                    gearSet250 = appleOptions.gear250;
                    remHelmetSet = appleOptions.kerbalRemoveHelmet;

                    // set SAS button on at launch

                    if (sasSet && !sasDone)
                    {
                        if (currentVesselType == VesselType.Lander || currentVesselType == VesselType.Plane ||
                            currentVesselType == VesselType.Probe || currentVesselType == VesselType.Ship)
                        {
                            FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.SAS, true);
                        }

                        sasDone = true;
                    }

                    // change bottom/left panel to show Ap/Pe info

                    if (dispSet && !dispDone)
                    {
                        if (currentVesselType == VesselType.Lander || currentVesselType == VesselType.Plane ||
                            currentVesselType == VesselType.Probe || currentVesselType == VesselType.Ship)
                        {
                            FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_EDIT);
                        }

                        dispDone = true;
                    }

                    // set brakes for planes/rovers

                    if (brakesSet && !brakesDone)
                    {
                        if (currentVesselType == VesselType.Rover || currentVesselType == VesselType.Plane)
                        {
                            FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.Brakes, true);
                        }
                        brakesDone = true;
                    }

                    // confirm visor state at start

                    if (FlightGlobals.ActiveVessel.isEVA)
                    {
                        visorStates = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().VisorState;

                        if (visorStates.Equals(KerbalEVA.VisorStates.Raised) || visorStates.Equals(KerbalEVA.VisorStates.Raising))
                        {
                            visorIsDown = false;
                        }
                        else if (visorStates.Equals(KerbalEVA.VisorStates.Lowered) || visorStates.Equals(KerbalEVA.VisorStates.Lowering))
                        {
                            visorIsDown = true;
                        }
                    }

                    // set warp lead time

                    if (warpRateSet10)
                    {
                        GameSettings.WARP_TO_MANNODE_MARGIN = 10F;
                    }
                    else if (!warpRateSet10)
                    {
                        GameSettings.WARP_TO_MANNODE_MARGIN = 30F;
                    }


                    if (gearSet250)
                    {
                        foreach (var part in FlightGlobals.ActiveVessel.Parts)
                        {
                            if (part.HasModuleImplementing<ModuleWheelDeployment>())
                            {
                                listOfDeployables.Add(part);                                // redundant but kept for future use
                            }
                        }

                        try
                        {

                            if (listOfDeployables.Count > 0)
                            {
                                LandingGearProcessor lpg = new LandingGearProcessor(FlightGlobals.ActiveVessel,
                            FlightGlobals.ActiveVessel.vesselType, listOfDeployables, FlightGlobals.ActiveVessel.Landed);

                                gearPermit = lpg.ProcessOutput;
                                startStatus = lpg.StartStateOk;

                                if (gearPermit)
                                {
                                    dataArr = lpg.VesHeight();

                                    if (dataArr[0] != 0)
                                    {
                                        armHeight = dataArr[0];
                                        depHeight = dataArr[1];
                                        retHeight = dataArr[2];
                                        armStatus = false;
                                    }

                                    else gearPermit = false;  // internal error

                                }

                            }
                        }
                        catch
                        {
                            return;
                            // no deployables
                        }

                    }

                }
                catch
                {
                    // internal error
                }
            }

        }




        public void Update()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                try
                {                   // Vessel lights
                    if (sLSet)
                    {

                        if (!FlightGlobals.ActiveVessel.directSunlight && !shipLightsAreOn)
                        {
                            foreach (var part in FlightGlobals.ActiveVessel.Parts)
                            {
                                if (part.HasModuleImplementing<ModuleLight>())
                                {
                                    part.GetComponent<ModuleLight>().LightsOn();
                                }
                                else if (part.HasModuleImplementing<ModuleColorChanger>())
                                {
                                    part.SendEvent("Lights On");
                                }
                            }

                            FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.Light, true);
                            shipLightsAreOn = true;
                        }



                        // turn off lights if in sunlight unless player has done so already

                        else if (FlightGlobals.ActiveVessel.directSunlight && shipLightsAreOn)
                        {
                            foreach (var part in FlightGlobals.ActiveVessel.Parts)
                            {
                                if (part.HasModuleImplementing<ModuleLight>())
                                {
                                    part.GetComponent<ModuleLight>().LightsOff();
                                }
                                else if (part.HasModuleImplementing<ModuleColorChanger>())
                                {
                                    part.SendEvent("Lights Off");
                                }
                            }
                            FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.Light, false);
                            shipLightsAreOn = false;
                        }
                    }

                    // visor control

                    if (visorSet)
                    {
                        if (FlightGlobals.ActiveVessel.isEVA && FlightGlobals.ActiveVessel.directSunlight && !visorIsDown)
                        {
                            try
                            {
                                FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().LowerVisor();
                                visorIsDown = true;
                            }
                            catch
                            {
                                // no helmet
                            }
                        }

                        else if (FlightGlobals.ActiveVessel.isEVA && !FlightGlobals.ActiveVessel.directSunlight && visorIsDown)
                        {
                            try
                            {
                                FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().RaiseVisor();
                                visorIsDown = false;
                            }

                            catch
                            { // no helmet 
                            }
                        }
                    }

                    // helmet light

                    if (lightSet)
                    {
                        if (FlightGlobals.ActiveVessel.isEVA && FlightGlobals.ActiveVessel.directSunlight && helmetLightOn)
                        {
                            try
                            {
                                FlightGlobals.ActiveVessel.evaController.headLamp.SetActive(false);
                                helmetLightOn = false;
                            }
                            catch
                            {
                                // no helmet
                            }
                        }

                        else if (FlightGlobals.ActiveVessel.isEVA && !FlightGlobals.ActiveVessel.directSunlight && !helmetLightOn)
                        {
                            try
                            {
                                FlightGlobals.ActiveVessel.evaController.headLamp.SetActive(true);
                                helmetLightOn = true;
                            }
                            catch
                            {
                                // no helmet
                            }
                        }
                    }

                    // set helmet status

                    if (remHelmetSet)
                    {
                        try
                        {
                            if (FlightGlobals.ActiveVessel.isEVA && !helmetRemoved)
                            {
                                if (FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().CanSafelyRemoveHelmet())
                                {
                                    FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().ToggleHelmetAndNeckRing(false, false);
                                    helmetRemoved = true;
                                }


                            }

                            else if (FlightGlobals.ActiveVessel.isEVA && helmetRemoved)
                            {

                                if (!FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().CanSafelyRemoveHelmet())
                                {
                                    FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>().ToggleHelmetAndNeckRing(true, true);
                                    helmetRemoved = false;
                                }

                            }



                        }
                        catch
                        {

                        }
                    }


                    // auto manueuver node selector on SAS

                    if (autoSAS)
                    {
                        try
                        {
                            if (FlightGlobals.ActiveVessel.Autopilot.CanSetMode(VesselAutopilot.AutopilotMode.Maneuver))
                            {
                                // if sm Class unassigned then assign

                                if (!smIsCalled)
                                {
                                    sm = new SASManipulator();
                                    smIsCalled = true;
                                }

                                // want to call this same frame hence "if". Checks for change in maneuver node dV

                                if (smIsCalled)
                                {
                                    activateSASNow = sm.ClearedToProceed();
                                }

                                // if change detected, move to maneuver node on SAS, but disable lock allowing player to select another mode if they need to

                                if (activateSASNow)
                                {
                                    FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.SAS, true);
                                    FlightGlobals.ActiveVessel.Autopilot.SetMode(VesselAutopilot.AutopilotMode.Maneuver);
                                    smIsCalled = false;
                                    activateSASNow = false;
                                }
                                else return;

                            }
                            else
                            {
                                sm = null;
                                smIsCalled = false;
                                activateSASNow = false;
                            }
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
                catch
                { // internal error
                }
            }
        }


        public void FixedUpdate()
        {
            // Auto gear deployment : Raycast = physics = fixedupdate

            if (HighLogic.LoadedSceneIsFlight && gearSet250 && gearPermit)
            {
                currentHeight = FlightGlobals.ActiveVessel.heightFromTerrain;

                if (!armStatus)                                 // requires min height arming to prevent action on startup
                {
                    if (currentHeight >= armHeight)
                    {
                        armStatus = true;
                    }
                    else armStatus = false;
                }

                // only called if armStatus is true...

                else
                {

                    if (currentHeight <= depHeight && !gearIsDeployed)
                    {
                        FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.Gear, true);
                        gearIsDeployed = true;
                    }

                    else if (currentHeight <= depHeight && gearIsDeployed)
                    {
                        return;
                    }

                    else if (currentHeight >= retHeight && gearIsDeployed)
                    {
                        FlightGlobals.ActiveVessel.ActionGroups.SetGroup(KSPActionGroup.Gear, false);
                        gearIsDeployed = false;
                        armStatus = false;
                    }
                    else if (currentHeight >= retHeight && !gearIsDeployed)
                    {
                        return;
                    }

                }

            }

        }

    }
}

