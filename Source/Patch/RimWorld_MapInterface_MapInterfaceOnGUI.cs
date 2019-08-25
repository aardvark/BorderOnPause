using Harmony;
using RimWorld;

namespace BorderOnPause.Patch
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
    internal static class RimWorld_MapInterface_MapInterfaceOnGUI_BeforeMainTabs
    {
        private static void Prefix() => Core.InitDraw();
    }
}