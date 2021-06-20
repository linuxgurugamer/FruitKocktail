using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PotentiallyReallyUsefulNewEditorSorter
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class PRUNES : MonoBehaviour
    {

        // convert enum result to int for sort by type
        public int SetSortMethod(PruneOptions.SortBy sb)
        {
            switch (sb)
            {
                case PruneOptions.SortBy.NAME:
                    return 0;
                case PruneOptions.SortBy.MASS:
                    return 1;
                case PruneOptions.SortBy.COST:
                    return 2;
                case PruneOptions.SortBy.SIZE:
                    return 3;
                default:
                    return 1;
            }

        }

        // convert enum result to int for direction
        public int SetDirMethod(PruneOptions.AscDesc ad)
        {
            switch (ad)
            {
                case PruneOptions.AscDesc.Ascending:
                    return 0;
                case PruneOptions.AscDesc.Descending:
                    return 1;
                default:
                    return 0;
            }

        }

        // apply the settings in editor
        public void SetPrefMethod(int code, int dCode)
        {
            if (code != 0)
            {
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.partListSorter.ClickButton(0);
            }
            else
            {
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.partListSorter.ClickButton(1);
            }


            if (dCode == 0)
            {
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.partListSorter.ClickButton(code);
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.Refresh();
            }

            else
            {
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.partListSorter.ClickButton(code);
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.partListSorter.ClickButton(code);
                KSP.UI.Screens.PartCategorizer.Instance.editorPartList.Refresh();
            }
        } 


        public void Start()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                PruneOptions.SortBy sB = HighLogic.CurrentGame.Parameters.CustomParams<PruneOptions>().sortBy;
                PruneOptions.AscDesc aD = HighLogic.CurrentGame.Parameters.CustomParams<PruneOptions>().ascDesc;
                int choiceCode = SetSortMethod(sB);
                int dirChoice = SetDirMethod(aD);
                SetPrefMethod(choiceCode, dirChoice);
            }
        }

    }
}
