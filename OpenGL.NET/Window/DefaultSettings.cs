using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace OpenGL
{
    static class Settings
    {
        public static class Window
        {
            public const int WindowWidth = 800;
            public const int WindowHeight = 600;
            public const string WindowTitle = "OpenGL Application";
            public const int WindowUpdateFrequency = 60;
            public const int WindowRenderFrequency = 60;
        }
        public static class Camera
        {
            public static Vector3 InitialPosition = new Vector3(0, 10.0f, 45.0f);
            public static Vector3 InitialOrientation = new Vector3(MathHelper.Pi, -MathHelper.PiOver4 / 4, 0f);
            public const float MoveSpeed = 0.4f;
            public const float MouseSensitivity = 0.0015f;
        }
    }
}
