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

        public Form1()
        {
            InitializeComponent();
            width = image.Width;
            height = image.Height;
            camera_pos = new Vec3(0, 0, 10);
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
                    Vec3 c = cast_ray(new Ray(camera_pos, dir), new Sphere(new Vec3(0, 0, 0), 2, new Material(new Vec3(20, 20, 20))));

                    bmp.SetPixel(i, j, Color.FromArgb(255, (int)c.x, (int)c.y, (int)c.z));
                }
            }
            image.Image = bmp;
        }

        public Vec3 cast_ray(Ray ray, Sphere sphere)
        {
            if (sphere.intersect(ray) == -1)
                return new Vec3( 150, 150, 255);
            return new Vec3( 0, 255, 255);
        }
    }
}
