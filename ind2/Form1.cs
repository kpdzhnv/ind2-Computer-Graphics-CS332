using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ind2
{
    public partial class Form1 : Form
    {
        public int width, height;
        public Bitmap bmp;
        public Vec3 camera_pos;
        public List<Light> lights;
        public List<Sphere> spheres;

        public Form1()
        {
            InitializeComponent();
            width = image.Width;
            height = image.Height;
            camera_pos = new Vec3(0, 0, 10);

            lights = new List<Light>();
            spheres = new List<Sphere>();
            spheres.Add(new Sphere(new Vec3(0, 0, 0), 2.0, new Material(new Vec3(20, 20, 20))));
            spheres.Add(new Sphere(new Vec3(0, 5, 0), 3.0, new Material(new Vec3(20, 20, 20))));
            spheres.Add(new Sphere(new Vec3(-3, -1, -2), 4.0, new Material(new Vec3(20, 20, 20))));
            lights.Add(new Light(new Vec3(10, 7, 20), 5));
        }

        private void image_Click(object sender, EventArgs e)
        {
            render();
        }

        public void render()
        {
            bmp = new Bitmap(width, height);
            double fov = Math.PI / 2.0;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    double x = (2 * (i + 0.5) / (double)width - 1) * Math.Tan(fov / 2.0) * width / (double)height;
                    double y = -(2 * (j + 0.5) / (double)height - 1) * Math.Tan(fov / 2.0);
                    Vec3 dir = new Vec3(x, y, -1).normalize();
                    Vec3 c = cast_ray(new Ray(camera_pos, dir));

                    bmp.SetPixel(i, j, Color.FromArgb(255, (int)c.x, (int)c.y, (int)c.z));
                }
            }
            image.Image = bmp;
        }

        public Vec3 cast_ray(Ray ray)
        {
            Vec3 point = new Vec3(), N = new Vec3();
            Material m = new Material(new Vec3(255, 0, 0));
            if (!scene_intersect(ray, ref m, ref point, ref N))
                return new Vec3(150, 150, 255);

            double diffuse_light_intensity = 0;
            for (int i = 0; i < lights.Count(); i++)
            {
                Vec3 light_dir = (lights[i].position - point).normalize();
                diffuse_light_intensity += lights[i].intensity * Math.Max(0.0, light_dir * N);
            }
            return m.color * diffuse_light_intensity;
        }

        public bool scene_intersect(Ray ray, ref Material material, ref Vec3 hit, ref  Vec3 N)
        {
            double spheres_dist = Double.MaxValue;
            for (int i = 0; i < spheres.Count(); i++)
            {
                double dist_i = spheres[i].intersect(ray);
                if (dist_i != -1 && dist_i < spheres_dist)
                {
                    spheres_dist = dist_i;
                    hit = ray.origin + ray.direction * dist_i;
                    N = (hit - spheres[i].center).normalize();
                    material = spheres[i].material;
                }
            }
            return spheres_dist < 1000;
        }
    }
}
