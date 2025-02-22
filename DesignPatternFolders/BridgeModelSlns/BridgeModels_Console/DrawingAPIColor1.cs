using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeModels_Console
{
    public class DrawingAPIColor1 : IDrawingColorAPI
    {
        public void DrawShapeColor(double x, double y, double radius)
        {
            Console.WriteLine($"颜色1 at {x}:{y} radius {radius}");
        }
    }
}
