using KSP.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace VSIndicator
{
    public class VSIOptions : GameParameters.CustomParameterNode
    {
        public override string Title { get { return Localizer.Format("#LOC_VSIndicator_16"); } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return Localizer.Format("#LOC_VSIndicator_17"); } }
        public override string DisplaySection { get { return Localizer.Format("#LOC_VSIndicator_17"); } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        // button to disable the toolbar button
        [GameParameters.CustomParameterUI("Disable Toolbar Button")]
        public bool disableButton = false;

        // stores the preferred colour 
        [GameParameters.CustomParameterUI("Saved Ascending Colour")]
        public string ascCol;

        // stores the preferred colour 
        [GameParameters.CustomParameterUI("Saved Descending Colour")]
        public string desCol;

        // stores the preferred colour 
        [GameParameters.CustomParameterUI("Saved Safe Velocity Colour")]
        public string safCol;

        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (member.Name == Localizer.Format("#LOC_VSIndicator_18"))
                return true;

            return true;
        }


    }
}
