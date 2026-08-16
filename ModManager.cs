using ICities;
using CitiesHarmony.API;

namespace PURepipeconnectorsys
{
    public class ModManager : IUserMod
    {
        public string Name => "Piped Roads";
        public string Description => "Automatically places pipes under roads.";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled)
                Patcher.UnpatchAll();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase mainGroup = helper.AddGroup("Piped Roads Settings");

            mainGroup.AddCheckbox("Enable auto-pipes", ModSettings.EnableAutoPipes,
                (b) => ModSettings.EnableAutoPipes = b);

            mainGroup.AddCheckbox("Auto-remove pipes when road is deleted", ModSettings.AutoRemoveWithRoad,
                (b) => ModSettings.AutoRemoveWithRoad = b);

            UIHelperBase categoryGroup = helper.AddGroup("Road Categories");
            categoryGroup.AddCheckbox("Highways", ModSettings.ApplyHighways, (b) => ModSettings.ApplyHighways = b);
            categoryGroup.AddCheckbox("Avenues", ModSettings.ApplyAvenues, (b) => ModSettings.ApplyAvenues = b);
            categoryGroup.AddCheckbox("Streets / Local roads", ModSettings.ApplyStreets, (b) => ModSettings.ApplyStreets = b);
            categoryGroup.AddCheckbox("Dirt roads", ModSettings.ApplyDirtRoads, (b) => ModSettings.ApplyDirtRoads = b);
            categoryGroup.AddCheckbox("One-way roads", ModSettings.ApplyOneWays, (b) => ModSettings.ApplyOneWays = b);
        }
    }
}
