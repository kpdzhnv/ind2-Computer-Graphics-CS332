using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ind2
{
    public class Wall
    {
        public Vec3 p1;
        public Vec3 p2;
        public Vec3 p3;
        public Vec3 p4;
        public Material material;

        public Wall(Vec3 _p1, Vec3 _p2, Vec3 _p3, Vec3 _p4, Material _material)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;
            p4 = _p4;
            material = _material;
        }
        public Wall(List<Vec3> pnts, Material _material)
        {
            p1 = pnts[0];
            p2 = pnts[1];
            p3 = pnts[2];
            p4 = pnts[3];
            material = _material;
        }
    }
    public class Sphere
    {
        public double radius;
        public Vec3 center;
        public Material material;

        public Sphere(Vec3 _center, double _radius, Material _material)
        {
            center = _center;
            radius = _radius;
            material = _material;
        }
        public double intersect(Ray ray)
        {
            double t;
            
            Vec3 L = center - ray.origin;
            double tca = scalar_prod(ray.direction, L);
            double d2 = scalar_prod(L, L) - tca * tca;
            if (d2 > radius * radius) 
                return -1;
            double thc = Math.Sqrt(radius * radius - d2);
            t = tca - thc;
            double t1 = tca + thc;
            if (t < 0) 
                t = t1;
            if (t < 0) 
                return -1;
            return t;
        }
        private double scalar_prod(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
    }
    public class Light
    {
        public Vec3 position;
        public double intensity;

        public Light(Vec3 pos, double intens)
        {
            position = pos;
            intensity = intens;
        }

        public double IntensityDiffusion(double koeff, Vec3 normal)
        {
            return intensity * koeff * cos(normal, position);
        }

        private double scalar_prod(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        private double cos(Vec3 v1, Vec3 v2)
        {
            return scalar_prod(v1, v2) / (v1.Length() * v2.Length());
        }

    }

    public class Mesh
    {
        public List<Vec3> geometric_vertices = new List<Vec3>();
        public List<Vec2> texture_vertices = new List<Vec2>();
        public List<Vec3> vertex_normals = new List<Vec3>();

        public List<Triangle> triangles = new List<Triangle>();
        public Vec3 center_point = new Vec3();
        public Material material;

        public Mesh(List<Vec3> vert, List<Triangle> triangles_, Material _material)
        {
            geometric_vertices = vert;
            triangles = triangles_;
            center_point = SetCenter();
            material = _material;
        }

        public Mesh() { }

        public void Clear()
        {
            geometric_vertices.Clear();
            texture_vertices.Clear();
            vertex_normals.Clear();
            triangles.Clear();
        }

        public void FromFile(string filename)
        {
            Clear();

            StreamReader sr = File.OpenText(filename);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                    continue;
                line = line.Replace(',', '.');
                string[] ss = line.Split();
                ss = ss.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string type = ss[0];
                switch (type)
                {
                    case "v":
                        geometric_vertices.Add(new Vec3(double.Parse(ss[1], CultureInfo.InvariantCulture),
                            double.Parse(ss[2], CultureInfo.InvariantCulture),
                            double.Parse(ss[3], CultureInfo.InvariantCulture)));
                        break;

                    case "vt":
                        texture_vertices.Add(new Vec2(double.Parse(ss[1], CultureInfo.InvariantCulture),
                            double.Parse(ss[2], CultureInfo.InvariantCulture)));
                        break;

                    case "vn":
                        vertex_normals.Add(new Vec3(double.Parse(ss[1], CultureInfo.InvariantCulture),
                            double.Parse(ss[2], CultureInfo.InvariantCulture),
                            double.Parse(ss[3], CultureInfo.InvariantCulture)));
                        break;

                    case "f":
                        triangles.Add(new Triangle(ss[1], ss[2], ss[3]));
                        break;

                    default:
                        break;
                }
            }

            sr.Close();
            center_point = SetCenter();
        }

        private Vec3 SetCenter(Vec3 p1, Vec3 p2, Vec3 p3) => (p1 + p2 + p3) / 3;

        public Vec3 SetCenter()
        {
            Vec3 p = new Vec3();
            foreach (Vec3 p1 in geometric_vertices)
                p += p1;
            return p / geometric_vertices.Count();
        }
    }
}
