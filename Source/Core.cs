using UnityEngine;
using Verse;

namespace BorderOnPause
{
    [StaticConstructorOnStartup]
    internal static class Core
    {
        private static readonly Texture2D BorderTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.0f, 0.0f, 0.25f));

        static float borderThickness = 25f;

        private static readonly Rect AllUI = new Rect(0, 0, UI.screenWidth, UI.screenHeight);
        private static readonly Rect LeftBorder = new Rect(0, 0, borderThickness, UI.screenHeight);

        private static readonly Rect TopBorder = new
            Rect(borderThickness, 0, UI.screenWidth - borderThickness, borderThickness);

        private static readonly Rect RightBorder =
            new Rect(UI.screenWidth - borderThickness, 0, borderThickness, UI.screenHeight);

        private static readonly Rect BottomBorder = new Rect(borderThickness, UI.screenHeight - borderThickness * 2,
            UI.screenWidth - borderThickness, borderThickness);


        public static void Draw()
        {
            if (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused) return;

            GUI.BeginGroup(AllUI);
            Widgets.DrawAtlas(LeftBorder, BorderTex);
            Widgets.DrawAtlas(TopBorder, BorderTex);
            Widgets.DrawAtlas(RightBorder, BorderTex);
            Widgets.DrawAtlas(BottomBorder, BorderTex);
            GUI.EndGroup();
        }
    }
}