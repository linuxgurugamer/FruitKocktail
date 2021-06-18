using UnityEngine;
using ToolbarControl_NS;

namespace LieInMustEnsue
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(LIME.MODID, LIME.MODNAME);
        }
    }
}