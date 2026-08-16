using HarmonyLib;

namespace PURepipeconnectorsys
{
    [HarmonyPatch(typeof(RoadBaseAI), "CreateSegment")]
    public static class RoadAI_CreateSegment_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ushort segmentID, ref NetSegment data, RoadBaseAI __instance)
        {
            NetInfo info = __instance.m_info;
            if (!RoadCategoryHelper.ShouldApplyPipe(info))
                return;

            PipeSpawner.SpawnPipeForSegment(segmentID, ref data);
        }
    }

    [HarmonyPatch(typeof(RoadBaseAI), "ReleaseSegment")]
    public static class RoadAI_ReleaseSegment_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(ushort segmentID, ref NetSegment data)
        {
            if (!ModSettings.AutoRemoveWithRoad)
                return;

            PipeSpawner.RemovePipeForSegment(segmentID);
        }
    }
}
