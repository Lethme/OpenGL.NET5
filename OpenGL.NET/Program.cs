using OpenGL.Window.Graphics.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new OpenGLWindow
            (
                width: 800,
                height: 800,
                title: "Shvabrinskoe govno v 3D",
                updateFrequency: 60,
                renderFrequency: 60,
                backgroundColor: Color.CornflowerBlue
            );

            window.Run();
        }
    }
}
