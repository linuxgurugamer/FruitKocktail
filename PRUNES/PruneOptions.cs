using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace PotentiallyReallyUsefulNewEditorSorter
{
    public class PruneOptions : GameParameters.CustomParameterNode
    {
        // sorting choices 
        public enum SortBy
        {
            NAME,
            MASS,
            COST,
            SIZE,
        } 

        // direction choices
        public enum AscDesc
        {
            Ascending,
            Descending,
        }


        public override string Title { get { return Localizer.Format("#LOC_PRUNES_1"); } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return Localizer.Format("#LOC_PRUNES_2"); } }
        public override string DisplaySection { get { return Localizer.Format("#LOC_PRUNES_2"); } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomStringParameterUI("Options", autoPersistance = true, lines = 2, 
            title = "#LOC_PRUNES_3")]
        public string optStr = "";

        [GameParameters.CustomParameterUI("Editor Sort By")]
        public SortBy sortBy = SortBy.MASS;

        [GameParameters.CustomParameterUI("Ascending / Descending")]
        public AscDesc ascDesc = AscDesc.Ascending;


        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            #region NO_LOCALIZATION
            if (member.Name == "EnabledForSave")
                return true;
            #endregion
            return true;
        }

        


    }
}
