using Harmony;
using Verse;

namespace BorderOnPause
{
    [StaticConstructorOnStartup]
    internal static class Mod
    {
        public const string Id = "BorderOnPause";
        public const string Name = "Border on pause";
        public const string Version = "0.0.1";

        static Mod()
        {
            HarmonyInstance.Create(Id).PatchAll();
            Log("Initialized");
        }

        public static void Log(string msg) => Verse.Log.Message(WithNameAndVersion(msg));
        private static string WithNameAndVersion(string msg) => $"[{Name} {Version}] {msg}";

        public class Exception : System.Exception
        {
            public Exception(string message) : base($"[{Name}: EXCEPTION] {message}")
            {
            }
        }
    }
}