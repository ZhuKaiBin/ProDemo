using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeModels_Console
{
    public class DrawingAPI2Color : IDrawingColorAPI
    {
        public void DrawShapeColor(double x, double y, double radius)
        {
            Console.WriteLine($"颜色2.circle at {x}:{y} radius {radius}");
        }
    }
}
