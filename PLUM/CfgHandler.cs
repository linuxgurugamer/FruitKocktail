using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ParachutesLetsUseMaths
{
    public class CfgHandler
    {
        private List<string> cfgData;

        // list of custom entries per profile
        private List<string> custom1Entries = new List<string>();
        private List<string> custom2Entries = new List<string>();
        private List<string> custom3Entries = new List<string>();
        private List<string> custom4Entries = new List<string>();
        private List<string> custom5Entries = new List<string>();
        private List<string> custom6Entries = new List<string>();
        private List<string> custom7Entries = new List<string>();
        private List<string> custom8Entries = new List<string>();
        private List<string> custom9Entries = new List<string>();

        // static instances
        public static CustomFileHandler customFileHandler;
        public static CfgHandler Instance;

        // main
        public CfgHandler(List<string> _cfgData)
        {
            Instance = this;
            cfgData = _cfgData;
            SortData();
        }

        // sorts the data...
        private void SortData()
        {
            if (cfgData[0] != "DATA")
            {
                Debug.LogError("ERROR - PLUM : cfg file is not in the correct format!");
            }

            int lineCount = cfgData.Count;

            for (int x = 0; x < lineCount; x++)
            {
                cfgData[x] = cfgData[x].Trim();
                cfgData[x] = cfgData[x].Replace(" ", "");
            }

            for (int y = 0; y < lineCount; y++)
            {

                if (cfgData[y].Contains("id"))
                {

                    int posOfE = cfgData[y].IndexOf("=") + 1;
                    int posOfSC = cfgData[y].IndexOf(";");
                    string buildStr1 = cfgData[y].Substring(posOfE, posOfSC - posOfE);
                    string string1 = buildStr1.Trim();


                    posOfE = cfgData[y + 1].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 1].IndexOf(";");
                    buildStr1 = cfgData[y + 1].Substring(posOfE, posOfSC - posOfE);
                    string string2 = buildStr1.Trim();


                    posOfE = cfgData[y + 2].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 2].IndexOf(";");
                    buildStr1 = cfgData[y + 2].Substring(posOfE, posOfSC - posOfE);
                    string string3 = buildStr1.Trim();


                    posOfE = cfgData[y + 3].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 3].IndexOf(";");
                    buildStr1 = cfgData[y + 3].Substring(posOfE, posOfSC - posOfE);
                    string string4 = buildStr1.Trim();


                    posOfE = cfgData[y + 4].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 4].IndexOf(";");
                    buildStr1 = cfgData[y + 4].Substring(posOfE, posOfSC - posOfE);
                    string string5 = buildStr1.Trim();


                    posOfE = cfgData[y + 5].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 5].IndexOf(";");
                    buildStr1 = cfgData[y + 5].Substring(posOfE, posOfSC - posOfE);
                    string string6 = buildStr1.Trim();


                    posOfE = cfgData[y + 6].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 6].IndexOf(";");
                    buildStr1 = cfgData[y + 6].Substring(posOfE, posOfSC - posOfE);
                    string string7 = buildStr1.Trim();


                    posOfE = cfgData[y + 7].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 7].IndexOf(";");
                    buildStr1 = cfgData[y + 7].Substring(posOfE, posOfSC - posOfE);
                    string string8 = buildStr1.Trim();


                    posOfE = cfgData[y + 8].IndexOf("=") + 1;
                    posOfSC = cfgData[y + 8].IndexOf(";");
                    buildStr1 = cfgData[y + 8].Substring(posOfE, posOfSC - posOfE);
                    string string9 = buildStr1.Trim();

                    string[] vals1 =
                    {
                        string1,
                        string2,
                        string3,
                        string4,
                        string5,
                        string6,
                        string7,
                        string8,
                        string9,
                    };

                    switch (string1)
                    {
                        case "1":
                            custom1Entries.AddRange(vals1);
                            break;
                        case "2":
                            custom2Entries.AddRange(vals1);
                            break;
                        case "3":
                            custom3Entries.AddRange(vals1);
                            break;
                        case "4":
                            custom4Entries.AddRange(vals1);
                            break;
                        case "5":
                            custom5Entries.AddRange(vals1);
                            break;
                        case "6":
                            custom6Entries.AddRange(vals1);
                            break;
                        case "7":
                            custom7Entries.AddRange(vals1);
                            break;
                        case "8":
                            custom8Entries.AddRange(vals1);
                            break;
                        case "9":
                            custom9Entries.AddRange(vals1);
                            break;
                        default:
                            Debug.LogError("ERROR - PLUM : Unable to assign cfg values to lists!");
                            break;
                    }


                }

            }


        }

        // refreshes data following save
        public void RefreshData(List<string> _cfgData)
        {
            cfgData = _cfgData;
            custom1Entries.Clear();
            custom2Entries.Clear();
            custom3Entries.Clear();
            custom4Entries.Clear();
            custom5Entries.Clear();
            custom6Entries.Clear();
            custom7Entries.Clear();
            custom8Entries.Clear();
            custom9Entries.Clear();
            SortData();
            GUIElements.PopulateLists();
        }

        // sends the data
        public List<string> ReturnData(int index)
        {
            switch (index)
            {
                case 0:
                    return custom1Entries;
                case 1:
                    return custom2Entries;
                case 2:
                    return custom3Entries;
                case 3:
                    return custom4Entries;
                case 4:
                    return custom5Entries;
                case 5:
                    return custom6Entries;
                case 6:
                    return custom7Entries;
                case 7:
                    return custom8Entries;
                case 8:
                    return custom9Entries;
                default:
                    return null;


            }



        }

        // saves the data
        public void SaveProfile(int index, string name, float grav, float aD, float c0, float c1, float c2, float c3, float c4)
        {
            customFileHandler = CustomFileHandler.Instance;

            index = index == 8 ? 0 : index + 1;

            string index2 = index.ToString();
            string grav2 = grav.ToString();
            string aD2 = aD.ToString();
            string c02 = c0.ToString();
            string c12 = c1.ToString();
            string c22 = c2.ToString();
            string c32 = c3.ToString();
            string c42 = c4.ToString();

            customFileHandler.SaveData(index2, name, grav2, aD2, c02, c12, c22, c32, c42);


        }



    }
}
