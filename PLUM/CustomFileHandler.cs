using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace ParachutesLetsUseMaths
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class CustomFileHandler : MonoBehaviour
    {
        // KSP directory reference
        public string dataDirectory;

        // filename references
        public string fileName = "data.plum";
        public string tempFileName = "data.txt";
        public string pathToData;
        public string tempPathtoData;
        public List<string> cfgContents;
        public static CfgHandler cfgHandler;
        public static CustomFileHandler Instance;
        public bool runOnce;


        public void Start()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                runOnce = false;

                try
                {
                    // set all paths assuming they exist
                    dataDirectory = KSPUtil.ApplicationRootPath + "/GameData/FruitKocktail/PLUM/PluginData/";
                    pathToData = dataDirectory + fileName;
                    tempPathtoData = dataDirectory + tempFileName;
                    ImportDataFromFile();

                }
                catch
                {
                    Debug.LogError("ERROR - PLUM: Unable to detect PluginData directory!");
                }


            }
            else return;
        }

        // import the data...
        public void ImportDataFromFile()
        {
            if (File.Exists(pathToData))
            {
                FileInfo fileInfo = new FileInfo(pathToData);
                fileInfo.MoveTo(tempPathtoData);

                cfgContents = new List<string>(File.ReadAllLines(tempPathtoData));

            }
            else
            {
                Debug.LogError("ERROR - PLUM: Unable to find plumdata.cfg!");
            }

            if (cfgContents.Count != 0)
            {
                FileInfo fileInfo2 = new FileInfo(tempPathtoData);
                fileInfo2.MoveTo(pathToData);

                if (!runOnce)
                {
                    ProcessCfg();
                    runOnce = true;
                }
                else
                {
                    ReProcess();
                }
            }
        }

        // send imported data to Handler
        public void ProcessCfg()
        {
            Instance = this;
            cfgHandler = new CfgHandler(cfgContents);
        }

        // send new saved data to Handler
        public void ReProcess()
        {
            cfgHandler = CfgHandler.Instance;
            cfgHandler.RefreshData(cfgContents);
        }

        // physically ammend and save the data
        public void SaveData(string id, string name, string grav, string aD, string c0, string c1, string c2, string c3, string c4)
        {
            FileInfo fileInfo = new FileInfo(pathToData);
            fileInfo.MoveTo(tempPathtoData);
            cfgContents = new List<string>(File.ReadAllLines(tempPathtoData));
            int cfCount = cfgContents.Count();

            if (cfCount != 0)
            {
                for (int x = 0; x < cfCount - 1; x++)
                {
                    if (cfgContents[x].Contains("id"))
                    {
                        if (cfgContents[x].Contains(id))
                        {
                            int scPos = cfgContents[x + 1].IndexOf(";");
                            int eqPos = cfgContents[x + 1].IndexOf("=") + 1;
                            string selection = cfgContents[x + 1].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 1] = cfgContents[x + 1].Replace(selection, name);

                            scPos = cfgContents[x + 2].IndexOf(";");
                            eqPos = cfgContents[x + 2].IndexOf("=") + 1;
                            selection = cfgContents[x + 2].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 2] = cfgContents[x + 2].Replace(selection, grav);

                            scPos = cfgContents[x + 3].IndexOf(";");
                            eqPos = cfgContents[x + 3].IndexOf("=") + 1;
                            selection = cfgContents[x + 3].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 3] = cfgContents[x + 3].Replace(selection, aD);

                            scPos = cfgContents[x + 4].IndexOf(";");
                            eqPos = cfgContents[x + 4].IndexOf("=") + 1;
                            selection = cfgContents[x + 4].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 4] = cfgContents[x + 4].Replace(selection, c0);

                            scPos = cfgContents[x + 5].IndexOf(";");
                            eqPos = cfgContents[x + 5].IndexOf("=") + 1;
                            selection = cfgContents[x + 5].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 5] = cfgContents[x + 5].Replace(selection, c1);

                            scPos = cfgContents[x + 6].IndexOf(";");
                            eqPos = cfgContents[x + 6].IndexOf("=") + 1;
                            selection = cfgContents[x + 6].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 6] = cfgContents[x + 6].Replace(selection, c2);

                            scPos = cfgContents[x + 7].IndexOf(";");
                            eqPos = cfgContents[x + 7].IndexOf("=") + 1;
                            selection = cfgContents[x + 7].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 7] = cfgContents[x + 7].Replace(selection, c3);

                            scPos = cfgContents[x + 8].IndexOf(";");
                            eqPos = cfgContents[x + 8].IndexOf("=") + 1;
                            selection = cfgContents[x + 8].Substring(eqPos, scPos - eqPos);
                            cfgContents[x + 8] = cfgContents[x + 8].Replace(selection, c4);

                        }
                    }
                }

                File.WriteAllText(tempPathtoData, string.Empty);
                File.AppendAllLines(tempPathtoData, cfgContents);
                fileInfo.MoveTo(pathToData);
                ImportDataFromFile();

            }
        }


    }
}
