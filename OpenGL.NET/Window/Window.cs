using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

using Graphics = OpenGL.Window.Graphics.Graphics;
using Objects = OpenGL.Window.Graphics.Objects;
using OpenGL.Window.Camera;
using OpenGL.Window.Graphics;
using OpenGL.Window.Graphics.Properties;

namespace OpenGL
{
    public class OpenGLWindow : GameWindow
    {
        public bool ShowNormals { get; private set; } = false;
        private FloatColor BackgroundColor { get; set; } = Color.Black;
        private double alpha { get; set; } = 0.0;
        private Camera Camera { get; set; }
        private int Rotation { get; set; } = 0;
        private FloatPoint3 LightPosition { get; set; } = (30f, 150f, -30f);
        public OpenGLWindow
        (
            int width = Settings.Window.Width,
            int height = Settings.Window.Height,
            string title = Settings.Window.Title,
            int updateFrequency = Settings.Window.UpdateFrequency,
            int renderFrequency = Settings.Window.RenderFrequency,
            FloatColor backgroundColor = null
        )
            : base(width, height, GraphicsMode.Default, title)
        {
            if (backgroundColor == null) BackgroundColor = Settings.Drawing.BackgroundColor;
            else BackgroundColor = backgroundColor;
            TargetUpdateFrequency = updateFrequency;
            TargetRenderFrequency = renderFrequency;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Camera.UpdateState();
            Camera.ProcessInput();

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.Q)) Rotation += 1;
            if (keyboardState.IsKeyDown(Key.E)) Rotation -= 1;
            if (keyboardState.IsKeyDown(Key.R)) Rotation = 0;
            if (Rotation <= -360 || Rotation >= 360) Rotation = 0;
            
            base.OnUpdateFrame(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            Graphics.Initialize(this);

            CursorVisible = false;
            CursorGrabbed = true;

            GL.ClearColor(BackgroundColor);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            this.Camera = new Camera(this);
            var mouseState = Mouse.GetState();
            Camera.LastMousePos = new Vector2(mouseState.X, mouseState.Y);

            base.OnLoad(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
            base.OnUnload(e);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            Camera.UpdateState();
            base.OnResize(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.Escape)) this.Close();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.N))
            {
                if (ShowNormals == true) ShowNormals = false;
                else ShowNormals = true;
            }
            if (keyboardState.IsKeyDown(Key.V))
            {
                if (Camera.VerticalMovement) Camera.VerticalMovement = false;
                else Camera.VerticalMovement = true;
            }
            if (keyboardState.IsKeyDown(Key.O))
            {
                Console.WriteLine($"{Camera.Orientation.X} {Camera.Orientation.Y} {Camera.Orientation.Z}");
            }
        }
        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);
            var mouseState = Mouse.GetState();
            Camera.LastMousePos = new Vector2(mouseState.X, mouseState.Y);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Graphics.DrawCoordinatesSystem(80f);

            Graphics.CreateCoordinatesSystem(() =>
            {
                GL.Rotate(Rotation, 0, 0, 1);
                Objects.DrawBench();
            });

            //Graphics.CreateNewCoordinatesSystem(() =>
            //{
            //    GL.Translate(0, 100f, 0);
            //    GL.Rotate(alpha * 5, 0, 0, 1);
            //    Graphics.DrawCoordinatesSystem(20f);
            //    Graphics.DrawCylinder((0, 0f, 0), 3f, 30f, Color.Pink);
            //    Graphics.DrawSphere((0, 15f, 0), 3f, Color.DeepPink);
            //    Graphics.DrawSphere((-3.5f, -15f, 0), 5f, Color.HotPink);
            //    Graphics.DrawSphere((3.5f, -15f, 0), 5f, Color.HotPink);
            //});

            Graphics.CreateCoordinatesSystem(() =>
            {
                GL.Translate(0, 15f, 0);
                Graphics.DrawCylinder((0, 0, 0), 1.5f, 2f, 5f, Color.Green, Color.Black);
                Graphics.DrawSphere((0, 0, 0), 5f, Color.FromArgb(100, Color.HotPink));
            });

            Graphics.CreateCoordinatesSystem(() =>
            {
                GL.Translate(0, -12f, 0);
                Graphics.DrawParallelepiped((-100f, 0, 60f), (100f, -5f, -60f), Color.Gray, Color.Black);
            });

            Graphics.CreateCoordinatesSystem(() =>
            {
                GL.Translate(-50f, 0, 0);
                Graphics.DrawPyramid((0, 5f, 0), 10f, 10f, 10f, Color.Yellow, Color.Black);
            });

            Graphics.CreateCoordinatesSystem(() =>
            {
                GL.Translate(-80f, 0, 0);
                Graphics.DrawTruncatedPyramid((0, 5f, 0), 5f, 15f, 10f, 7f, 10f, Color.Green, Color.Black);
                Graphics.DrawSphere((0, 5f, 0), 1f, Color.Red);
            });

            //Graphics.CreateCoordinatesSystem(() =>
            //{
            //    GL.Translate(0, 20f, 90f);

            //    Graphics.CreateCoordinatesSystem(() =>
            //    {
            //        Graphics.DrawSphere((0, 0, 20f), 7f, Color.FromArgb(100, Color.GreenYellow));
            //    });

            //    Graphics.CreateCoordinatesSystem(() =>
            //    {
            //        Graphics.DrawParallelepiped((-10f, 5f, 0.1f), (10f, -5f, -0.1f), Color.Transparent);
            //    });

            //    GL.Begin(PrimitiveType.Quads);
            //    GL.Color3(Color.Brown);
            //    GL.Vertex3(-30f, -25f, 0);
            //    GL.Vertex3(30f, -25f, 0);
            //    GL.Vertex3(30f, 25f, 0);
            //    GL.Vertex3(-30f, 25f, 0);
            //    GL.End();
            //});

            Context.SwapBuffers();

            alpha += 1.0;
            if (alpha > 360)
            {
                alpha -= 360;
            }

            base.OnRenderFrame(e);
        }
    }
}