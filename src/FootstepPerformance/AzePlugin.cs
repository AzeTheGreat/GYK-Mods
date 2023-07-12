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

        partial void OnAwake()
        {
            MaxTrails = Config.Bind(string.Empty, "Max Footprints", 1000,
                new ConfigDescription("The maximum number of footprints allowed.\n" +
                "If this threshold is exceeded, the oldest footprints will be marked for deletion as soon as they leave the camera view.",
                new AcceptableValueRange<int>(0, 10000), null));

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
        }
    }
}
