using HarmonyLib;

namespace PURepipeconnectorsys
{
    public static class Patcher
    {
        private const string HarmonyId = "purepipeconnectorsys.piperoads";
        private static Harmony harmony;

        public static void PatchAll()
        {
            harmony = new Harmony(HarmonyId);
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public static void UnpatchAll()
        {
            harmony?.UnpatchAll(HarmonyId);
        }
    }
}
