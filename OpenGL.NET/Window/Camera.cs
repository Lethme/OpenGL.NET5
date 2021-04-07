using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Camera
    {

        public Vector3 Position = Vector3.Zero;
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public Vector2 LastMousePos = new Vector2();
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.0015f;
        private NativeWindow Window { get; }
        public Camera(NativeWindow window)
        {
            this.Window = window;
        }
        public Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            return Matrix4.LookAt(Position, Position + lookat, Vector3.UnitY);
        }
        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            Position += offset;
        }
        public void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }
        public void ProcessInput()
        {
            if (Window.Focused)
            {
                var mouseState = Mouse.GetState();
                Vector2 delta = LastMousePos - new Vector2(mouseState.X, mouseState.Y);
                LastMousePos += delta;

                AddRotation(delta.X, delta.Y);
                LastMousePos = new Vector2(mouseState.X, mouseState.Y);
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
            if (keyboardState.IsKeyDown(Key.ShiftLeft))
            {
                Move(0f, 0f, -0.1f);
            }
        }
    }
}
