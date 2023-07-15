using AzeLib;
using BepInEx.Configuration;

namespace FootprintPerformance
{
    public partial class AzePlugin
    {
        public static ConfigEntry<int> MaxTrails { get; private set; }
        public static ConfigEntry<float> DegSpeedOutside { get; private set; }
        public static ConfigEntry<float> DegSpeedInside { get; private set; }
        public static ConfigEntry<float> DegSpeedOutsideRain { get; private set; }
        public static ConfigEntry<bool> ShouldFixMissingTrails { get; private set; }
        public static ConfigEntry<float> TrailUpdateThreshold { get; private set; }

        partial void OnAwake()
        {
            // General
            MaxTrails = Config.Bind(string.Empty, "Max Footprints", 1000,
                new ConfigDescription("The maximum number of footprints allowed.\n" +
                "If this threshold is exceeded, the oldest footprints will be marked for deletion as soon as they leave the camera view.",
                new AcceptableValueRange<int>(0, 10000), null));

            // Footprint Fade Rate
            var fadeCat = "Footprint Fade Rate";
            string GetFadeDesc(string loc, float vanillaVal) => $"The rate (%/s) that footprints fade when {loc}. (Unmodded = {vanillaVal}%/s)";
            var acceptableVals = new AcceptableValueRange<float>(0f, 100f);

            DegSpeedOutside = Config.Bind(fadeCat, "Outside", 1f,
                new ConfigDescription(GetFadeDesc("outside", TrailObject.DEGRADE_SPEED_OUTSIDE),
                acceptableVals, new ConfigurationManagerAttributes() { Order = 0 }));
            DegSpeedInside = Config.Bind(fadeCat, "Inside", 2.5f,
                new ConfigDescription(GetFadeDesc("inside", TrailObject.DEGRADE_SPEED_INSIDE),
                acceptableVals, new ConfigurationManagerAttributes() { Order = -1 }));
            DegSpeedOutsideRain = Config.Bind(fadeCat, "Rain", 10f,
                new ConfigDescription(GetFadeDesc("in the rain", TrailObject.DEGRADE_SPEED_OUTSIDE_RAIN),
                acceptableVals, new ConfigurationManagerAttributes() { Order = -2 }));

            // Visual Tweaks
            var tweakCat = "Visual Tweaks";

            ShouldFixMissingTrails = Config.Bind(tweakCat, "Fix Missing Footprints", true,
                new ConfigDescription("If enabled: fixes common edge cases in which the game fails to leave a footprint.\n" +
                "(Restart required to apply setting changes.)",
                null, new ConfigurationManagerAttributes() { Order = 0, IsAdvanced = true }));
            TrailUpdateThreshold = Config.Bind(tweakCat, "Footprint Style Update Threshold", 0.5f,
                new ConfigDescription($"The threshold value to use when moving onto ground that has its own footprints. (Unmodded = {OverrideTrailUpdateThresh.UNMODDED_THRESH:0%})",
                new AcceptableValueRange<float>(OverrideTrailUpdateThresh.UNMODDED_THRESH, 1f), new ConfigurationManagerAttributes() { Order = -1, ShowRangeAsPercent = true, IsAdvanced = true }));
        }
    }
}
