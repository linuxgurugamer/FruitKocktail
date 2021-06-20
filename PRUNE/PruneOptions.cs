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


        public override string Title { get { return "PRUNES Options"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "PRUNES"; } }
        public override string DisplaySection { get { return "PRUNES"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomStringParameterUI("Options", autoPersistance = true, lines = 2, 
            title = "Choose Your Editor Sort Options")]
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
            if (member.Name == "EnabledForSave")
                return true;

            return true;
        }

        


    }
}
