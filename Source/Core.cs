using System;
using System.Globalization;
using UnityEngine;
using Verse;

namespace BorderOnPause
{
    public class Core : Verse.Mod
    {
        private static readonly BorderBuilder Builder = new BorderBuilder();
        private static readonly Rect AllUi = new Rect(0, 0, UI.screenWidth, UI.screenHeight);
        private Settings settings;

        public static void InitDraw()
        {
            if (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused) return;
            GUI.BeginGroup(AllUi);
            var borders = Builder.CreateBorders(Settings.BorderSize, Settings.StartAlpha, Settings.EndAlpha,
                Settings.Color_R, Settings.Color_G, Settings.Color_B
            );
            borders.ForEach(pair => Widgets.DrawAtlas(pair.First, pair.Second));
            GUI.EndGroup();
        }

        public Core(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Settings>();
        }

        private static int _borderSize = 25;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            var borderSizeBuffer = ((int) Settings.BorderSize).ToString();
            listingStandard.Label("Set border size in pixels");
            listingStandard.IntEntry(ref _borderSize, ref borderSizeBuffer);
            Settings.BorderSize = Core._borderSize;

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