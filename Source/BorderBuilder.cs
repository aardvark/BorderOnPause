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

        private static Texture2D GradientTexture(TextureStruct textureProto, GradientType type)
        {
            if (!UnityData.IsInMainThread)
            {
                Log.Error("Tried to create a texture from a different thread.");
                return null;
            }

            var from = textureProto.ColorFrom();
            var to = textureProto.ColorTo();

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


        private struct TextureStruct
        {
            private readonly float _startTransparency;
            private readonly float _finalTransparency;
            private readonly float _r;
            private readonly float _g;
            private readonly float _b;

            public TextureStruct(float start, float final, float r, float g, float b)
            {
                _startTransparency = start;
                _finalTransparency = final;
                _r = r;
                _g = g;
                _b = b;
            }

            public Color ColorFrom()
            {
                return new Color(_r, _g, _b, _startTransparency);
            }

            public Color ColorTo()
            {
                return new Color(_r, _g, _b, _finalTransparency);
            }
        }

        public static List<Pair<Rect, Texture2D>> CreateBordersUsingSettings()
        {
            var textureStruct = new TextureStruct(Settings.StartAlpha, Settings.EndAlpha, Settings.Color_R, Settings.Color_G,
                Settings.Color_B);
            var bordersUsingSettings = CreateBorders(Settings.BorderSize,
                textureStruct);
            return bordersUsingSettings;
        }

        private static List<Pair<Rect, Texture2D>> CreateBorders(float borderSize, TextureStruct textureStruct)
        {
            var topLeftGradient = GradientTexture(textureStruct, GradientType.TopLeft);
            var leftGradient = GradientTexture(textureStruct, GradientType.Left);
            var topRightGradient = GradientTexture(textureStruct, GradientType.TopRight);
            var rightGradient = GradientTexture(textureStruct, GradientType.Right);
            var topGradient = GradientTexture(textureStruct, GradientType.Top);
            var bottomLeft = GradientTexture(textureStruct, GradientType.BottomLeft);
            var bottomRight = GradientTexture(textureStruct, GradientType.BottomRight);
            var bottomGradient = GradientTexture(textureStruct, GradientType.Bottom);


            var leftTopCorner = new Rect(0, 0, borderSize, borderSize);
            var leftBorder = new Rect(0, borderSize, borderSize,
                UI.screenHeight - BottomMenuSize - borderSize - borderSize);

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