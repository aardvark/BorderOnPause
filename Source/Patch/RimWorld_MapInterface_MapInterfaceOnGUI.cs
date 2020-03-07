using HarmonyLib;
using RimWorld;
using Verse;

namespace BorderOnPause.Patch
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs")]
    internal static class RimWorld_MapInterface_MapInterfaceOnGUI_BeforeMainTabs
    {
        private static void Prefix()
        {
            if (!Find.ScreenshotModeHandler.Active)
            {
                Core.InitDraw();
            }
        }
    }
}