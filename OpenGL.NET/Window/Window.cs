﻿using System;
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
        public OpenGLWindow
        (
            int width,
            int height,
            string title = Settings.Window.WindowTitle,
            int updateFrequency = Settings.Window.WindowUpdateFrequency,
            int renderFrequency = Settings.Window.WindowRenderFrequency,
            FloatColor backgroundColor = null
        )
            : base(width, height, GraphicsMode.Default, title)
        {
            BackgroundColor = backgroundColor;
            TargetUpdateFrequency = updateFrequency;
            TargetRenderFrequency = renderFrequency;
        }
        public OpenGLWindow() : base
        (
            Settings.Window.WindowWidth,
            Settings.Window.WindowHeight,
            GraphicsMode.Default,
            Settings.Window.WindowTitle
        )
        {
            TargetUpdateFrequency = Settings.Window.WindowUpdateFrequency;
            TargetRenderFrequency = Settings.Window.WindowRenderFrequency;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var matrix = Camera.ViewMatrix * Matrix4.CreatePerspectiveFieldOfView(((MathHelper.Pi / 180) * 70.0f), Width / Height, 1.0f, 300.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            base.OnUpdateFrame(e);
            Camera.ProcessInput();

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.Q)) Rotation += 1;
            if (keyboardState.IsKeyDown(Key.E)) Rotation -= 1;
            if (keyboardState.IsKeyDown(Key.R)) Rotation = 0;
            if (Rotation <= -360 || Rotation >= 360) Rotation = 0;
        }
        protected override void OnLoad(EventArgs e)
        {
            Graphics.Initialize(this);

            CursorVisible = false;
            CursorGrabbed = true;

            GL.ClearColor(BackgroundColor);
            GL.Enable(EnableCap.DepthTest);

            this.Camera = new Camera(this);
            var mouseState = Mouse.GetState();
            Camera.LastMousePos = new Vector2(mouseState.X, mouseState.Y);

            base.OnLoad(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            GL.Disable(EnableCap.DepthTest);
            base.OnUnload(e);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var matrix = Camera.ViewMatrix * Matrix4.CreatePerspectiveFieldOfView((((float)Math.PI / 180) * 70.0f), Width / Height, 1.0f, 300.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            base.OnResize(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.Escape)) this.Close();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            var keyboarState = Keyboard.GetState();
            if (keyboarState.IsKeyDown(Key.N))
            {
                if (ShowNormals == true) ShowNormals = false;
                else ShowNormals = true;
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

            Graphics.DrawCoordinatesSystem(80f);

            GL.PushMatrix();
            GL.Rotate(Rotation, 0, 0, 1);
            Objects.DrawBench();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0, 100f, 0);
            GL.Rotate(alpha * 3, 0, 0, 1);
            Graphics.DrawCoordinatesSystem(20f);
            Graphics.DrawCylinder((0, 0f, 0), 3f, 30f, Color.Pink);
            Graphics.DrawSphere((0, 15f, 0), 3f, Color.DeepPink);
            Graphics.DrawSphere((-3.5f, -15f, 0), 5f, Color.HotPink);
            Graphics.DrawSphere((3.5f, -15f, 0), 5f, Color.HotPink);
            GL.PopMatrix();

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