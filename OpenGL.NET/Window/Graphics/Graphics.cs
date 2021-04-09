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
    public static class Objects
    {
        public static void DrawBench()
        {
            //Поджопные доски
            GL.PushMatrix();
            GL.Translate(0, 0, 0);
            GL.Scale(2, 0.05, 0.2);
            Graphics.DrawCube(Color.FromArgb(66, 33, 9));
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 0, -6.0f);
            GL.Scale(2, 0.05, 0.2);
            Graphics.DrawCube(Color.FromArgb(66, 33, 9));
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 0, 6.0f);
            GL.Scale(2, 0.05, 0.2);
            Graphics.DrawCube(Color.FromArgb(66, 33, 9));
            GL.PopMatrix();

            //Ноги
            GL.PushMatrix();
            GL.Translate(-13, -5.5, 0);
            GL.Scale(0.15, 0.5, 0.8);
            Graphics.DrawCube(Color.FromArgb(33, 16, 4));
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(13, -5.5, 0);
            GL.Scale(0.15, 0.5, 0.8);
            Graphics.DrawCube(Color.FromArgb(33, 16, 4));
            GL.PopMatrix();

            //Спина
            GL.PushMatrix();
            GL.Translate(-13, 5, -10.5f);
            GL.Rotate(70.0f, 1, 0, 0);
            GL.Scale(0.15, 0.05, 1);
            Graphics.DrawCube(Color.FromArgb(33, 16, 4));
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(13, 5, -10.5f);
            GL.Rotate(70.0f, 1, 0, 0);
            GL.Scale(0.15, 0.05, 1);
            Graphics.DrawCube(Color.FromArgb(33, 16, 4));
            GL.PopMatrix();

            //Спинные доски
            GL.PushMatrix();
            GL.Translate(0, 2.7, -8.5f);
            GL.Rotate(70.0f, 1, 0, 0);
            GL.Scale(2, 0.05, 0.2);
            Graphics.DrawCube(Color.FromArgb(66, 33, 9));
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 7.3, -10.2f);
            GL.Rotate(70.0f, 1, 0, 0);
            GL.Scale(2, 0.05, 0.2);
            Graphics.DrawCube(Color.FromArgb(66, 33, 9));
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 12, -12.0f);
            GL.Rotate(70.0f, 1, 0, 0);
            GL.Scale(2, 0.05, 0.2);
            Graphics.DrawCube(Color.FromArgb(66, 33, 9));
            GL.PopMatrix();
        }
    }
    public static class Graphics
    {
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
        public static void DrawCube(Color color)
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(color);

            //front
            GL.Vertex3(-10.0, 10.0, 10.0);
            GL.Vertex3(-10.0, 10.0, -10.0);
            GL.Vertex3(-10.0, -10.0, -10.0);
            GL.Vertex3(-10.0, -10.0, 10.0);
            //back
            GL.Vertex3(10.0, 10.0, 10.0);
            GL.Vertex3(10.0, 10.0, -10.0);
            GL.Vertex3(10.0, -10.0, -10.0);
            GL.Vertex3(10.0, -10.0, 10.0);
            //top
            GL.Vertex3(10.0, -10.0, 10.0);
            GL.Vertex3(10.0, -10.0, -10.0);
            GL.Vertex3(-10.0, -10.0, -10.0);
            GL.Vertex3(-10.0, -10.0, 10.0);
            //bottom
            GL.Vertex3(10.0, 10.0, 10.0);
            GL.Vertex3(10.0, 10.0, -10.0);
            GL.Vertex3(-10.0, 10.0, -10.0);
            GL.Vertex3(-10.0, 10.0, 10.0);
            //right
            GL.Vertex3(10.0, 10.0, -10.0);
            GL.Vertex3(10.0, -10.0, -10.0);
            GL.Vertex3(-10.0, -10.0, -10.0);
            GL.Vertex3(-10.0, 10.0, -10.0);
            //left
            GL.Vertex3(10.0, 10.0, 10.0);
            GL.Vertex3(10.0, -10.0, 10.0);
            GL.Vertex3(-10.0, -10.0, 10.0);
            GL.Vertex3(-10.0, 10.0, 10.0);

            GL.End();
        }
    }
}
