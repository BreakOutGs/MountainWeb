using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MountainWeb.Models
{
    struct ColorRGBA
    {
        public int r;
        public int g;
        public int b;
        public double a;
        public ColorRGBA(int R,int G, int B, double A)
        {
            r = R; g = G; b = B; a = A;
        }
    }

    public class PriorityColors
    {
      static   ColorRGBA[] colors = {
            new ColorRGBA(227,209,234,1),
            new ColorRGBA(129,218,97,1),
            new ColorRGBA(28,172,40,1),
            new ColorRGBA(100,122,234,1),
            new ColorRGBA(160,100,234,1),
            new ColorRGBA(162,47,211,1),
            new ColorRGBA(218,142,50,1)
            };
        public static string GetColorByPriority(int Priority)
        {
            double priorityPoint = 100 / colors.Length;
            ColorRGBA color = colors[Convert.ToInt32(Priority / priorityPoint)-1];
            return "rgba(" +color.r+", "+color.g+", "+color.b+", "+color.a+")";
        }


    }
}
