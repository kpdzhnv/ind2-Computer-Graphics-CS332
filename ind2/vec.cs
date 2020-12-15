using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ind2
{
    public class Vec3
    {
        public double x, y, z;
        public Vec3(double x_, double y_, double z_)
        {
            x = x_;
            y = y_;
            z = z_;
        }
        public Vec3()
        {
            x = 0;
            y = 0;
            z = 0;
        }
        public static Vec3 operator +(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vec3 operator -(Vec3 v1, Vec3 v2)
        {
            return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static double operator *(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
        public static Vec3 operator *(Vec3 v, double f)
        {
            return new Vec3(v.x * f, v.y * f, v.z * f);
        }
        public static Vec3 operator /(Vec3 v, double f)
        {
            return v * (1 / f);
        }
        public static double operator %(Vec3 v1, Vec3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
        public double Length()
        {
            return Math.Sqrt(this % this);
        }
        public Vec3 normalize()
        {
            return this / Length();
        }
        public static Vec3 operator !(Vec3 v)
        {
            return v / v.Length();
        }
        public override string ToString()
        {
            return String.Format("{0}{1}, {2}, {3}{4}", "{", x, y, z, "}");
        }
    }
    public class Vec2
    {
        public double x, y;
        public Vec2(double x_, double y_)
        {
            x = x_;
            y = y_;
        }
        public static Vec2 operator +(Vec2 v1, Vec3 v2)
        {
            return new Vec2(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vec2 operator -(Vec2 v1, Vec2 v2)
        {
            return new Vec2(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vec2 operator *(Vec2 v, double f)
        {
            return new Vec2(v.x * f, v.y * f);
        }
        public static Vec2 operator /(Vec2 v, double f)
        {
            return v * (1 / f);
        }
        public static double operator %(Vec2 v1, Vec2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }
        public double len()
        {
            return Math.Sqrt(this % this);
        }
        public static Vec2 operator !(Vec2 v)
        {
            return v / v.len();
        }
        public override string ToString()
        {
            return String.Format("{0}{1}, {2}{4}", "{", x, y, "}");
        }
    }

    public class Ray
    {
        public Vec3 origin;
        public Vec3 direction;

        public Ray()
        {
            origin = new Vec3();
            direction = new Vec3();
        }

        public Ray(Vec3 _origin, Vec3 _dir)
        {
            origin = _origin;
            direction = _dir;
        }
        public Vec3 Point(double t)
        {
            return origin + (direction * t);
        }
    }
}
