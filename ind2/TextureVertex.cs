using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{
    class TextureVertex
    {
        public double U;
        public double V;
        public double W;

        public TextureVertex() { new TextureVertex(0, 0, 0); }

        public TextureVertex(double u, double v, double w = 1)
        {
            U = u;
            V = v;
            W = w;
        }
        public TextureVertex(TextureVertex tv)
        {
            U = tv.U;
            V = tv.V;
            W = tv.W;
        }

    }
}
