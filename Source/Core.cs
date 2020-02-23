using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BorderOnPause
{
    public class Core : Verse.Mod
    {
        private static readonly BorderBuilder Builder = new BorderBuilder();
        private Settings _settings;

        private static float _prevBorderSize;
        private static float _prevStartAlpha;
        private static float _prevEndAlpha;
        private static float _prevColorR;
        private static float _prevColorG;
        private static float _prevColorB;

        private static float _prevUiWidth;
        private static float _prevUiHeight;

        private static List<Pair<Rect, Texture2D>> _borders;

        private static bool AlmostMatch(float a, float b)
        {
            return Math.Abs(a - b) < 0.000001;
        }

        public static void InitDraw()
        {
            if (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused) return;

            if (AlmostMatch(Settings.BorderSize, _prevBorderSize) &&
                AlmostMatch(Settings.StartAlpha, _prevStartAlpha) &&
                AlmostMatch(Settings.EndAlpha, _prevEndAlpha) &&
                AlmostMatch(Settings.Color_G, _prevColorG) &&
                AlmostMatch(Settings.Color_R, _prevColorR) &&
                AlmostMatch(Settings.Color_B, _prevColorB) &&
                AlmostMatch(UI.screenHeight, _prevUiHeight) &&
                AlmostMatch(UI.screenWidth, _prevUiWidth)
            )
            {
                DrawBorders();
                return;
            }

            _borders = BorderBuilder.CreateBordersUsingSettings();
            
            _prevBorderSize = Settings.BorderSize;
            _prevStartAlpha = Settings.StartAlpha;
            _prevEndAlpha = Settings.EndAlpha;
            _prevColorR = Settings.Color_R;
            _prevColorG = Settings.Color_G;
            _prevColorB = Settings.Color_B;
            _prevUiHeight = UI.screenHeight;
            _prevUiWidth = UI.screenWidth;

            DrawBorders();
        }

        private static void DrawBorders()
        {
            var allUi = new Rect(0, 0, UI.screenWidth, UI.screenHeight);
            GUI.BeginGroup(allUi);
            _borders.ForEach(pair => Widgets.DrawAtlas(pair.First, pair.Second));
            GUI.EndGroup();
        }

        public Core(ModContentPack content) : base(content)
        {
            _settings = GetSettings<Settings>();
        }

        private static int _borderSize = 25;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            var borderSizeBuffer = ((int) Settings.BorderSize).ToString();
            listingStandard.Label("Set border size in pixels");
            listingStandard.IntEntry(ref _borderSize, ref borderSizeBuffer);
            Settings.BorderSize = _borderSize;

            listingStandard.Label("Gradient starting opacity (transparent -> solid)");
            Settings.StartAlpha = listingStandard.Slider(Settings.StartAlpha, 0f, 1.0f);

            listingStandard.Label("Gradient final opacity (transparent -> solid)");
            Settings.EndAlpha = listingStandard.Slider(Settings.EndAlpha, 0f, 1.0f);

            listingStandard.Label("Border color. (Red, Green, Blue)");
            Settings.Color_R = listingStandard.Slider(Settings.Color_R, 0f, 1.0f);
            Settings.Color_G = listingStandard.Slider(Settings.Color_G, 0f, 1.0f);
            Settings.Color_B = listingStandard.Slider(Settings.Color_B, 0f, 1.0f);

            var buttonText = listingStandard.ButtonText("Reset to default");
            if (buttonText)
            {
                Settings.BorderSize = 25f;
                Settings.StartAlpha = 0.25f;
                Settings.EndAlpha = 0.25f;

                Settings.Color_R = 1.0f;
                Settings.Color_G = 0.0f;
                Settings.Color_B = 0.0f;
            }

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return Mod.Name;
        }
    }
}