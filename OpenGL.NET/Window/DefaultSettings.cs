using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;

namespace OpenGL
{
    static class Settings
    {
        public static class Window
        {
            public const int Width = 800;
            public const int Height = 800;
            public const string Title = "OpenGL Application";
            public const int UpdateFrequency = 60;
            public const int RenderFrequency = 60;
        }
        public static class Drawing
        {
            public static Color FillColor { get; } = Color.White;
            public static Color BorderColor { get; } = Color.Transparent;
            public static Color NormalColor { get; } = Color.AliceBlue;
            public static Color BackgroundColor { get; } = Color.Black;
        }
        public static class Camera
        {
            public static Vector3 InitialPosition { get; } = new Vector3(0, 10.0f, 45.0f);
            public static Vector3 InitialOrientation { get; } = new Vector3(MathHelper.Pi, -MathHelper.PiOver4 / 4, 0f);
            public const float Fov = 70f;
            public const float MinFov = 30f;
            public const float MaxFov = 120f;
            public const float MoveSpeed = 0.4f;
            public const float MoveSpeedAcceleration = 2.5f;
            public const float MoveSpeedAccelerated = MoveSpeed * MoveSpeedAcceleration;
            public const float MouseSensitivity = 0.0015f;
        }
    }
}
