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
        public double diffuse_albedo;
        public double specular_albedo;
        public double transparency_albedo;
        public double reflectiveness_albedo;
        public double specular_value;
        public Material(Vec3 _color, double _diffuse_albedo, double _specular_albedo, double _reflectiveness_albedo, double _transparency_albedo, double _specular_value = 0.5) 
        {
            color = _color;
            diffuse_albedo = _diffuse_albedo;
            specular_albedo = _specular_albedo;
            transparency_albedo = _transparency_albedo;
            reflectiveness_albedo = _reflectiveness_albedo;

            specular_value = _specular_value;
        }
        public Material()
        {
            color = new Vec3();
        }
    }
}
