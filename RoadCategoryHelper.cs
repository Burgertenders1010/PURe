using ColossalFramework;

namespace PURepipeconnectorsys
{
    public static class RoadCategoryHelper
    {
        public static bool ShouldApplyPipe(NetInfo info)
        {
            if (info == null) return false;

            string prefabName = info.name;

            if (ModSettings.SpecificRoadOverrides.TryGetValue(prefabName, out bool overrideValue))
                return overrideValue;

            if (!ModSettings.EnableAutoPipes)
                return false;

            if (info.m_class.m_service != ItemClass.Service.Road)
                return false;

            string lower = prefabName.ToLower();

            if (lower.Contains("gravel") || lower.Contains("dirt"))
                return ModSettings.ApplyDirtRoads;

            if (lower.Contains("highway"))
                return ModSettings.ApplyHighways;

            if (info.m_hasForwardVehicleLanes && !info.m_hasBackwardVehicleLanes)
                return ModSettings.ApplyOneWays;

            int laneCount = CountVehicleLanes(info);
            if (laneCount >= 4)
                return ModSettings.ApplyAvenues;

            return ModSettings.ApplyStreets;
        }

        private static int CountVehicleLanes(NetInfo info)
        {
            int count = 0;
            foreach (var lane in info.m_lanes)
            {
                if (lane.m_laneType == NetInfo.LaneType.Vehicle)
                    count++;
            }
            return count;
        }
    }
}
