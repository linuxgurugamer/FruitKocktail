using KSP.Localization;
using Expansions.Missions.Adjusters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GasRepairsAndProbablyExpensiveSnacks
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class GasStation : PartModule
    {
        public bool isWaitingForDelivery;
        public CelestialBody currentOrbit;
        public double lfoCost;
        public double lfCost;
        public double oCost;
        public double mCost;
        public double xCost;
        public double batCost;
        public double repCost;
        public double totalPrice;
        public double modCost;
        public double lfDif;
        public double oDif;
        public double mDif;
        public double xDif;
        public double batDif;
        public double runningFuel;
        public double creditAmount = 0;
        public List<double> basePrices;
        private static double lfoCost2;
        private static double lfCost2;
        private static double oCost2;
        private static double mCost2;
        private static double xCost2;
        private static double batCost2;
        private static double repCost2;
        public static GasStation Instance;

        [KSPField(isPersistant = true)]
        public double timeTillNextDel;

        [KSPField(isPersistant = true)]
        public double timerEnd;


        public void Start()
        {
            Instance = this;

            try
            {
                basePrices = new List<double>();
                currentOrbit = FlightGlobals.ActiveVessel.mainBody;
                string strOrbit = currentOrbit.displayName;

                GrapeUtils priceModifier = new GrapeUtils();
                modCost = priceModifier.DistanceCalculator(strOrbit);
                basePrices = priceModifier.BaseRates();

                lfoCost = Math.Round(basePrices[0] * modCost, 2);
                lfCost = Math.Round(basePrices[1] * modCost, 2);
                oCost = Math.Round(basePrices[2] * modCost, 2);
                mCost = Math.Round(basePrices[3] * modCost, 2);
                xCost = Math.Round(basePrices[4] * modCost, 2);
                batCost = Math.Round(basePrices[5] * modCost, 2);
                repCost = Math.Round(basePrices[6] * modCost, 2);

                totalPrice = 0;

            }
            catch
            {
                // internal error
            }
        }

        public void Update()
        {
            // refreshes prices if station moves to another area

            CelestialBody loc = FlightGlobals.ActiveVessel.mainBody;

            if (loc != currentOrbit)
            {
                basePrices = new List<double>();
                currentOrbit = loc;
                string strOr2 = currentOrbit.displayName;
                GrapeUtils priceModifier = new GrapeUtils();
                modCost = priceModifier.DistanceCalculator(strOr2);
                basePrices = priceModifier.BaseRates();

                lfoCost = Math.Round(basePrices[0] * modCost, 2);
                lfCost = Math.Round(basePrices[1] * modCost, 2);
                oCost = Math.Round(basePrices[2] * modCost, 2);
                mCost = Math.Round(basePrices[3] * modCost, 2);
                xCost = Math.Round(basePrices[4] * modCost, 2);
                batCost = Math.Round(basePrices[5] * modCost, 2);
                repCost = Math.Round(basePrices[6] * modCost, 2);
            }
        }

        // calculates how much fuel the player 'needs'
        public static double GetFuelAmount()
        {
            Instance.lfDif = 0;
            Instance.oDif = 0;
            Instance.mDif = 0;
            Instance.xDif = 0;

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_1")))
                {
                    if (part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).amount < part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).maxAmount)
                    {
                        Instance.lfDif += part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).maxAmount - part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).amount;
                    }
                }

                if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_2")))
                {
                    if (part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).amount < part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).maxAmount)
                    {
                        Instance.oDif += part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).maxAmount - part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).amount;
                    }
                }

                if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_3")))
                {
                    if (part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).amount < part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).maxAmount)
                    {
                        Instance.mDif += part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).maxAmount - part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).amount;
                    }
                }

                if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_4")))
                {
                    if (part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).amount < part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).maxAmount)
                    {
                        Instance.xDif += part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).maxAmount - part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).amount;
                    }
                }

            }

            if (Instance.lfDif > 0 || Instance.oDif > 0 || Instance.mDif > 0 || Instance.xDif > 0)
            {
                Instance.runningFuel = 0;
                Instance.runningFuel = (Instance.lfDif * Instance.lfCost) + (Instance.oDif * Instance.oCost) +
                    (Instance.mDif * Instance.mCost) + (Instance.xDif * Instance.xCost);
                Instance.totalPrice = Math.Round(Instance.runningFuel, 0);

                if (Instance.totalPrice < 1)
                {
                    Instance.totalPrice = 1;
                }

                return Instance.totalPrice;
            }

            else return 0;

        }

        // get the total credit
        public static double GetCreditAmount()
        {
            List<Part> cardList = new List<Part>();
            double totalCredit = 0;

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.HasModuleImplementing<FuelCard>())
                {
                    cardList.Add(part);
                }
            }

            if (cardList.Count == 0)
            {
                Instance.creditAmount = totalCredit;
                return 0;
            }
            else
            {
                for (int x = 0; x < cardList.Count; x++)
                {
                    if (cardList[x].GetComponent<FuelCard>().availableCredit > 0)
                    {
                        totalCredit += cardList[x].GetComponent<FuelCard>().availableCredit;
                    }
                }

                Instance.creditAmount = totalCredit;

                if (totalCredit == 0)
                {

                    return 0;
                }
                else return totalCredit;
            }

        }

        // checks if the player has battery capacity available to allow recharge
        public static bool QueryRecharge()
        {
            double batDif = 0;

            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_5")))
                {
                    if (part.Resources.Get(Localizer.Format("#LOC_GRAPES_5")).amount < part.Resources.Get(Localizer.Format("#LOC_GRAPES_5")).maxAmount)
                    {
                        batDif += part.Resources.Get(Localizer.Format("#LOC_GRAPES_5")).maxAmount - part.Resources.Get(Localizer.Format("#LOC_GRAPES_5")).amount;
                    }
                }
            }

            Instance.batDif = batDif;

            if (batDif > 1)
            {
                return true;
            }

            else return false;

        }

        // sends price list to GUI
        public static List<double> ProvidePrices()
        {
            List<double> sendList = new List<double>();

            lfoCost2 = Instance.lfoCost;
            lfCost2 = Instance.lfCost;
            oCost2 = Instance.oCost;
            mCost2 = Instance.mCost;
            xCost2 = Instance.xCost;
            batCost2 = Instance.batCost;
            repCost2 = Instance.repCost;

            double[] allOfThem =
            {
                lfoCost2,
                lfCost2,
                oCost2,
                mCost2,
                xCost2,
                batCost2,
                repCost2
            };

            sendList.AddRange(allOfThem);

            return sendList;
        }

        // refuel procedure
        public static int Refuel()
        {
            double _creditAmount = Instance.creditAmount;
            double _refuelCost = Instance.totalPrice;
            double difference = _creditAmount - _refuelCost;
            double fuelToAdd;


            if (difference > 0)
            {
                // can refuel

                TakePayment(_refuelCost);

                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_1")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).amount = part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).maxAmount;
                    }
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_2")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).amount = part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).maxAmount;
                    }
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_3")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).amount = part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).maxAmount;
                    }
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_4")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).amount = part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).maxAmount;
                    }
                }

                TimerEvent();
                return 1;

            }

            else
            {
                TakePayment(0);

                fuelToAdd = (100 / _refuelCost) * _creditAmount;

                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_1")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).amount = (part.Resources.Get(Localizer.Format("#LOC_GRAPES_1")).maxAmount / 100) * fuelToAdd;
                    }
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_2")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).amount = (part.Resources.Get(Localizer.Format("#LOC_GRAPES_2")).maxAmount / 100) * fuelToAdd;
                    }
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_3")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).amount = (part.Resources.Get(Localizer.Format("#LOC_GRAPES_3")).maxAmount / 100) * fuelToAdd;
                    }
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_4")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).amount = (part.Resources.Get(Localizer.Format("#LOC_GRAPES_4")).maxAmount / 100) * fuelToAdd;
                    }
                }

                TimerEvent();
                return 2;

            }

        }

        // recharge procedure
        public static int Recharge()
        {

            double _creditAmount = Instance.creditAmount;
            double _rechargeRate = Instance.batCost;
            double difference = _creditAmount - _rechargeRate;

            if (difference < 0)
            {
                return 3;
            }

            else
            {
                TakePayment(_rechargeRate);

                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.Resources.Contains(Localizer.Format("#LOC_GRAPES_5")))
                    {
                        part.Resources.Get(Localizer.Format("#LOC_GRAPES_5")).amount = part.Resources.Get(Localizer.Format("#LOC_GRAPES_5")).maxAmount;
                    }
                }


                return 4;

            }

        }

        // take payment
        public static void TakePayment(double typeToTake)
        {
            if (typeToTake == 0)
            {
                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.HasModuleImplementing<FuelCard>())
                    {
                        part.GetComponent<FuelCard>().availableCredit = 0;
                    }
                }
            }

            else
            {

                foreach (var part in FlightGlobals.ActiveVessel.Parts)
                {
                    if (part.HasModuleImplementing<FuelCard>())
                    {
                        if (part.GetComponent<FuelCard>().availableCredit < typeToTake)
                        {
                            typeToTake -= part.GetComponent<FuelCard>().availableCredit;
                            part.GetComponent<FuelCard>().availableCredit = 0;
                        }

                        else if (part.GetComponent<FuelCard>().availableCredit >= typeToTake)
                        {
                            part.GetComponent<FuelCard>().availableCredit -= typeToTake;
                        }
                    }
                }


            }



        }

        // handles timer event after player refuels (simulates waiting for redelivery)
        public static void TimerEvent()
        {
            // in seconds... useful...

            double thisTime = Planetarium.fetch.time;

            // a week

            double fixedStdTime = 151200;

            CelestialBody loc = FlightGlobals.ActiveVessel.mainBody;
            GrapeUtils gU = new GrapeUtils();

            double timeModifier = gU.TimeCalculator(loc.displayName);
            double revisedTime = fixedStdTime * timeModifier;
            double timeOfNextDelivery = thisTime + revisedTime;
            double timeAsDays = Math.Round((timeOfNextDelivery / 21600), 0);

            Instance.timeTillNextDel = timeAsDays;
            Instance.timerEnd = timeOfNextDelivery;


        }

        // sends timer detail to GUI
        public static double TimerEnd()
        {
            return Instance.timerEnd;
        }

    }
}

