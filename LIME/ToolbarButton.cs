using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

using ToolbarControl_NS;
using ClickThroughFix;

namespace LieInMustEnsue
{
    public partial class LIME
    {
        // the toolbar button
        static ToolbarControl toolbarControl;

        // is button pressed?
        public bool btnIsPressed = false;

        // menu selection id
        public static int selGridInt = 1;

        // menu options
        public static string[] selString = new string[] { "Sunrise (Stock)", "Sunny", "Sunset", "Midnight" };

        // close button on menu
        public static bool closeBtn;

        // menu position reference, set for middle of the screen
        private Vector2 menuPR = new Vector2((Screen.width / 2) - 100, (Screen.height / 2) - 93);

        // menu size reference
        private Vector2 menuSR = new Vector2(200, 250);

        // the menu position holder
        private static Rect menuPos;


        internal const string MODID = "LIME";
        internal const string MODNAME = "Lie In Must Ensue";




        public void StartToolbarButton()
        {
            //  preload menu position


            menuPos = new Rect(menuPR, menuSR);



            if (toolbarControl == null)
            {
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(onTrue, onFalse, onHover, onHoverOut, null, null,
                    ApplicationLauncher.AppScenes.SPACECENTER,
                    MODID,
                    "GRAPESButton",
                    "FruitKocktail/LIME/PluginData/Icons/limeon-38",
                    "FruitKocktail/LIME/PluginData/Icons/limeon-24",
                    "FruitKocktail/LIME/PluginData/Icons/limeoff-38",
                    "FruitKocktail/LIME/PluginData/Icons/limeoff-24",
                    MODNAME
                );

            }

            toolbarControl.SetTexture("FruitKocktail/LIME/PluginData/Icons/limeoff-38",
                 "FruitKocktail/LIME/PluginData/Icons/limeoff-24");
        }

        private static void ItsLimeTime()
        {

            // instantiate the menu

            menuPos = ClickThruBlocker.GUILayoutWindow(123456, menuPos, MenuWindow,
                "LIME Time Options", new GUIStyle(HighLogic.Skin.window));

        }

        private static void MenuWindow(int windowID)
        {
            // menu defs

            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();

            selGridInt = GUI.SelectionGrid(new Rect(20, 50, 200, 186), selGridInt, selString, 1, new GUIStyle(HighLogic.Skin.toggle));

            GUILayout.EndHorizontal();
            GUILayout.Space(25);

            GUILayout.BeginHorizontal();

            if ( GUI.Button(new Rect(20, 200, 160, 25), "Close", new GUIStyle(HighLogic.Skin.button)))
            {
                    LIME.newMode = selGridInt;
                    //limeBtn.SetFalse();
                    toolbarControl.SetFalse();
                    closeBtn = false;

            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow();

        }


        public void OnGUI()
        {
            // handles GUI event (ie button clicked)

            if (btnIsPressed)
            {
                ItsLimeTime();
            }
        }

        // button callbacks

        public void onTrue()
        {
            // ie when clicked on
            toolbarControl.SetTexture("FruitKocktail/LIME/PluginData/Icons/limeon-38",
                    "FruitKocktail/LIME/PluginData/Icons/limeon-24");
            btnIsPressed = true;

        }

        public void onFalse()
        {
            // ie when clicked off
            toolbarControl.SetTexture("FruitKocktail/LIME/PluginData/Icons/limeoff-38",
                    "FruitKocktail/LIME/PluginData/Icons/limeoff-24");
            btnIsPressed = false;
        }

        public void onHover()
        {
            // ie on hover when not currently on

            if (!btnIsPressed)
            {
                toolbarControl.SetTexture("FruitKocktail/LIME/PluginData/Icons/limeoff-38",
                        "FruitKocktail/LIME/PluginData/Icons/limeoff-24");
            }
        }

        public void onHoverOut()
        {
            // ie when leave button when not currently on

            if (!btnIsPressed)
            {
                toolbarControl.SetTexture("FruitKocktail/LIME/PluginData/Icons/limeoff-38",
        "FruitKocktail/LIME/PluginData/Icons/limeoff-24");

            }
        }
    }
}
