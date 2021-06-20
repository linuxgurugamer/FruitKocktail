using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProgramaticExtensionAndRetraction
{
    [KSPAddon(KSPAddon.Startup.FlightAndEditor, false)]
    public class PEAR : MonoBehaviour
    {
        private List<string> blackList;
        private string filePath = KSPUtil.ApplicationRootPath + "/GameData/FruitKocktail/PEAR/PluginData/blacklist.txt";
   
        // event to handle switching vessels

        public void VesselSwitchEvent(Vessel vOld, Vessel vNew)
        {
            blackList = new List<String>(File.ReadAllLines(filePath));

            foreach (var part in vNew.Parts)
            {
                if (blackList.Contains(part.name))
                {
                    try
                    {
                        part.RemoveModule(part.GetComponent<PearPowerController>());
                        part.RemoveModule(part.GetComponent<PearModule>());
                    }
                    catch
                    {
                        return;
                    }
                }

                else if (part.HasModuleImplementing<PearPowerController>())
                {
                    PowerTogglePressed(part, 0);
                }
            }
        }

        // power button controller
        public static void PowerTogglePressed(Part _part, int sender)
        {
            PearPowerController pPC = _part.GetComponent<PearPowerController>();

            if (HighLogic.LoadedSceneIsEditor)
            {
                if (pPC.isPowerOn)
                {
                    pPC.isPowerOn = false;
                    pPC.pearStatus = "OFFLINE";
                }
                else
                {
                    pPC.isPowerOn = true;
                    pPC.pearStatus = "Active";
                }
            }

            else if (HighLogic.LoadedSceneIsFlight)
            {
                if (!pPC.isPowerOn && sender == 0)
                {
                    pPC.isPowerOn = false;
                    pPC.pearStatus = "OFFLINE";
                }
                else if (pPC.isPowerOn && sender == 0)
                {
                    pPC.isPowerOn = true;
                    pPC.pearStatus = "Active";

                }
                else if (pPC.isPowerOn && sender == 1)
                {
                    pPC.isPowerOn = false;
                    pPC.pearStatus = "OFFLINE";
                }
                else if (!pPC.isPowerOn && sender == 1)
                {
                    pPC.isPowerOn = true;
                    pPC.pearStatus = "Active";
                }
                               
                SetPearModule(_part, pPC);
            }
        }

        // controls the PearModule
        public static void SetPearModule(Part _part2, PearPowerController _pPC)
        {
            PearModule pM = _part2.GetComponent<PearModule>();
            ModuleDeployablePart mDP = _part2.GetComponent<ModuleDeployablePart>();
            PearPowerController pPC = _pPC;

            if (pPC.isPowerOn && mDP.deployState == ModuleDeployablePart.DeployState.RETRACTED || 
                mDP.deployState == ModuleDeployablePart.DeployState.RETRACTING)
            {
                pM.Events["ExtendAll"].active = true;
                pM.Events["RetractAll"].active = false;

            }

            else if (pPC.isPowerOn && mDP.deployState == ModuleDeployablePart.DeployState.EXTENDED ||
                mDP.deployState == ModuleDeployablePart.DeployState.EXTENDING)
            {
                pM.Events["ExtendAll"].active = false;
                pM.Events["RetractAll"].active = true;
            }

            else if (!pPC.isPowerOn || mDP.deployState == ModuleDeployablePart.DeployState.BROKEN)
            {
                pM.Events["ExtendAll"].active = false;
                pM.Events["RetractAll"].active = false;
            }
        }

        // handles extending/retracting
        public static void ToggleExtendables(bool toExtend)
        {
            ModuleDeployablePart mDP;
            PearPowerController pPC;

            if (toExtend)
            {

                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.HasModuleImplementing<PearModule>())
                    {
                        mDP = part.GetComponent<ModuleDeployablePart>();
                        pPC = part.GetComponent<PearPowerController>();

                        if (pPC.isPowerOn)
                        {
                            mDP.Extend();
                            SetPearModule(part, pPC);
                        }
                        else continue;

                    }
                    else continue;
                   
                }
            }

            else
            {
                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.HasModuleImplementing<PearModule>())
                    {
                        mDP = part.GetComponent<ModuleDeployablePart>();
                        pPC = part.GetComponent<PearPowerController>();

                        if (pPC.isPowerOn)
                        {
                            mDP.Retract();
                            SetPearModule(part, pPC);
                        }
                        else continue;

                    }
                    else continue;

                }
            }
        }

 

        public void Start()
        {
            blackList = new List<String>(File.ReadAllLines(filePath));

            GameEvents.onVesselSwitching.Add(VesselSwitchEvent);

            if (HighLogic.LoadedSceneIsFlight)
            {
                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (blackList.Contains(part.name))
                    {
                        try
                        {
                            part.RemoveModule(part.GetComponent<PearPowerController>());
                            part.RemoveModule(part.GetComponent<PearModule>());
                        }
                        catch
                        {
                            return;
                        }
                    }

                    else if (part.HasModuleImplementing<PearPowerController>())
                    {
                        PowerTogglePressed(part, 0);
                    }
                } 

            }
        }

        public void Update()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                try
                {
                    if (EditorLogic.fetch.ship.Parts != null)
                    {
                        foreach (var part in EditorLogic.fetch.ship.Parts)
                        {
                            if (blackList.Contains(part.name) && part.HasModuleImplementing<PearPowerController>())
                            {
                                part.RemoveModule(part.GetComponent<PearPowerController>());
                                part.RemoveModule(part.GetComponent<PearModule>());
                            }
                        }
                    }
                }
                catch { // no parts
                      }

            }
        }


        public void OnDestroy()
        {
            GameEvents.onVesselSwitching.Remove(VesselSwitchEvent);
        }
          
       
    }
}
