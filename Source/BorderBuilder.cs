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
            TopLeft,
            BottomLeft,

            Right,
            TopRight,
            BottomRight,

            Top,
            Bottom
        }
        
        private const float BottomMenuSize = 35f;
        
        private static Texture2D GradientTexture(float startingTransparency, float finalTransparency, GradientType type,
            float r, float g, float b)
        {
            if (!UnityData.IsInMainThread)
            {
                Log.Error("Tried to create a texture from a different thread.");
                return null;
            }

            var from = new Color(r, g, b, startingTransparency);
            var to = new Color(r, g, b, finalTransparency);

            Texture2D texture2D;
            switch (type)
            {
                case GradientType.Left:
                    texture2D = LeftGradient(ref from, ref to);
                    break;
                case GradientType.TopLeft:
                    texture2D = TopLeftGradient(ref from, ref to);
                    break;
                case GradientType.BottomLeft:
                    texture2D = BottomLeftGradient(ref from, ref to);
                    break;
                case GradientType.Right:
                    texture2D = RightGradient(ref from, ref to);
                    break;
                case GradientType.TopRight:
                    texture2D = TopRightGradient(ref from, ref to);
                    break;
                case GradientType.BottomRight:
                    texture2D = BottomRightGradient(ref from, ref to);
                    break;
                case GradientType.Top:
                    texture2D = TopGradient(ref from, ref to);
                    break;
                case GradientType.Bottom:
                    texture2D = BottomGradient(ref from, ref to);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.Apply();
            return texture2D;
        }

        private static Texture2D BottomGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(1, 2) {name = $"GradientBTTColorTex-{from}"};
            texture.SetPixel(0, 0, from);
            texture.SetPixel(0, 1, to);
            return texture;
        }

        private static Texture2D TopGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(1, 2) {name = $"GradientTTBColorTex-{from}"};
            texture.SetPixel(0, 0, to);
            texture.SetPixel(0, 1, from);
            return texture;
        }

        private static Texture2D BottomRightGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(2, 2) {name = $"GradientBottomRightTex-{from}"};
            texture.SetPixel(0, 0, from);
            texture.SetPixel(0, 1, to);
            texture.SetPixel(1, 1, from);
            texture.SetPixel(1, 0, from);
            return texture;
        }

        private static Texture2D RightGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(2, 1) {name = $"GradientRTLColorTex-{from}"};
            texture.SetPixel(0, 0, to);
            texture.SetPixel(1, 0, from);
            return texture;
        }

        private static Texture2D BottomLeftGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(2, 2) {name = $"GradientBottomLeftTex-{from}"};
            texture.SetPixel(0, 0, from);
            texture.SetPixel(0, 1, from);
            texture.SetPixel(1, 1, to);
            texture.SetPixel(1, 0, from);
            return texture;
        }

        private static Texture2D TopRightGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(2, 2) {name = $"GradientTopRightTex-{from}"};
            texture.SetPixel(0, 0, to);
            texture.SetPixel(0, 1, from);
            texture.SetPixel(1, 1, from);
            texture.SetPixel(1, 0, from);
            return texture;
        }

        private static Texture2D LeftGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(2, 1) {name = $"GradientLTRColorTex-{from}"};
            texture.SetPixel(0, 0, from);
            texture.SetPixel(1, 0, to);
            return texture;
        }

        private static Texture2D TopLeftGradient(ref Color from, ref Color to)
        {
            var texture = new Texture2D(2, 2) {name = $"GradientTopLeftTex-{from}"};
            texture.SetPixel(0, 0, from);
            texture.SetPixel(0, 1, from);
            texture.SetPixel(1, 1, from);
            texture.SetPixel(1, 0, to);
            return texture;
        }


        public static List<Pair<Rect, Texture2D>> CreateBordersUsingSettings()
        {
            return CreateBorders(Settings.BorderSize, Settings.StartAlpha, Settings.EndAlpha,
                Settings.Color_R, Settings.Color_G, Settings.Color_B);
        }

        private static List<Pair<Rect, Texture2D>> CreateBorders(float borderSize, float start, float stop, float r, float g,
            float b)
        {
            var topLeftGradient = GradientTexture(start, stop, GradientType.TopLeft, r, g, b);
            var leftGradient = GradientTexture(start, stop, GradientType.Left, r, g, b);
            var topRightGradient = GradientTexture(start, stop, GradientType.TopRight, r, g, b);
            var rightGradient = GradientTexture(start, stop, GradientType.Right, r, g, b);
            var topGradient = GradientTexture(start, stop, GradientType.Top, r, g, b);
            var bottomLeft = GradientTexture(start, stop, GradientType.BottomLeft, r, g, b);
            var bottomRight = GradientTexture(start, stop, GradientType.BottomRight, r, g, b);
            var bottomGradient = GradientTexture(start, stop, GradientType.Bottom, r, g, b);


            var leftTopCorner = new Rect(0, 0, borderSize, borderSize);
            var leftBorder = new Rect(0, borderSize, borderSize, UI.screenHeight - BottomMenuSize - borderSize - borderSize);

            var topBorder = new Rect(borderSize, 0, UI.screenWidth - borderSize - borderSize, borderSize);
            var rightTopCorner = new Rect(UI.screenWidth - borderSize, topBorder.y, borderSize, borderSize);

            var rightBorder = new Rect(UI.screenWidth - borderSize, borderSize, borderSize,
                UI.screenHeight - BottomMenuSize - borderSize - borderSize);


            var bottomBorder = new Rect(borderSize, UI.screenHeight - borderSize - BottomMenuSize,
                UI.screenWidth - borderSize - borderSize, borderSize);

            var rightBottomCorner = new Rect(rightBorder.x, bottomBorder.y, borderSize, borderSize);

            var leftBottomCorner = new Rect(0, bottomBorder.y, borderSize, borderSize);

            var pairs = new List<Pair<Rect, Texture2D>>
            {
                new Pair<Rect, Texture2D>(leftBorder, leftGradient),
                new Pair<Rect, Texture2D>(rightBorder, rightGradient),
                new Pair<Rect, Texture2D>(topBorder, topGradient),
                new Pair<Rect, Texture2D>(bottomBorder, bottomGradient),
                new Pair<Rect, Texture2D>(leftTopCorner, topLeftGradient),
                new Pair<Rect, Texture2D>(rightTopCorner, topRightGradient),
                new Pair<Rect, Texture2D>(rightBottomCorner, bottomRight),
                new Pair<Rect, Texture2D>(leftBottomCorner, bottomLeft)
            };

            return pairs;
        }
    }
}