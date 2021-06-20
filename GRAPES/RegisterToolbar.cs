﻿
using UnityEngine;
using ToolbarControl_NS;

namespace GasRepairsAndProbablyExpensiveSnacks
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(GUIComponents.MODID, GUIComponents.MODNAME);
        }
    }
}