using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BorderOnPause
{
    [StaticConstructorOnStartup]
    public class Core : Verse.Mod
    {
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

            /*
             * we don't need to create new textures if our setting have not changed
             * since last draw
             */
            if (AlmostMatch(Settings.BorderSize, _prevBorderSize) &&
                AlmostMatch(Settings.StartAlpha, _prevStartAlpha) &&
                AlmostMatch(Settings.EndAlpha, _prevEndAlpha) &&
                AlmostMatch(Settings.ColorG, _prevColorG) &&
                AlmostMatch(Settings.ColorR, _prevColorR) &&
                AlmostMatch(Settings.ColorB, _prevColorB) &&
                AlmostMatch(UI.screenHeight, _prevUiHeight) &&
                AlmostMatch(UI.screenWidth, _prevUiWidth)
               )
            {
                DrawBorders();
                return;
            }

            /*
             * "settings" values changed so we need to create new batch of textures
             */

            _borders = BorderBuilder.CreateBordersUsingSettings();

            /*
             * and we need updated check values from settings values
             */
            _prevBorderSize = Settings.BorderSize;
            _prevStartAlpha = Settings.StartAlpha;
            _prevEndAlpha = Settings.EndAlpha;
            _prevColorR = Settings.ColorR;
            _prevColorG = Settings.ColorG;
            _prevColorB = Settings.ColorB;
            _prevUiHeight = UI.screenHeight;
            _prevUiWidth = UI.screenWidth;

            DrawBorders();
        }

        private static void DrawBorders()
        {
            var allUi = new Rect(0, 0, UI.screenWidth, UI.screenHeight);
            GUI.BeginGroup(allUi);
            _borders.ForEach(pair => Widgets.DrawAtlas(pair.First, pair.Second, true));
            GUI.EndGroup();
        }

        /*
         * This is just "inline" ("copy-paste") of Widgets.DrawAtlas with uiScaling redefined to 0.
         * 
         * Scaling change our structure rect x and y coordinates which result in
         * either overlapping or gaps between textures.
         * You can live with those gaps...
         * But I really don't like them.
         */
        public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
        {
            rect.x = Mathf.Round(rect.x);
            rect.y = Mathf.Round(rect.y);
            rect.width = Mathf.Round(rect.width);
            rect.height = Mathf.Round(rect.height);

            /* redefining scaling */
            float uiScalingCeil = 0.0f;
                        
            Widgets.BeginGroup(rect);
            Rect drawRect;
            Rect uvRect;
            if (drawTop)
            {
                drawRect = new Rect(0.0f, 0.0f, uiScalingCeil, uiScalingCeil);
                uvRect = new Rect(0.0f, 0.0f, 0.25f, 0.25f);
                Widgets.DrawTexturePart(drawRect, uvRect, atlas);
                drawRect = new Rect(rect.width - uiScalingCeil, 0.0f, uiScalingCeil, uiScalingCeil);
                uvRect = new Rect(0.75f, 0.0f, 0.25f, 0.25f);
                Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            }

            drawRect = new Rect(0.0f, rect.height - uiScalingCeil, uiScalingCeil, uiScalingCeil);
            uvRect = new Rect(0.0f, 0.75f, 0.25f, 0.25f);
            Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            drawRect = new Rect(rect.width - uiScalingCeil, rect.height - uiScalingCeil, uiScalingCeil, uiScalingCeil);
            uvRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
            Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            drawRect = new Rect(uiScalingCeil, uiScalingCeil, rect.width - uiScalingCeil * 2f,
                rect.height - uiScalingCeil * 2f);
            if (!drawTop)
            {
                drawRect.height += uiScalingCeil;
                drawRect.y -= uiScalingCeil;
            }

            uvRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);
            Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            if (drawTop)
            {
                drawRect = new Rect(uiScalingCeil, 0.0f, rect.width - uiScalingCeil * 2f, uiScalingCeil);
                uvRect = new Rect(0.25f, 0.0f, 0.5f, 0.25f);
                Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            }

            drawRect = new Rect(uiScalingCeil, rect.height - uiScalingCeil, rect.width - uiScalingCeil * 2f,
                uiScalingCeil);
            uvRect = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
            Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            drawRect = new Rect(0.0f, uiScalingCeil, uiScalingCeil, rect.height - uiScalingCeil * 2f);
            if (!drawTop)
            {
                drawRect.height += uiScalingCeil;
                drawRect.y -= uiScalingCeil;
            }

            uvRect = new Rect(0.0f, 0.25f, 0.25f, 0.5f);
            Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            drawRect = new Rect(rect.width - uiScalingCeil, uiScalingCeil, uiScalingCeil,
                rect.height - uiScalingCeil * 2f);
            if (!drawTop)
            {
                drawRect.height += uiScalingCeil;
                drawRect.y -= uiScalingCeil;
            }

            uvRect = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
            Widgets.DrawTexturePart(drawRect, uvRect, atlas);
            Widgets.EndGroup();
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

            var borderSizeBuffer = ((int)Settings.BorderSize).ToString();
            //EN: "Set border size in pixels"
            listingStandard.Label("BOP_SetBorderSizeInPixels".Translate());
            listingStandard.IntEntry(ref _borderSize, ref borderSizeBuffer);
            Settings.BorderSize = _borderSize;
            
            //EN: "Gradient starting opacity (transparent -> solid)"
            listingStandard.Label("BOP_GradientStartingOpacity".Translate());
            Settings.StartAlpha = listingStandard.Slider(Settings.StartAlpha, 0f, 1.0f);
            
            //EN: "Gradient final opacity (transparent -> solid)"
            listingStandard.Label("BOP_GradientFinalOpacity".Translate());
            Settings.EndAlpha = listingStandard.Slider(Settings.EndAlpha, 0f, 1.0f);

            //EN:"Border color. (Red, Green, Blue)"
            listingStandard.Label("BOP_BorderColorRGB".Translate());
            Settings.ColorR = listingStandard.Slider(Settings.ColorR, 0f, 1.0f);
            Settings.ColorG = listingStandard.Slider(Settings.ColorG, 0f, 1.0f);
            Settings.ColorB = listingStandard.Slider(Settings.ColorB, 0f, 1.0f);

            //EN: "Reset to default"
            var buttonText = listingStandard.ButtonText("BOP_ResetToDefault".Translate());
            if (buttonText)
            {
                Settings.BorderSize = 25f;
                Settings.StartAlpha = 0.25f;
                Settings.EndAlpha = 0.25f;

                Settings.ColorR = 1.0f;
                Settings.ColorG = 0.0f;
                Settings.ColorB = 0.0f;
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