# Pipes under Roads (PURepipeconnectorsys)

Cities: Skylines (1) mod — automatically places underground water pipes under
roads as they're built, matching road curvature, connecting at intersections,
and removing pipes when roads are deleted.

## Requirements

- Cities: Skylines (base game)
- **Harmony (CitiesHarmony)** by boformer — Steam Workshop dependency, required
  at runtime. Subscribe and enable it alongside this mod.

## Project setup

- Target framework: **.NET Framework 3.5**
- NuGet package: **CitiesHarmony.API** (pulls in `CitiesHarmony.Harmony` too)
- References needed from the game's `Managed` folder:
  `Assembly-CSharp.dll`, `ColossalManaged.dll`, `ICities.dll`, `UnityEngine.dll`
- Do **not** reference a raw `0Harmony.dll` alongside `CitiesHarmony.API` —
  pick one (the NuGet package) or you'll get ambiguous `Harmony` type errors.

## Files

| File | Purpose |
|---|---|
| `ModManager.cs` | `IUserMod` entry point + Options-menu settings UI |
| `Patcher.cs` | Harmony patch bootstrap (patch/unpatch on mod enable/disable) |
| `ModSettings.cs` | User-configurable toggles (category filters, auto-remove, etc.) |
| `RoadCategoryHelper.cs` | Decides whether a given road prefab should get a pipe |
| `PipeSegmentTracker.cs` | In-memory road-segment/node ↔ pipe-segment/node ID mapping |
| `RoadAIPatch.cs` | Harmony postfix/prefix patches on `RoadBaseAI.CreateSegment` / `ReleaseSegment` |
| `PipeSpawner.cs` | Creates/removes the underground pipe `NetSegment`/`NetNode`s |

## Known limitations / next steps

- **`UpdateSegment` is intentionally not patched.** An earlier attempt to patch
  `RoadBaseAI.UpdateSegment` threw a Harmony "Undefined target method" error at
  startup; it was removed rather than resolved. Practical effect: pipes are
  placed correctly at road-creation time (including curves), but a road that's
  reshaped *after* placement won't re-sync its pipe until you delete/replace it.
- **`PipeSegmentTracker`'s dictionaries are in-memory only** — they are not
  hooked into the game's save/load serialization (no `ISerializableDataExtension`
  implemented). They rebuild as you place new segments in a session, but a
  fresh game load starts them empty. In practice this was tested as stable
  (no crash, no orphaned-pipe issue observed), but if you see roads placed in
  a *previous* session fail to auto-remove their pipe after a reload, this is
  why — wiring up persistence is the fix.
- **Pipe prefab is hardcoded** to whatever `PrefabCollection<NetInfo>.FindLoaded("Water Pipe")`
  resolves to — verify this exact prefab name is correct for your game version
  via ModTools/dnSpy if pipes fail to spawn.
- **Heating pipes (Snowfall DLC) support was planned but not implemented** in
  this version — see chat history for the sketched `DLCHelper.cs` /
  `ModSettings.UseHeatingPipes` approach if picking this back up. The DLC
  ownership check (`SteamHelper.IsDLCOwned`) should be verified against the
  real `SteamHelper.DLC` enum in dnSpy before relying on it.
- **"Specific road" override UI** (per-prefab checkboxes, as opposed to broad
  categories) was planned in `ModSettings.SpecificRoadOverrides` but the
  settings-UI list-population code was never added.
- Category matching in `RoadCategoryHelper` uses **name-string heuristics and
  lane counts** rather than `ItemClass.SubService` enum matching — earlier
  attempts to switch on `SubService.RoadHighway` / `RoadBridge` / `RoadTunnel` /
  `RoadSlope` failed to compile (`does not contain a definition for`), meaning
  those exact enum member names are wrong for this game version. If refining
  categorization, check the real enum in dnSpy first.

## Build & deploy

1. Build (Debug while testing, Release before publishing).
2. Copy the output DLL plus `CitiesHarmony.API.dll` (and `CitiesHarmony.Harmony.dll`
   if present) from `bin\Debug\` (or `bin\Release\`) into:
   `%LocalAppData%\Colossal Order\Cities_Skylines\Addons\Mods\PURepipeconnectorsys\`
3. Enable both this mod and "Harmony" in Content Manager → Mods.
