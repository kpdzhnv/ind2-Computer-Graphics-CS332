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
    class Wall
    {
        public Vec3 p1;
        public Vec3 p2;
        public Vec3 p3;
        public Vec3 p4;

        public Wall(Vec3 _p1, Vec3 _p2, Vec3 _p3, Vec3 _p4)
        {
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;
            p4 = _p4;
        }
        public Wall(List<Vec3> pnts)
        {
            p1 = pnts[0];
            p2 = pnts[1];
            p3 = pnts[2];
            p4 = pnts[3];
        }
    }
    class Sphere
    {
        public double radius;
        public Vec3 center;

        public Sphere(Vec3 _center, double _radius)
        {
            center = _center;
            radius = _radius;
        }
    }
    class Mesh
    {
        // points of polyhedron
        public List<Vec3> geometric_vertices = new List<Vec3>();
        // points of polyhedron
        public List<Vec2> texture_vertices = new List<Vec2>();
        // points of polyhedron
        public List<Vec3> vertex_normals = new List<Vec3>();

        // faces of polyhedron
        public List<Triangle> triangles = new List<Triangle>();
        // central point of polyhedron (weights)
        public Vec3 center_point = new Vec3();

        public Mesh(List<Vec3> vert, List<Triangle> triangles_)
        {
            geometric_vertices = vert;
            triangles = triangles_;
            center_point = SetCenter();
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
