using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AutomatedPopularPreLaunchExperiment
{
    public class AppleOptions : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "General Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "APPLE"; } }
        public override string DisplaySection { get { return "APPLE"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Auto SAS on for ships and probes")]
        public bool sasOn = true;

        [GameParameters.CustomParameterUI("Set display to Ap/Pe info")]
        public bool manNodeModeOn = true;

        [GameParameters.CustomParameterUI("Set brakes for rovers & planes")]
        public bool brakesOn = true;

        [GameParameters.CustomParameterUI("Vessel lights use auto light sensors")]
        public bool shipLightsOn = true;

        [GameParameters.CustomParameterUI("Kerbal visors use auto light sensors")]
        public bool visorOn = true;

        [GameParameters.CustomParameterUI("Kerbal lights use auto light sensors")]
        public bool kerbalLightsOn = true;

        [GameParameters.CustomFloatParameterUI("Kerbal removes/adds helmet where possible")]
        public bool kerbalRemoveHelmet = true;

        [GameParameters.CustomParameterUI("SAS mode set automatically")]
        public bool autoSetSAS = true;

        [GameParameters.CustomParameterUI("Warp lead time = 10 seconds")]
        public bool warp10 = true;

        [GameParameters.CustomParameterUI("Autodeploy landing gear at 1000m")]
        public bool gear250 = true;

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
