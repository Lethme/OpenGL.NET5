using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace OpenGL.Window.Camera
{
    public class Camera
    {
        private float fov = Settings.Camera.Fov;
        private Vector3 position = Settings.Camera.InitialPosition;
        private Vector3 orientation = Settings.Camera.InitialOrientation;
        private Vector2 lastMousePos = new Vector2();
        public float MoveSpeed { get; set; } = Settings.Camera.MoveSpeed;
        public float MouseSensitivity { get; set; } = Settings.Camera.MouseSensitivity;
        private NativeWindow Window { get; }
        public Camera(NativeWindow window, Vector3? initialPosition = null, Vector3? initialOrientation = null)
        {
            if (window == null) throw new ArgumentNullException($"{nameof(window)} can't be null reference!");

            this.Window = window;
            if (initialPosition != null) this.position = (Vector3)initialPosition;
            if (initialOrientation != null) this.orientation = (Vector3)initialOrientation;
        }
        public float Fov 
        { 
            get { return fov; } 
            set
            {
                if (value < Settings.Camera.MinFov || value > Settings.Camera.MaxFov)
                    throw new ArgumentException($"Camera fov must be in specified range: [{Settings.Camera.MinFov}; {Settings.Camera.MaxFov}]!");
                fov = value;
            } 
        }
        public bool VerticalMovement { get; set; } = false;
        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Orientation { get { return orientation; } set { orientation = value; } }
        public Vector2 LastMousePos { get { return lastMousePos; } set { lastMousePos = value; } }
        public Matrix4 ViewMatrix
        { 
            get
            {
                Vector3 lookat = new Vector3();

                lookat.X = (float)(Math.Sin((float)orientation.X) * Math.Cos((float)orientation.Y));
                lookat.Y = (float)Math.Sin((float)orientation.Y);
                lookat.Z = (float)(Math.Cos((float)orientation.X) * Math.Cos((float)orientation.Y));

                return Matrix4.LookAt(position, position + lookat, Vector3.UnitY);
            }
        }
        private bool VerticalDirection => orientation.Y == MathHelper.PiOver2 - 0.0001f || orientation.Y == -MathHelper.PiOver2 + 0.0001f;
        public void UpdateState()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var matrix = ViewMatrix * Matrix4.CreatePerspectiveFieldOfView(((MathHelper.Pi / 180) * fov), Window.Width / Window.Height, 1.0f, 300.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3(VerticalDirection ? 0 : (float)Math.Sin(orientation.X), VerticalMovement ? (float)Math.Sin(orientation.Y) : 0, VerticalDirection ? 0 : (float)Math.Cos(orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            position += offset;
        }
        public void Rotate(float x, float y)
        {
            x *= MouseSensitivity;
            y *= MouseSensitivity;

            orientation.X = (orientation.X + x) % ((float)Math.PI * 2.0f);
            orientation.Y = Math.Max(Math.Min(orientation.Y + y, MathHelper.PiOver2 - 0.0001f), -MathHelper.PiOver2 + 0.0001f);
        }
        public void ProcessInput()
        {
            if (Window.Focused)
            {
                var mouseState = Mouse.GetState();
                Vector2 delta = lastMousePos - new Vector2(mouseState.X, mouseState.Y);
                lastMousePos += delta;

                Rotate(delta.X, delta.Y);
                lastMousePos = new Vector2(mouseState.X, mouseState.Y);
            }

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.W))
            {
                Move(0f, 0.1f, 0f);
            }
            if (keyboardState.IsKeyDown(Key.S))
            {
                Move(0f, -0.1f, 0f);
            }
            if (keyboardState.IsKeyDown(Key.A))
            {
                Move(-0.1f, 0f, 0f);
            }
            if (keyboardState.IsKeyDown(Key.D))
            {
                Move(0.1f, 0f, 0f);
            }
            if (keyboardState.IsKeyDown(Key.Space))
            {
                Move(0f, 0f, 0.1f);
            }
            if (keyboardState.IsKeyDown(Key.ControlLeft))
            {
                Move(0f, 0f, -0.1f);
            }
            if (keyboardState.IsKeyDown(Key.ShiftLeft))
            {
                this.MoveSpeed = Settings.Camera.MoveSpeedAccelerated;
            }
            else
            {
                this.MoveSpeed = Settings.Camera.MoveSpeed;
            }
        }
    }
}
