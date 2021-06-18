using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GasRepairsAndProbablyExpensiveSnacks
{
    public class FuelCard : PartModule
    {

        // available credit

        [KSPField(guiActive = true, guiActiveEditor = true, isPersistant = true, guiName = "Available Credit", guiUnits = "k$"
            , guiFormat = "F2")]
        public double availableCredit;


    }
}