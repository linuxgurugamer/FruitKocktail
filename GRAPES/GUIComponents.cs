using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

using ToolbarControl_NS;
using ClickThroughFix;

namespace GasRepairsAndProbablyExpensiveSnacks
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class GUIComponents : MonoBehaviour
    {
        // are we awaiting a delivery?
        [KSPField(isPersistant = true)]
        public bool isAwaitingDelivery = false;

        // when will the delivery happen?
        [KSPField(isPersistant = true)]
        public double timerEnd;

        // the button reference
        static ToolbarControl toolbarControl;

        // is button pressed?
        public bool btnIsPressed = false;

        // this
        public static GUIComponents instance;

        // status text holder
        public static string statusStringToReturn = "";

        // code for action type
        public static int code;

        // are there tanks with space in them?
        private static bool canRefuel;

        // are there batteries with spare capacity?
        private static bool canRecharge;

        // close button on menu
        public static bool closeBtn;

        // refuel request button on menu
        public static bool refuelBtn;

        //recharge request button on menu
        public static bool rechargeBtn;

        // repair request button (not currently implemented)
        public static bool repairBtn;

        // are we at a fuel station?
        public static bool atStation;

        // menu position reference ie in the middle of the screen
        private Vector2 menuPR = new Vector2((Screen.width / 2) - 200, (Screen.height / 2) - 237);

        // menu size reference
        private Vector2 menuSR = new Vector2(400, 474);

        // the menu position holder
        private static Rect guiPos;

        // current prices for location
        private static List<double> rates;


        internal const string MODID = "GRAPE";
        internal const string MODNAME = "Gas Repairs & Probably Expensive Snacks";

        public void Start()
        {

            // get the icons from file, preload menu position, get prices, instantiate the toolbar button & set status'

            if (HighLogic.LoadedSceneIsFlight)
            {
                instance = this;


                //grapesTextureOff = GameDatabase.Instance.GetTexture("FruitKocktail/GRAPES/Icons/grapesoff", false);
                //grapesTextureOn = GameDatabase.Instance.GetTexture("FruitKocktail/GRAPES/Icons/grapeson", false);
                guiPos = new Rect(menuPR, menuSR);
                rates = GasStation.ProvidePrices();

                if (toolbarControl == null)
                {
                    toolbarControl = gameObject.AddComponent<ToolbarControl>();
                    toolbarControl.AddToAllToolbars(onTrue, onFalse,
                        ApplicationLauncher.AppScenes.FLIGHT,
                        MODID,
                        "GRAPESButton",
                        "FruitKocktail/GRAPES/PluginData/Icons/grapeson-38",
                        "FruitKocktail/GRAPES/PluginData/Icons/grapeson-24",
                        MODNAME
                    );

                    //toolbarControl.SetTrue(btnIsPressed);
                    toolbarControl.SetTexture("FruitKocktail/GRAPES/PluginData/Icons/grapesoff-38",
                        "FruitKocktail/GRAPES/PluginData/Icons/grapesoff-24");
                }
                if (!isAwaitingDelivery)
                {
                    statusStringToReturn = "Awaiting Your Order";
                }
                GameEvents.onHideUI.Add(OnHideUI);
                GameEvents.onShowUI.Add(OnShowUI);
                GameEvents.onGUILock.Add(OnHideUI);
                GameEvents.onGUIUnlock.Add(OnShowUI);
                GameEvents.onGamePause.Add(OnHideUI);
                GameEvents.onGameUnpause.Add(OnShowUI);

            }
        }

        private static void CheckAtStation()
        {
            atStation = false;

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.HasModuleImplementing<GasStation>())
                {
                    atStation = true;
                    break;
                }
            }
        }
        private static void ItsGrapesTime()
        {
            CheckAtStation();

            // instantiate the menu if we're at a refueling station

            if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.ORBITING)
            {
                if (atStation)
                {
                    // instantiate the menu
                    guiPos = ClickThruBlocker.GUILayoutWindow(123456, guiPos, MenuWindow,
                        "Current Prices", new GUIStyle(HighLogic.Skin.window));
                    toolbarControl.SetTrue();

                }
            }
            else
            {
                ScreenMessage screenMessage = new ScreenMessage("You are not in orbit!", 3F, ScreenMessageStyle.UPPER_CENTER);
                ScreenMessages.PostScreenMessage(screenMessage);
                toolbarControl.SetFalse();
            }
        }

        private static void MenuWindow(int windowID)
        {
            // the menu

            GUILayout.BeginVertical();
            GUILayout.Space(20);

            GUILayout.BeginArea(new Rect(20, 40, 360, 220));
            GUILayout.BeginHorizontal();

            GUILayout.Space(20);
            GUILayout.Label("Liquid Fuel/Oxidiser = " + rates[0].ToString("0.00"), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Liquid Fuel = " + rates[1].ToString("0.00"), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Oxidiser = " + rates[2].ToString("0.00"), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("MonoPropellant = " + rates[3].ToString("0.00"), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Xenon = " + rates[4].ToString("0.00"), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Recharge Service = " + GetRechargeAbility(), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Repair Service = " + GetRepairAbility(), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(40);

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(20, 250, 360, 100));

            GUILayout.BeginHorizontal();

            GUILayout.Space(20);
            GUILayout.Label("Cost To Fill Up = " + GetFillUpCost(), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Your Available Credit = " + GetCredit(), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Station Status = " + LabelStatus(), new GUIStyle(HighLogic.Skin.label));
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginHorizontal();
            refuelBtn = GUI.Button(new Rect(40, 350, 320, 25), "Request Fuel", new GUIStyle(HighLogic.Skin.button));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            rechargeBtn = GUI.Button(new Rect(40, 375, 320, 25), "Request Recharge", new GUIStyle(HighLogic.Skin.button));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            repairBtn = GUI.Button(new Rect(40, 400, 320, 25), "Request Repair", new GUIStyle(HighLogic.Skin.button));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            closeBtn = GUI.Button(new Rect(40, 425, 320, 25), "Cancel/Close", new GUIStyle(HighLogic.Skin.button));
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();

            GUI.DragWindow();

        }

        public void Update()
        {

            if (HighLogic.LoadedSceneIsFlight)
            {
                if (toolbarControl != null)
                {
                    // menu button handlers

                    if (closeBtn)
                    {
                        toolbarControl.SetFalse();
                        closeBtn = false;
                    }

                    if (refuelBtn && !isAwaitingDelivery)
                    {
                        TryRefuel();
                        refuelBtn = false;
                    }

                    if (rechargeBtn && !isAwaitingDelivery)
                    {
                        TryRecharge();
                        rechargeBtn = false;
                    }
                }
            }
        }

        bool hide = false;
        void OnHideUI() { hide = true; }
        void OnShowUI() { hide = false; }

        public void OnGUI()
        {
            // handles GUI event (ie button clicked)

            if (btnIsPressed && !hide)
            {
                ItsGrapesTime();
            }
        }

        // button callbacks

        public void onTrue()
        {
            CheckAtStation();
            // ie when clicked on

            btnIsPressed = true;

            if (atStation)
            {
                toolbarControl.SetTexture("FruitKocktail/GRAPES/PluginData/Icons/grapeson-38",
                    "FruitKocktail/GRAPES/PluginData/Icons/grapeson-24");
            }
        }

        public void onFalse()
        {
            // ie when clicked off
                toolbarControl.SetTexture("FruitKocktail/GRAPES/PluginData/Icons/grapesoff-38",
                    "FruitKocktail/GRAPES/PluginData/Icons/grapesoff-24");
                btnIsPressed = false;
        }


        private void OnDestroy()
        {
            // when destroyed

            GameEvents.onShowUI.Remove(OnShowUI);
            GameEvents.onHideUI.Remove(OnHideUI);
            GameEvents.onGUILock.Remove(OnHideUI);
            GameEvents.onGUIUnlock.Remove(OnShowUI);
            GameEvents.onGamePause.Remove(OnHideUI);
            GameEvents.onGameUnpause.Remove(OnShowUI);

        }

        // gets cost to fill up in current context
        private static string GetFillUpCost()
        {
            double grabbedPrice = GasStation.GetFuelAmount();

            if (grabbedPrice == 0)
            {
                canRefuel = false;
                return "All Tanks Full!";
            }

            else
            {
                canRefuel = true;
                string stringToReturn = grabbedPrice.ToString("0.00");
                return stringToReturn;
            }

        }

        // finds how much credit the player has available on their cards
        private static string GetCredit()
        {

            double creditAmount = GasStation.GetCreditAmount();

            if (creditAmount == 0)
            {
                canRecharge = false;
                canRefuel = false;
                return "All Cards Are Empty!";
            }

            else
            {
                canRecharge = true;
                canRefuel = true;
                string stringToReturn = creditAmount.ToString("0.00");
                return stringToReturn;
            }

        }

        // method to populate menu status field
        private static string LabelStatus()
        {
            if (!instance.isAwaitingDelivery)
            {
                return statusStringToReturn;
            }
            else
            {
                return GetTimeRemaining();
            }
        }

        // if waiting for delivery, sets status according to time remaining
        private static string GetTimeRemaining()
        {
            double currentTime = Planetarium.fetch.time;
            double timeRem = Math.Round((instance.timerEnd - currentTime), 0);
            double daysRem = Math.Round(timeRem / 21600, 0);

            if (daysRem == 0)
            {
                instance.isAwaitingDelivery = false;
                statusStringToReturn = "Awaiting Your Order";
                return statusStringToReturn;
            }

            else if (daysRem == 1)
            {
                return "Delivery Due Tomorrow";
            }

            else
            {
                return "Next Delivery In " + daysRem + " Days";
            }

        }


        // gets cost to recharge or n/a if no capacity
        private static string GetRechargeAbility()
        {
            canRecharge = GasStation.QueryRecharge();

            if (canRecharge)
            {
                return rates[5].ToString("0.00");
            }

            else
            {
                return "N/A";
            }

        }

        // not currently implemented
        private static string GetRepairAbility()
        {

            return "N/A";

        }

        // initiates recharge and sends result to status
        private static void TryRecharge()
        {
            if (canRecharge)
            {
                code = GasStation.Recharge();

                if (code == 4)
                {
                    statusStringToReturn = "Recharge complete!, come again soon!";
                    LabelStatus();
                }

            }

        }

        // initiates refuel and sends result to status
        private static void TryRefuel()
        {
            if (canRefuel)
            {
                code = GasStation.Refuel();

                if (code == 1)
                {
                    statusStringToReturn = "All tanks now full, come again soon!";
                }
                else if (code == 2)
                {
                    statusStringToReturn = "Part refill complete, come again soon!";
                }

                LabelStatus();
                StaticCoroutine.Start(Wait(5));

            }
        }

        // Coroutine to show delayed status regarding delivery as result of purchasing fuel (i.e. so player sees refuel complete message first)

        public static IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            instance.timerEnd = GasStation.TimerEnd();
            instance.isAwaitingDelivery = true;
            LabelStatus();
        }


    }
}
