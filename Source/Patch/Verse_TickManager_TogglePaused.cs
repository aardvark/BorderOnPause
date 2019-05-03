using Harmony;
using Verse;

namespace BorderOnPause.Patch
{
    [HarmonyPatch(typeof(TickManager), "TogglePaused")]
    internal static class Verse_TickManager_TogglePaused
    {
        private static void Postfix()
        {
            var tickManagerCurTimeSpeed = Find.TickManager.CurTimeSpeed;
            Mod.Log("Speed is: " + tickManagerCurTimeSpeed);
        }
    }
}