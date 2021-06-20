﻿using UnityEngine;
using ToolbarControl_NS;

namespace VSIndicator
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(VSIGUI.MODID, VSIGUI.MODNAME);
        }
    }
}