using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BorderOnPause
{
    public class Core : Verse.Mod
    {
        private static readonly BorderBuilder Builder = new BorderBuilder();
        private static readonly Rect AllUi = new Rect(0, 0, UI.screenWidth, UI.screenHeight);
        private Settings _settings;

        private static float _prevBorderSize;
        private static float _prevStartAlpha;
        private static float _prevEndAlpha;
        private static float _prevColorR;
        private static float _prevColorG;
        private static float _prevColorB;

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
                AlmostMatch(Settings.Color_B, _prevColorB)
            )
            {
                GUI.BeginGroup(AllUi);
                _borders.ForEach(pair => Widgets.DrawAtlas(pair.First, pair.Second));
                GUI.EndGroup();
                return;
            }

            GUI.BeginGroup(AllUi);

            var borderSize = Settings.BorderSize;
            var startAlpha = Settings.StartAlpha;
            var endAlpha = Settings.EndAlpha;
            var colorR = Settings.Color_R;
            var colorG = Settings.Color_G;
            var colorB = Settings.Color_B;
            _borders = Builder.CreateBorders(borderSize, startAlpha, endAlpha,
                colorR, colorG, colorB
            );
            _prevBorderSize = borderSize;
            _prevStartAlpha = startAlpha;
            _prevEndAlpha = endAlpha;
            _prevColorG = colorG;
            _prevColorR = colorR;
            _prevColorB = colorB;

            GUI.BeginGroup(AllUi);
            _borders.ForEach(pair => Widgets.DrawAtlas(pair.First, pair.Second));
            GUI.EndGroup();
        }

        public Core(ModContentPack content) : base(content)
        {
            this._settings = GetSettings<Settings>();
        }

        private static int _borderSize = 25;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
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

            bool buttonText = listingStandard.ButtonText("Reset to default");
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