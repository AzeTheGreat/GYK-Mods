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
            MaxTrails = Config.Bind("General", "Max Footprints", 100, "The maximum number of footprints allowed.\n" +
                "If this threshold is exceeded, the oldest footprints will be marked for deletion as soon as they leave the camera view.");
            DegSpeedOutside = Config.Bind("General", "Degradation Rate (Outside)", 1f, "The rate that footprints degrade when outside. (Unmodded = 0.1)");
            DegSpeedInside = Config.Bind("General", "Degradation Rate (Inside)", 2.5f, "The rate that footprints degrade when inside. (Unmodded = 2.5)");
            DegSpeedOutsideRain = Config.Bind("General", "Degradation Rate (Rain)", 10f, "The rate that footprints degrade when in the rain. (Unmodded = 10)");        
        }
    }
}
