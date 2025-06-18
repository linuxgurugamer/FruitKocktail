using KSP.UI.Screens.Flight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace VSIndicator
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class VSI : MonoBehaviour
    {
        // quick reference and stored colours

        [KSPField(isPersistant = true)]
        public Color32 savedA = new Color32(0, 225, 0, 255);

        [KSPField(isPersistant = true)]
        public Color32 savedD = new Color32(255, 0, 0, 255);

        [KSPField(isPersistant = true)]
        public Color32 savedS = new Color32(0, 225, 0, 225);

        public Color32 stockGreen = new Color32(0, 255, 0, 255);

        // The speed display component on the navball
        public SpeedDisplay sD;

        // the TM text for speed
        public TextMeshProUGUI tM;

        // the TM text for velocity mode
        public TextMeshProUGUI tM2;

        // bool to switch colour
        public bool colourSet = false;

        // bool to switch safe colour
        public bool safeColourSet = false;

        // pause menu options reference
        public VSIOptions vSIOptions;

        // indication of option selection
        public static bool shouldHideButton;

        // this
        public static VSI Instance;



        // allows subclasses to check navball setting
        public static bool GetTM2Text()
        {
            if (Instance.tM2.text == "Surface")
            {
                return true;
            }
            else return false;
        }


        // gets the code from the colour
        public static int GetColourCodeReversedA()
        {
            ColourDecoder cD = new ColourDecoder();
            Color32 tempCol = cD.GetColour(Instance.vSIOptions.ascCol);
            return cD.GetReversedColour(tempCol.ToString());
        }
        public static int GetColourCodeReversedD()
        {
            ColourDecoder cD = new ColourDecoder();
            Color32 tempCol = cD.GetColour(Instance.vSIOptions.desCol);
            return cD.GetReversedColour(tempCol.ToString());
        }

        public static int GetColourCodeReveredS()
        {
            ColourDecoder cD = new ColourDecoder();
            Color32 tempCol = cD.GetColour(Instance.vSIOptions.safCol);
            return cD.GetReversedColour(tempCol.ToString());
        }

        // stores the selected colours
        public static void TestSwatch(int colourCode, int type)
        {
            Color32 swatch;
            string codeName;
            ColourDecoder cD = new ColourDecoder();
            codeName = cD.DecipherCode(colourCode);
            swatch = cD.GetColour(codeName);

            if (type == 0)
            {
                Instance.savedA = swatch;
                var tempCol = cD.GetReversedColour(Instance.savedA.ToString());
                Instance.vSIOptions.ascCol = cD.DecipherCode(tempCol);
                
            }
            else if (type == 1)
            {
                Instance.savedD = swatch;
                var tempCol = cD.GetReversedColour(Instance.savedD.ToString());
                Instance.vSIOptions.desCol = cD.DecipherCode(tempCol);
                
            }
            else if (type == 2)
            {
                Instance.savedS = swatch;
                var tempCol = cD.GetReversedColour(Instance.savedS.ToString());
                Instance.vSIOptions.safCol = cD.DecipherCode(tempCol);
            }

        }





        public void Start()
        {
                Instance = this;
                sD = KSP.UI.Screens.Flight.SpeedDisplay.Instance;
                tM = sD.textSpeed;
                tM2 = sD.textTitle;

                vSIOptions = HighLogic.CurrentGame.Parameters.CustomParams<VSIOptions>();
                shouldHideButton = vSIOptions.disableButton;
                ColourDecoder cD = new ColourDecoder();

                try
                {
                    if (vSIOptions.ascCol == null)
                    {
                        savedA = cD.GetColour("Green");
                        vSIOptions.ascCol = "Green";
                    }
                    else
                    {
                        savedA = cD.GetColour(vSIOptions.ascCol);
                    }


                    if (vSIOptions.desCol == null)
                    {
                        savedD = cD.GetColour("Red");
                        vSIOptions.desCol = "Red";
                    }

                    else
                    {
                        savedD = cD.GetColour(vSIOptions.desCol);
                    }

                    if (vSIOptions.safCol == null)
                    {
                        savedS = cD.GetColour("Green");
                        vSIOptions.safCol = "Green";
                    }
                }

                catch
                {
                    Debug.LogError("Vertikal Speed Indicator: Error with persistent colour application");
                }
                


        }

        public void Update()
        {
            // if not surface mode then set to stock green

            if (tM2.text != "Surface")
            {
                if (tM.color != stockGreen)
                {
                    tM.color = stockGreen;
                    tM2.color = stockGreen;
                    tM.ForceMeshUpdate();
                    tM2.ForceMeshUpdate();
                }

            }
            else
            {
                // ascending colour handler

                if (!colourSet)
                {
                    if (tM.color != savedA)
                    {
                        tM.color = savedA;
                        tM2.color = savedA;
                        tM.ForceMeshUpdate();
                        tM2.ForceMeshUpdate();
                        

                    }

                }
                else if (colourSet)
                {
                    //descending colour handler

                    // safe speed
                    if (!safeColourSet)
                    {

                        if (tM.color != savedD)
                        {
                            tM.color = savedD;
                            tM2.color = savedD;
                            tM.ForceMeshUpdate();
                            tM2.ForceMeshUpdate();

                        }
                        else return;
                    }

                    // exceeded safe speed
                    else
                    {
                        if (tM.color != savedS)
                        {
                            tM.color = savedS;
                            tM2.color = savedS;
                            tM.ForceMeshUpdate();
                            tM2.ForceMeshUpdate();
                        }
                        else return;
                    }

                }
            }
        }

        public void FixedUpdate()
        {
            // if we're not landed and navball is in surface mode

            if (!FlightGlobals.ActiveVessel.Landed && tM2.text == "Surface")
            {
                double verticalSpeed = FlightGlobals.ActiveVessel.verticalSpeed;
                double safeSpeed = double.Parse(VSIGUI.selV.ToString()) * -1;

                if (verticalSpeed < 0)              // if negative (ie falling)
                {
                    colourSet = true;

                    safeColourSet = verticalSpeed >= safeSpeed ? true : false;

                }

                else
                {
                    colourSet = false;
                }

            }

            // if we land set back to green

            else if ( FlightGlobals.ActiveVessel.Landed && tM2.text == "Surface")
            {
                colourSet = false;
            }


        }

    }
}
