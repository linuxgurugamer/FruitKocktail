using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgramaticExtensionAndRetraction
{
    public class PearPowerController : PartModule
    {
        // power toggle button

        [KSPEvent(active = true, guiActive = true, guiActiveEditor = true, isPersistent = false, guiName = "Toggle PEAR")]
        public void TogglePear()
        {
            PEAR.PowerTogglePressed(this.part, 1); 
        }

        [KSPField(guiActive = true, guiActiveEditor = true, guiName = "PEAR Status", isPersistant = false)]
        public string pearStatus = "Active";

        [KSPField(isPersistant = true)]
        public bool isPowerOn = true;


        

      




    }
}
