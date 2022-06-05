using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolory_MAUI.Model
{
    public class Colors
    {
        public Colors(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
    }
}
