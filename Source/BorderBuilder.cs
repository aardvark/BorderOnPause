using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BorderOnPause
{
    public class BorderBuilder
    {
        private enum GradientType
        {
            Left,
            Right,
            Top,
            Bottom
        }

        private static Texture2D GradientTexture(float start, float stop, GradientType type, float r, float g, float b)
        {
            if (!UnityData.IsInMainThread)
            {
                Log.Error("Tried to create a texture from a different thread.");
                return null;
            }

            var from = new Color(r, g, b, start);
            var to = new Color(r, g, b, stop);

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

        private const float BottomMenuSize = 35f;

        public List<Pair<Rect, Texture2D>> CreateBorders(float borderSize, float start, float stop, float r, float g,
            float b)
        {
            var leftGradient = GradientTexture(start, stop, GradientType.Left, r, g, b);
            var rightGradient = GradientTexture(start, stop, GradientType.Right, r, g, b);
            var topGradient = GradientTexture(start, stop, GradientType.Top, r, g, b);
            var bottomGradient = GradientTexture(start, stop, GradientType.Bottom, r, g, b);


            var leftBorder = new Rect(0, 0, borderSize, UI.screenHeight - BottomMenuSize);
            var topBorder = new Rect(0, 0, UI.screenWidth, borderSize);

            var rightBorder = new Rect(UI.screenWidth - borderSize, 0, borderSize,
                UI.screenHeight - BottomMenuSize);

            var bottomBorder = new Rect(0, UI.screenHeight - borderSize - BottomMenuSize,
                UI.screenWidth, borderSize);

            var pairs = new List<Pair<Rect, Texture2D>>();
            pairs.Add(new Pair<Rect, Texture2D>(leftBorder, leftGradient));
            pairs.Add(new Pair<Rect, Texture2D>(rightBorder, rightGradient));
            pairs.Add(new Pair<Rect, Texture2D>(topBorder, topGradient));
            pairs.Add(new Pair<Rect, Texture2D>(bottomBorder, bottomGradient));

            return pairs;
        }
    }
}