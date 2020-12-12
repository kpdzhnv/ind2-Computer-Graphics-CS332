using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ind2
{
    public class Material
    {
        public Vec3 color;
        public Material(Vec3 _color) 
        {
            color = _color;
        }
        public Material()
        {
            color = new Vec3();
        }
    }
}
