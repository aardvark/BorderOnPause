using HarmonyLib;
using RimWorld;
using Verse;

namespace BorderOnPause.Patch
{
    [HarmonyPatch(typeof(MapInterface), "MapInterfaceOnGUI_BeforeMainTabs"), StaticConstructorOnStartup]
    internal static class RimWorld_MapInterface_MapInterfaceOnGUI_BeforeMainTabs
    {
        private static void Prefix()
        {
            // suppress border if we are in screenshot mode
            if (!Find.ScreenshotModeHandler.Active)
            {
                Core.InitDraw();
            }
        }
    }
}