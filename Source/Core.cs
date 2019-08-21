using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BorderOnPause
{
    [StaticConstructorOnStartup]
    internal static class Core
    {
        /* Gradient curve steps for smothing edges*/
        private static readonly List<float> Steps = new List<float> {0.25f, 0.20f, 0.15f, 0.10f, 0.05f};

        enum GradientType
        {
            Left,
            Right,
            Top,
            Bottom
        }

        private static Texture2D GradientTexture(float start, float stop, GradientType type)
        {
            if (!UnityData.IsInMainThread)
            {
                Log.Error("Tried to create a texture from a different thread.");
                return null;
            }

            var from = new Color(1f, 0.0f, 0.0f, start);
            var to = new Color(1f, 0.0f, 0.0f, stop);

            Texture2D LeftGradient()
            {
                var texture = new Texture2D(2, 1) {name = "GradientLTRColorTex-" + from};
                texture.SetPixel(0, 0, from);
                texture.SetPixel(1, 0, to);
                return texture;
            }

            Texture2D RightGradient()
            {
                var texture = new Texture2D(2, 1) {name = "GradientRTLColorTex-" + from};
                texture.SetPixel(0, 0, to);
                texture.SetPixel(1, 0, from);
                return texture;
            }

            Texture2D TopGradient()
            {
                var texture = new Texture2D(1, 2) {name = "GradientTTBColorTex-" + from};
                texture.SetPixel(0, 0, to);
                texture.SetPixel(0, 1, from);
                return texture;
            }

            Texture2D BottomGradient()
            {
                var texture = new Texture2D(1, 2) {name = "GradientBTTColorTex-" + from};
                texture.SetPixel(0, 0, from);
                texture.SetPixel(0, 1, to);
                return texture;
            }

            Texture2D texture2D;
            switch (type)
            {
                case GradientType.Left:
                    texture2D = LeftGradient();
                    break;
                case GradientType.Right:
                    texture2D = RightGradient();
                    break;
                case GradientType.Top:
                    texture2D = TopGradient();
                    break;
                case GradientType.Bottom:
                    texture2D = BottomGradient();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.Apply();
            return texture2D;
        }

        private static readonly Texture2D LeftGradient = GradientTexture(0.50f, 0.05f, GradientType.Left);
        private static readonly Texture2D RightGradient = GradientTexture(0.50f, 0.05f, GradientType.Right);
        private static readonly Texture2D TopGradient = GradientTexture(0.50f, 0.05f, GradientType.Top);
        private static readonly Texture2D BottomGradient = GradientTexture(0.50f, 0.05f, GradientType.Bottom);

        private const float BorderSize = 25f;
        private const float BottomMenuSize = 35f;

        private static readonly Rect AllUi = new Rect(0, 0, UI.screenWidth, UI.screenHeight);

        private static readonly Rect LeftBorder =
            new Rect(0, 0, BorderSize, UI.screenHeight - BottomMenuSize);

        private static readonly Rect TopBorder = new
            Rect(0, 0, UI.screenWidth, BorderSize);

        private static readonly Rect RightBorder =
            new Rect(UI.screenWidth - BorderSize, 0, BorderSize,
                UI.screenHeight - BottomMenuSize);

        private static readonly Rect BottomBorder = new Rect(0, UI.screenHeight - BorderSize - BottomMenuSize,
            UI.screenWidth, BorderSize);

        public static void Draw()
        {
            if (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused) return;

            GUI.BeginGroup(AllUi);
            Widgets.DrawAtlas(LeftBorder, LeftGradient);
            Widgets.DrawAtlas(TopBorder, TopGradient);
            Widgets.DrawAtlas(RightBorder, RightGradient);
            Widgets.DrawAtlas(BottomBorder, BottomGradient);
            GUI.EndGroup();
        }
    }
}