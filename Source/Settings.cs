using Verse;

namespace BorderOnPause
{
    [StaticConstructorOnStartup]
    public class Settings : ModSettings
    {
        public static float BorderSize = 25f;
        public static float StartAlpha = 0.5f;
        public static float EndAlpha = 0.05f;
        
        public static float ColorR = 1.0f;
        public static float ColorG = 0.0f;
        public static float ColorB = 0.0f;
        
        
        public override void ExposeData()
        {
            Scribe_Values.Look(ref BorderSize, "borderSize");
            Scribe_Values.Look(ref StartAlpha, "startAlpha");
            Scribe_Values.Look(ref EndAlpha, "endAlpha");
            
            Scribe_Values.Look(ref ColorR, "color_R");
            Scribe_Values.Look(ref ColorG, "color_G");
            Scribe_Values.Look(ref ColorB, "color_B");
            
            
            base.ExposeData();
        }
    }
}