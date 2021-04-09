using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using OpenGL.Window.Graphics.Properties;

namespace OpenGL.Window.Graphics
{
    public enum ColorEnum
    {
        LightBrown,
        DarkBrown
    }
    public static class Objects
    {
        public static void DrawBench()
        {
            Graphics.DrawParallelepiped((-25f, 0.7f, 2f), (25f, -0.7f, -2f), Graphics.Colors[ColorEnum.LightBrown], Color.Black);
            Graphics.DrawParallelepiped((-25f, 0.7f, -4f), (25f, -0.7f, -8f), Graphics.Colors[ColorEnum.LightBrown], Color.Black);
            Graphics.DrawParallelepiped((-25f, 0.7f, 4f), (25f, -0.7f, 8f), Graphics.Colors[ColorEnum.LightBrown], Color.Black);
            Graphics.DrawParallelepiped((-20f, -0.7f, 8f), (-15f, -12f, -8f), Graphics.Colors[ColorEnum.DarkBrown], Color.Black);
            Graphics.DrawParallelepiped((15f, -0.7f, 8f), (20f, -12f, -8f), Graphics.Colors[ColorEnum.DarkBrown], Color.Black);
            GL.PushMatrix();
            GL.Translate(0, 9f, -9.8f);
            GL.Rotate(70f, 1, 0, 0);
            Graphics.DrawParallelepiped((-25f, 0.7f, 2f), (25f, -0.7f, -2f), Graphics.Colors[ColorEnum.LightBrown], Color.Black);
            Graphics.DrawParallelepiped((-25f, 0.7f, -4f), (25f, -0.7f, -8f), Graphics.Colors[ColorEnum.LightBrown], Color.Black);
            Graphics.DrawParallelepiped((-25f, 0.7f, 4f), (25f, -0.7f, 8f), Graphics.Colors[ColorEnum.LightBrown], Color.Black);
            Graphics.DrawParallelepiped((-20f, -0.7f, -10f), (-15f, -2.5f, 12.2f), Graphics.Colors[ColorEnum.DarkBrown], Color.Black);
            Graphics.DrawParallelepiped((15f, -0.7f, -10f), (20f, -2.6f, 12.2f), Graphics.Colors[ColorEnum.DarkBrown], Color.Black);
            GL.PopMatrix();
        }
    }
    public static class Graphics
    {
        public static Dictionary<ColorEnum, Color> Colors { get; } = new Dictionary<ColorEnum, Color>()
        {
            { ColorEnum.LightBrown, Color.FromArgb(66, 33, 9) },
            { ColorEnum.DarkBrown, Color.FromArgb(33, 16, 4) }
        };
        public static void DrawParallelepiped(FloatPoint3 firstPoint, FloatPoint3 secondPoint, Color? fillColor = null, Color? borderColor = null)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(fillColor == null ? Color.White : (Color)fillColor);
            //front
            GL.Vertex3(firstPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, firstPoint.Z);
            //back
            GL.Vertex3(secondPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(firstPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, secondPoint.Z);
            //top
            GL.Vertex3(firstPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, firstPoint.Z);
            //bottom
            GL.Vertex3(firstPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, firstPoint.Z);
            //right
            GL.Vertex3(secondPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, secondPoint.Z);
            //left
            GL.Vertex3(firstPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, firstPoint.Z);

            GL.End();

            GL.Color3(borderColor == null ? Color.Black : (Color)borderColor);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(firstPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, firstPoint.Z);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(firstPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, secondPoint.Z);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(firstPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, firstPoint.Y, secondPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(firstPoint.X, secondPoint.Y, secondPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, firstPoint.Z);
            GL.Vertex3(secondPoint.X, secondPoint.Y, secondPoint.Z);
            GL.End();
        }
    }
}
