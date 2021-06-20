using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ParachutesLetsUseMaths
{
    public class PlumUtilities
    {
        // Dictionaries that hold parachute effect factors for each planet/moon

        private Dictionary<int, double> bValsK = new Dictionary<int, double>();
        private Dictionary<int, double> bValsE = new Dictionary<int, double>();
        private Dictionary<int, double> bValsD = new Dictionary<int, double>();
        private Dictionary<int, double> bValsL = new Dictionary<int, double>();

        // main
        public PlumUtilities()
        {
            CreateDictionaryOfBValuesS();

        }

        // adds values to dictionaries
        private void CreateDictionaryOfBValuesS()
        {

            const double k0 = 0.0332;
            const double k1 = 0.0371;
            const double k2 = 0.0230;
            const double k3 = 1.3700;
            const double k4 = 1.2786;

            const double e0 = 0.0106;
            const double e1 = 0.0114;
            const double e2 = 0.0070;
            const double e3 = 0.4841;
            const double e4 = 0.3859;

            const double d0 = 0.0936;
            const double d1 = 0.1182;
            const double d2 = 0.0659;
            const double d3 = 3.7549;
            const double d4 = 3.3189;

            const double l0 = 0.0384;
            const double l1 = 0.0531;
            const double l2 = 0.0279;
            const double l3 = 1.7339;
            const double l4 = 1.5006;

            bValsK.Add(0, k0);
            bValsK.Add(1, k1);
            bValsK.Add(2, k2);
            bValsK.Add(3, k3);
            bValsK.Add(4, k4);

            bValsE.Add(0, e0);
            bValsE.Add(1, e1);
            bValsE.Add(2, e2);
            bValsE.Add(3, e3);
            bValsE.Add(4, e4);

            bValsD.Add(0, d0);
            bValsD.Add(1, d1);
            bValsD.Add(2, d2);
            bValsD.Add(3, d3);
            bValsD.Add(4, d4);

            bValsL.Add(0, l0);
            bValsL.Add(1, l1);
            bValsL.Add(2, l2);
            bValsL.Add(3, l3);
            bValsL.Add(4, l4);

        }

        public double BCodeBase(int planet, int chute)
        {
            // returns the chute power factor according to supplied planet/chute type

            switch (planet)
            {
                case 0:
                    return bValsK[chute];
                case 1:
                    return bValsE[chute];
                case 2:
                    return bValsD[chute];
                case 3:
                    return bValsL[chute];
                default:
                    return bValsK[chute];
            }


        }

        public double GetSingleCustomB(int chute)
        {
            float tempHol;

            switch (chute)
            {
                case 0:
                    tempHol = GUIElements.chute0;
                    break;
                case 1:
                    tempHol = GUIElements.chute1;
                    break;
                case 2:
                    tempHol = GUIElements.chute2;
                    break;
                case 3:
                    tempHol = GUIElements.chute3;
                    break;
                case 4:
                    tempHol = GUIElements.chute4;
                    break;
                default:
                    tempHol = 1;
                    break;
            }

            double custGrav = GUIElements.customGravVal;
            double custAD = GUIElements.customAirDensity;

            // B = (2 * g) / ( p * Cd)

            double toReturn = Math.Round((2 * custGrav) / (custAD * tempHol), 4);

            return toReturn;

        }

        public string FetchATD(int selCode)
        {
            // returns the air density for the requested planet

            string bodyStr;

            switch (selCode)
            {
                case 0:
                    bodyStr = "1.22498";
                    break;
                case 1:
                    bodyStr = "4.20211";
                    break;
                case 2:
                    bodyStr = "0.10099";
                    break;
                case 3:
                    bodyStr = "0.76457";
                    break;
                case 4:
                    bodyStr = GUIElements.customAirDensity.ToString();
                    break;
                default:
                    bodyStr = "101.325";
                    break;
            }

            return bodyStr;

        }

        public string FetchSG(int selCode)
        {
            // returns the surface gravity for the supplied planet

            string bodyStr;

            switch (selCode)
            {
                case 0:
                    bodyStr = "9.81";
                    break;
                case 1:
                    bodyStr = "16.7";
                    break;
                case 2:
                    bodyStr = "2.94";
                    break;
                case 3:
                    bodyStr = "7.85";
                    break;
                case 4:
                    bodyStr = GUIElements.customGravVal.ToString();
                    break;
                default:
                    bodyStr = "9.81";
                    break;
            }

            return bodyStr;


        }

        public float FetchDragDefault(int index)
        {
            switch (index)
            {
                case 0:
                    return 482.427f;
                case 1:
                    return 431.714f;
                case 2:
                    return 696.373f;
                case 3:
                    return 11.691f;
                case 4:
                    return 12.527f;
                default:
                    return 552.296f;

            }


        }

        private bool InSymmetry(int _chute)
        {

            string chuteName;

            switch (_chute)
            {
                case 1:
                    chuteName = "parachuteRadial";
                    break;
                case 4:
                    chuteName = "radialDrogue";
                    break;
                default:
                    chuteName = "ERROR";
                    break;
            }

            foreach (var part in EditorLogic.fetch.ship.Parts)
            {

                if (part.name == chuteName)
                {
                    return part.symmetryCounterparts.Count == 0 ? false : true;
                }
                else continue;
            }

            return false;

        }

        public double GetMulti(int planet, int chute, int count)
        {
            // if there are multiple chutes, the formula is slightly different as there is a bonus for using radial chutes in symmetry.
            // this was originally calculated as a 1.5 bonus (as opposed to standard 1) however this doesn't work anymore/with enough accuracy.
            // As a compromise I've settled on 1.525 although this isn't 100% either. Unfortunately unless the offical details are released by
            // squad, it is near impossibe to calculate exactly.


            if (planet == 0)
            {
                if (chute == 0 || chute == 2 || chute == 3)
                {
                    return count / bValsK[chute];
                }
                else
                {
                    return !InSymmetry(chute) ? count / bValsK[chute] : Math.Pow(count, 1.525) / bValsK[chute];
                }
            }
            else if (planet == 1)
            {
                if (chute == 0 || chute == 2 || chute == 3)
                {
                    return count / bValsE[chute];
                }
                else
                {
                    if (!InSymmetry(chute))
                    {
                        return count / bValsE[chute];
                    }
                    else
                    {
                        return Math.Pow(count, 1.525) / bValsE[chute];
                    }
                }
            }
            else if (planet == 2)
            {
                if (chute == 0 || chute == 2 || chute == 3)
                {
                    return count / bValsD[chute];
                }
                else
                {
                    if (!InSymmetry(chute))
                    {
                        return count / bValsD[chute];
                    }
                    else
                    {
                        return Math.Pow(count, 1.525) / bValsD[chute];
                    }
                }
            }
            else if (planet == 3)
            {
                if (chute == 0 || chute == 2 || chute == 3)
                {
                    return count / bValsL[chute];
                }
                else
                {
                    if (!InSymmetry(chute))
                    {
                        return count / bValsL[chute];
                    }
                    else
                    {
                        return Math.Pow(count, 1.525) / bValsL[chute];
                    }
                }
            }

            else
            {
                if (chute == 0 || chute == 2 || chute == 3)
                {
                    return count / GetSingleCustomB(chute);
                }
                else
                {
                    if (!InSymmetry(chute))
                    {
                        return count / GetSingleCustomB(chute);
                    }
                    else
                    {
                        return Math.Pow(count, 1.525) / GetSingleCustomB(chute);
                    }
                }
            }


        }




    }
}
