using OpenGL.Window.Graphics.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenGL.Window.Graphics.Properties;

namespace OpenGL
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new OpenGLWindow
            (
                width: 900,
                height: 900,
                title: "OpenGL App",
                updateFrequency: 60,
                renderFrequency: 60,
                backgroundColor: Color.DarkSeaGreen
            );

            window.Run();
        }
    }
}
