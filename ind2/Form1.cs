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

            spheres = new List<Sphere>();
            spheres.Add(new Sphere(new Vec3(0, 0, 0), 2.0, new Material(new Vec3(255, 150, 255), 0.6, 0.1, 0.6, 0.1, 0.1)));
            spheres.Add(new Sphere(new Vec3(0, 5, 0), 3.0, new Material(new Vec3(255, 150, 255), 0.9, 0.3, 0.6, 0.1, 5)));
            spheres.Add(new Sphere(new Vec3(-5, -1, -2), 4.0, new Material(new Vec3(255, 150, 255), 0.6, 0.3, 0.6, 0.1, 10.0)));

            lights = new List<Light>();
            lights.Add(new Light(new Vec3(10, 7, 30), 1.0));
            lights.Add(new Light(new Vec3(-10, -20, 20), 1.0));
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
                    if (c.x > 255)
                        c.x = 255;
                    if (c.y > 255)
                        c.y = 255;
                    if (c.z > 255)
                        c.z = 255;

                    bmp.SetPixel(i, j, Color.FromArgb(255, (int)c.x, (int)c.y, (int)c.z));
                }
            }
            image.Image = bmp;
        }

        public Vec3 cast_ray(Ray ray, int depth = 0)
        {
            Vec3 point = new Vec3(), N = new Vec3();
            Material m = new Material(new Vec3(255, 0, 0), 1, 1, 1, 0);
            if (depth > 4 || !scene_intersect(ray, ref m, ref point, ref N))
                return new Vec3(150, 150, 255);

            Vec3 reflect_dir = reflect(ray.direction, N).normalize();
            Vec3 reflect_orig = reflect_dir * N < 0 ? point - N * 1e-3 : point + N * 1e-3;
            Ray reflect_ray = new Ray(reflect_orig, reflect_dir);
            Vec3 reflect_color = cast_ray(reflect_ray, depth + 1);
            double diffuse_light_intensity = 0, specular_light_intensity = 0;
            for (int i = 0; i < lights.Count(); i++)
            {
                Vec3 light_dir = (lights[i].position - point).normalize();

                double light_distance = (lights[i].position - point).Length();

                Vec3 shadow_orig = light_dir * N < 0 ? point - N * 1e-3 : point + N * 1e-3; 
                Vec3 shadow_pt = new Vec3(), shadow_N = new Vec3();
                Material tmpmaterial = new Material();
                Ray shadow_ray = new Ray(shadow_orig, light_dir);
                if (scene_intersect(shadow_ray, ref tmpmaterial, ref shadow_pt, ref shadow_N) && (shadow_pt - shadow_orig).Length() < light_distance)
                    continue;

                diffuse_light_intensity += lights[i].intensity * Math.Max(0.0, light_dir * N);
                specular_light_intensity += Math.Pow(Math.Max(0.0, reflect(light_dir, N) * ray.direction), m.specular_value)
                    * lights[i].intensity;
            }
            if (specular_light_intensity > 1)
                specular_light_intensity = 1;
            return m.color * diffuse_light_intensity * m.diffuse_albedo + 
                new Vec3(255, 255, 255) * specular_light_intensity * m.specular_albedo +
                reflect_color * m.reflectiveness_albedo;
        }

        public Vec3 reflect(Vec3 I, Vec3 N)
        {
            return I - N * 2.0 * (I * N);
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
