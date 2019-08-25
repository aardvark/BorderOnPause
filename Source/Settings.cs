using Verse;

namespace BorderOnPause
{
    public class Settings : ModSettings
    {
        public static float BorderSize = 25f;
        public static float StartAlpha = 0.5f;
        public static float EndAlpha = 0.05f;
        
        public static float Color_R = 1.0f;
        public static float Color_G = 0.0f;
        public static float Color_B = 0.0f;
        
        
        public override void ExposeData()
        {
            Scribe_Values.Look(ref BorderSize, "borderSize");
            Scribe_Values.Look(ref StartAlpha, "startAlpha");
            Scribe_Values.Look(ref EndAlpha, "endAlpha");
            
            Scribe_Values.Look(ref Color_R, "color_R");
            Scribe_Values.Look(ref Color_G, "color_G");
            Scribe_Values.Look(ref Color_B, "color_B");
            
            
            base.ExposeData();
        }
    }
}