using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace MyAntennaNeverGoesOffline
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class MANGO : MonoBehaviour
    {
        [KSPField]
        public List<Part> listOfAntennae;
       
        [KSPField]
        public List<Part> listOfGenerators;
       
        [KSPField]
        public int nBOfAntennae;
      
        [KSPField]
        public int nBOfGenerators;
      
        [KSPField]
        public bool processorPermitted;
      
        [KSPField]
        public float powerGen;
     
        [KSPField]
        public float newRate;
     
        [KSPField]
        public float mitAmount = 0F;

 
        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                try
                {
                    powerGen = 0.0F;
                    listOfAntennae = new List<Part>();
                    listOfGenerators = new List<Part>();

                    // add solar panels and antennae to lists

                    foreach (var parts in FlightGlobals.ActiveVessel.Parts)
                    {
                        if (parts.HasModuleImplementing<MangoAntenna>())
                        {
                            listOfAntennae.Add(parts);
                        }

                        if (parts.HasModuleImplementing<MangoSolar>())
                        {
                            listOfGenerators.Add(parts);
                        }
                    }

                    try
                    {
                        nBOfAntennae = listOfAntennae.Count();
                    }
                    catch
                    {
                        nBOfAntennae = 0;
                    }
                    try
                    {
                        nBOfGenerators = listOfGenerators.Count();
                    }
                    catch
                    {
                        nBOfGenerators = 0;
                    }

                    if (nBOfAntennae != 0 && nBOfGenerators != 0)                   // if vessel has both data transmitter and method
                    {                                                               // of generating power then activate the processor
                        processorPermitted = true;
                    }
                    else processorPermitted = false;

                    if (processorPermitted)
                    {
                        foreach (var part in listOfAntennae)
                        {
                            part.GetComponent<ModuleDataTransmitter>().packetResourceCost = 0.1F;       // buff antennae
                        }

                        foreach (var part in listOfGenerators)
                        {
                            if (part.HasModuleImplementing<ModuleDeployableSolarPanel>())
                            {
                                
                                float chargeR = part.GetComponent<ModuleDeployableSolarPanel>().chargeRate;
                                powerGen += chargeR;
                                part.GetComponent<ModuleDeployableSolarPanel>().chargeRate = (chargeR / 100) * 75;    // nerf solar panels to balance 

                                
                                

                                
                            }
                            else
                            {
                                continue; // is RTG; processor can't use low power generation by design (forces solar panel useage)
                            }
                        }

                        MangoUtility mU = new MangoUtility(powerGen);
                        newRate = mU.SetTime();

                        foreach (var antenna in listOfAntennae)
                        {
                            antenna.GetComponent<ModuleDataTransmitter>().packetInterval = newRate;        // change antennae rates using
                        }                                                                                   // new rate
                    }
                }
                catch
                { //internal error
                }

            }            
        }


    }
}
