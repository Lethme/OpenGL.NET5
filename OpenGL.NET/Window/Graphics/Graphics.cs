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
        private static OpenGLWindow Window { get; set; }
        public static Dictionary<ColorEnum, Color> Colors { get; } = new Dictionary<ColorEnum, Color>()
        {
            { ColorEnum.LightBrown, Color.FromArgb(66, 33, 9) },
            { ColorEnum.DarkBrown, Color.FromArgb(33, 16, 4) }
        };
        public static void Initialize(NativeWindow window)
        {
            if (window == null) throw new ArgumentNullException($"{nameof(window)} can't be null reference!");
            Window = (OpenGLWindow)window;
        }
        public static void CreateCoordinatesSystem(Action action)
        {
            GL.PushMatrix();
            action.Invoke();
            GL.PopMatrix();
        }
        public static void DrawCoordinatesSystem(float vectorLength = 10f)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, vectorLength, 0);
            GL.Color4(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(vectorLength, 0, 0);
            GL.Color4(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, vectorLength);
            GL.End();
        }
        public static void DrawSphere(FloatPoint3 center, float radius, Color? fillColor = null, Color? normalColor = null)
        {
            int nx = 64, ny = 64;
            int ix, iy;
            double x, y, z, sy, cy, sy1, cy1, sx, cx, piy, pix, ay, ay1, ax, ty, dnx, dny, diy;
            dnx = 1.0 / (double)nx;
            dny = 1.0 / (double)ny;
            // Рисуем полигональную модель сферы, формируем нормали и задаем коодинаты текстуры
            // Каждый полигон - это трапеция. Трапеции верхнего и нижнего слоев вырождаются в треугольники
            GL.Begin(PrimitiveType.QuadStrip);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            piy = Math.PI * dny;
            pix = Math.PI * dnx;
            for (iy = 0; iy < ny; iy++)
            {
                diy = (double)iy;
                ay = diy * piy;
                sy = Math.Sin(ay);
                cy = Math.Cos(ay);
                ty = diy * dny;
                ay1 = ay + piy;
                sy1 = Math.Sin(ay1);
                cy1 = Math.Cos(ay1);
                for (ix = 0; ix <= nx; ix++)
                {
                    ax = 2.0 * ix * pix;
                    sx = Math.Sin(ax);
                    cx = Math.Cos(ax);
                    x = center.X + radius * sy * cx;
                    y = center.Y + radius * sy * sx;
                    z = center.Z + radius * cy;
                    // Координаты нормали в текущей вершине
                    GL.Normal3(x, y, z); // Нормаль направлена от центра
                    GL.Vertex3(x, y, z);
                    x = center.X + radius * sy1 * cx;
                    y = center.Y + radius * sy1 * sx;
                    z = center.Z + radius * cy1;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                }
            }
            GL.End();
            if (Window.ShowNormals)
            {
                double rv = 1.15 * radius;
                // Толщина линии, отображающей нормаль
                GL.LineWidth(1);
                GL.Color4(normalColor == null ? Settings.Drawing.NormalColor : (Color)normalColor);
                GL.Begin(PrimitiveType.Lines);
                piy = Math.PI * dny;
                pix = Math.PI * dnx;
                for (iy = 0; iy < ny; iy++)
                {
                    diy = (double)iy;
                    ay = diy * piy;
                    sy = Math.Sin(ay);
                    cy = Math.Cos(ay);
                    for (ix = 0; ix <= nx; ix++)
                    {
                        ax = 2.0 * ix * pix;
                        sx = Math.Sin(ax);
                        cx = Math.Cos(ax);
                        x = center.X + radius * sy * cx;
                        y = center.Y + radius * sy * sx;
                        z = center.Z + radius * cy;
                        GL.Vertex3(x, y, z);
                        x = center.X + rv * sy * cx;
                        y = center.Y + rv * sy * sx;
                        z = center.Z + rv * cy;
                        GL.Vertex3(x, y, z);
                    }
                }
                GL.End();
                GL.LineWidth(1);
                GL.Color4(Color.LightGray);
            }
        }
        public static void DrawCone(FloatPoint3 center, float radius, float height, Color? fillColor = null, Color? borderColor = null)
        {
            var segments = 360;
            var points = new float[segments * 2];
            var count = 0;
            for (var angle = 0f; angle < 360; angle += segments / 360f)
            {
                points[count++] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * radius);
                points[count++] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * radius);
            }

            DrawCylinderBase(center - (0f, height / 2, 0f), points, fillColor, borderColor);

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            GL.Vertex3(center.X, center.Y + height / 2, center.Z);
            for (var i = 0; i < points.Length / 2 - 1; i++)
            {
                GL.Vertex3(points[i * 2], center.Y - height / 2, points[i * 2 + 1]);
            }
            GL.Vertex3(points[0], center.Y - height / 2, points[1]);
            GL.End();
        }
        public static void DrawCone(FloatPoint3 center, float horizontalRadius, float verticalRadius, float height, Color? fillColor = null, Color? borderColor = null)
        {
            var segments = 360;
            var points = new float[segments * 2];
            var count = 0;
            for (var angle = 0f; angle < 360; angle += segments / 360f)
            {
                points[count++] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * horizontalRadius);
                points[count++] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * verticalRadius);
            }

            DrawCylinderBase(center - (0f, height / 2, 0f), points, fillColor, borderColor);

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            GL.Vertex3(center.X, center.Y + height / 2, center.Z);
            for (var i = 0; i < points.Length / 2 - 1; i++)
            {
                GL.Vertex3(points[i * 2], center.Y - height / 2, points[i * 2 + 1]);
            }
            GL.Vertex3(points[0], center.Y - height / 2, points[1]);
            GL.End();
        }
        public static void DrawCylinder(FloatPoint3 center, float radius, float height, Color? fillColor = null, Color? borderColor = null)
        {
            var segments = 360;
            var points = new float[segments * 2];
            var count = 0;
            for (var angle = 0f; angle < 360; angle += segments / 360f)
            {
                points[count++] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * radius);
                points[count++] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * radius);
            }

            DrawCylinderBase(center - (0f, height / 2, 0f), points, fillColor, borderColor);
            DrawCylinderBase(center + (0f, height / 2, 0f), points, fillColor, borderColor);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            for (var i = 0; i < points.Length / 2 - 1; i++)
            {
                GL.Vertex3(points[i * 2], center.Y + height / 2, points[i * 2 + 1]);
                GL.Vertex3(points[i * 2], center.Y - height / 2, points[i * 2 + 1]);
                GL.Vertex3(points[i * 2 + 2], center.Y - height / 2, points[i * 2 + 3]);
                GL.Vertex3(points[i * 2 + 2], center.Y + height / 2, points[i * 2 + 3]);
            }
            GL.Vertex3(points[0], center.Y + height / 2, points[1]);
            GL.Vertex3(points[0], center.Y - height / 2, points[1]);
            GL.Vertex3(points[points.Length - 2], center.Y - height / 2, points[points.Length - 1]);
            GL.Vertex3(points[points.Length - 2], center.Y + height / 2, points[points.Length - 1]);
            GL.End();
        }
        public static void DrawCylinder(FloatPoint3 center, float topRadius, float bottomRadius, float height, Color? fillColor = null, Color? borderColor = null)
        {
            var segments = 360;
            var topPoints = new float[segments * 2];
            var bottomPoints = new float[segments * 2];
            var arrayLength = topPoints.Length;
            var count = 0;
            for (var angle = 0f; angle < 360; angle += segments / 360f)
            {
                topPoints[count] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * topRadius);
                topPoints[count + 1] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * topRadius);
                bottomPoints[count] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * bottomRadius);
                bottomPoints[count + 1] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * bottomRadius);
                count += 2;
            }

            DrawCylinderBase(center - (0f, height / 2, 0f), bottomPoints, fillColor, borderColor);
            DrawCylinderBase(center + (0f, height / 2, 0f), topPoints, fillColor, borderColor);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            for (var i = 0; i < arrayLength / 2 - 1; i++)
            {
                GL.Vertex3(topPoints[i * 2], center.Y + height / 2, topPoints[i * 2 + 1]);
                GL.Vertex3(bottomPoints[i * 2], center.Y - height / 2, bottomPoints[i * 2 + 1]);
                GL.Vertex3(bottomPoints[i * 2 + 2], center.Y - height / 2, bottomPoints[i * 2 + 3]);
                GL.Vertex3(topPoints[i * 2 + 2], center.Y + height / 2, topPoints[i * 2 + 3]);
            }
            GL.Vertex3(topPoints[0], center.Y + height / 2, topPoints[1]);
            GL.Vertex3(bottomPoints[0], center.Y - height / 2, bottomPoints[1]);
            GL.Vertex3(bottomPoints[arrayLength - 2], center.Y - height / 2, bottomPoints[arrayLength - 1]);
            GL.Vertex3(topPoints[arrayLength - 2], center.Y + height / 2, topPoints[arrayLength - 1]);
            GL.End();
        }
        public static void DrawCylinder(FloatPoint3 center, float topHorizontalRadius, float topVerticalRadius, float bottomHorizontalRadius, float bottomVerticalRadius, float height, Color? fillColor = null, Color? borderColor = null)
        {
            var segments = 360;
            var topPoints = new float[segments * 2];
            var bottomPoints = new float[segments * 2];
            var arrayLength = topPoints.Length;
            var count = 0;
            for (var angle = 0f; angle < 360; angle += segments / 360f)
            {
                topPoints[count] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * topHorizontalRadius);
                topPoints[count + 1] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * topVerticalRadius);
                bottomPoints[count] = (float)(center.X + Math.Cos(Math.PI / 180f * angle) * bottomHorizontalRadius);
                bottomPoints[count + 1] = (float)(center.Z + Math.Sin(Math.PI / 180f * angle) * bottomVerticalRadius);
                count += 2;
            }

            DrawCylinderBase(center - (0f, height / 2, 0f), bottomPoints, fillColor, borderColor);
            DrawCylinderBase(center + (0f, height / 2, 0f), topPoints, fillColor, borderColor);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            for (var i = 0; i < arrayLength / 2 - 1; i++)
            {
                GL.Vertex3(topPoints[i * 2], center.Y + height / 2, topPoints[i * 2 + 1]);
                GL.Vertex3(bottomPoints[i * 2], center.Y - height / 2, bottomPoints[i * 2 + 1]);
                GL.Vertex3(bottomPoints[i * 2 + 2], center.Y - height / 2, bottomPoints[i * 2 + 3]);
                GL.Vertex3(topPoints[i * 2 + 2], center.Y + height / 2, topPoints[i * 2 + 3]);
            }
            GL.Vertex3(topPoints[0], center.Y + height / 2, topPoints[1]);
            GL.Vertex3(bottomPoints[0], center.Y - height / 2, bottomPoints[1]);
            GL.Vertex3(bottomPoints[arrayLength - 2], center.Y - height / 2, bottomPoints[arrayLength - 1]);
            GL.Vertex3(topPoints[arrayLength - 2], center.Y + height / 2, topPoints[arrayLength - 1]);
            GL.End();
        }
        private static void DrawCylinderBase(FloatPoint3 center, float[] points, Color? fillColor = null, Color? borderColor = null)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            GL.Vertex3(center.X, center.Y, center.Z);
            for (var i = 0; i < points.Length / 2; i++)
            {
                GL.Vertex3(points[i * 2], center.Y, points[i * 2 + 1]);
            }
            GL.Vertex3(points[0], center.Y, points[1]);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(borderColor == null ? Settings.Drawing.BorderColor : (Color)borderColor);
            for (var i = 0; i < points.Length / 2; i++)
            {
                GL.Vertex3(points[i * 2], center.Y, points[i * 2 + 1]);
            }
            GL.End();
        }
        public static void DrawTruncatedPyramid(FloatPoint3 center, float topBaseWidth, float topBaseHeight, float bottomBaseWidth, float bottomBaseHeight, float pyramidHeight, Color? fillColor = null, Color? borderColor = null)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);

            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);

            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);

            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(borderColor == null ? Settings.Drawing.BorderColor : (Color)borderColor);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z + bottomBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z + topBaseHeight / 2);
            GL.Vertex3(center.X + bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X + topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.Vertex3(center.X - bottomBaseWidth / 2, center.Y - pyramidHeight / 2, center.Z - bottomBaseHeight / 2);
            GL.Vertex3(center.X - topBaseWidth / 2, center.Y + pyramidHeight / 2, center.Z - topBaseHeight / 2);
            GL.End();
        }
        public static void DrawPyramid(FloatPoint3 center, float baseWidth, float baseHeight, float pyramidHeight, Color? fillColor = null, Color? borderColor = null)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex3(center.X, center.Y + pyramidHeight / 2, center.Z);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(borderColor == null ? Settings.Drawing.BorderColor : (Color)borderColor);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(center.X, center.Y + pyramidHeight / 2, center.Z);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.Vertex3(center.X, center.Y + pyramidHeight / 2, center.Z);
            GL.Vertex3(center.X - baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X, center.Y + pyramidHeight / 2, center.Z);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z - baseHeight / 2);
            GL.Vertex3(center.X, center.Y + pyramidHeight / 2, center.Z);
            GL.Vertex3(center.X + baseWidth / 2, center.Y - pyramidHeight / 2, center.Z + baseHeight / 2);
            GL.End();
        }
        public static void DrawParallelepiped(FloatPoint3 firstPoint, FloatPoint3 secondPoint, Color? fillColor = null, Color? borderColor = null)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(fillColor == null ? Settings.Drawing.FillColor : (Color)fillColor);
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

            GL.Color4(borderColor == null ? Settings.Drawing.BorderColor : (Color)borderColor);

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
