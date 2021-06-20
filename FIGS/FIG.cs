using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FillingInventoryGenerously
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FIG : MonoBehaviour
    {
        [KSPField]
        public string rK;

        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                try
                {
                    rK = "evaRepairKit";                                            // assign repair kit to string

                    foreach (var part in FlightGlobals.ActiveVessel.Parts)          // check all parts for inventory AND command ability
                    {
                        if (part.HasModuleImplementing<ModuleInventoryPart>() && part.HasModuleImplementing<ModuleCommand>())
                        {
                            ModuleInventoryPart mPI = part.GetComponent<ModuleInventoryPart>();
                            int nbOfSlots = mPI.TotalEmptySlots();                                  // number of empty slots

                            if (nbOfSlots != 0) 
                            {
                                for (int x = 0; x < nbOfSlots; x++)
                                {
                                    if (mPI.IsSlotEmpty(x))
                                    {
                                        mPI.StoreCargoPartAtSlot(rK, x);                       // add repair kit to first empty slot
                                        mPI.UpdateStackAmountAtSlot(x, 4);                     // make them stack so 4 per slot
                                        
                                    }
                                }
                            }
                        }
                    }
                }
                catch { // non-flight scene error 
                }
            }

        }



    }
}
