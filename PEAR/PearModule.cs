using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProgramaticExtensionAndRetraction
{
    class PearModule : PartModule
    {
        // Extend all button

        [KSPEvent(isPersistent = false, guiActive = true, guiActiveEditor = false, active = false, guiName = "Extend All Extendables")]
        public void ExtendAll()
        {
            if (this.part.GetComponent<ModuleDeployablePart>().deployState == ModuleDeployablePart.DeployState.RETRACTED)
            {
                PEAR.ToggleExtendables(true);
            }
        }

        //retract all button

        [KSPEvent(isPersistent = false, guiActive = true, guiActiveEditor = false, active = false, guiName = "Retract All Extendables")]
        public void RetractAll()
        {
            if (this.part.GetComponent<ModuleDeployablePart>().deployState == ModuleDeployablePart.DeployState.EXTENDED)
            {
                PEAR.ToggleExtendables(false);
            }
        }

        
       






    }
}
