using System.Reflection;
using HarmonyLib;
using Verse;

namespace BorderOnPause
{
    [StaticConstructorOnStartup]
    internal static class Mod
    {
        public const string Id = "BorderOnPause";
        public const string Name = "More visible pause";
        public const string Version = "2.0.0.16";

        static Mod()
        {
            var harmony = new Harmony(Id);
            harmony.PatchAll();
            Log("Initialized");
        }

        public static void Log(string msg) => Verse.Log.Message(WithNameAndVersion(msg));
        private static string WithNameAndVersion(string msg) => $"[{Id} {Version}] {msg}";
    }
}