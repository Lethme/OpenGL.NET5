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
using OpenGL.Window.Graphics;
using OpenGL.Window.Graphics.Properties;

namespace OpenGL
{
    public class OpenGLWindow : GameWindow
    {
        private double alpha { get; set; } = 0.0;
        private Camera Camera { get; set; }
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
            var matrix = Camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView((((float)Math.PI / 180) * 60.0f), Width / Height, 1.0f, 150.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            base.OnUpdateFrame(e);
            Camera.ProcessInput();
        }
        protected override void OnLoad(EventArgs e)
        {
            CursorVisible = false;
            CursorGrabbed = true;

            GL.ClearColor(Color.Black);
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
            var matrix = Camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView((((float)Math.PI / 180) * 45.0f), Width / Height, 1.0f, 100.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            base.OnResize(e);
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.Escape)) this.Close();
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

            Objects.DrawBench();

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