using UnityEngine;
using ToolbarControl_NS;

namespace ParachutesLetsUseMaths
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(GUIElements.MODID, GUIElements.MODNAME);
        }
    }
}