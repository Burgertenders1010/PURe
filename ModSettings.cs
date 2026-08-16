using System.Collections.Generic;

namespace PURepipeconnectorsys
{
    public static class ModSettings
    {
        public static bool EnableAutoPipes = true;
        public static bool AutoRemoveWithRoad = true;

        // Broad category toggles
        public static bool ApplyHighways = true;
        public static bool ApplyAvenues = true;
        public static bool ApplyStreets = true;
        public static bool ApplyDirtRoads = false;
        public static bool ApplyOneWays = true;

        // Specific road name overrides (prefab name -> enabled)
        public static Dictionary<string, bool> SpecificRoadOverrides = new Dictionary<string, bool>();
    }
}
