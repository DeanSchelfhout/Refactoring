using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Worlds
{
    public class BuildWorld1 : WorldBase, IBuildWorld
    {
        public BuildWorld1(Random random) : base(random)
        {
        }
        public bool[,] BuildWorld(int maxy, int maxx, double coverage)
        {
            int dx = maxx;
            int dy = maxy;
            bool[,] squares = new bool[dx, dy];
            int y1 = random.Next(dy);
            int y2 = random.Next(dy);
            int yb = Math.Min(y1, y2);
            int ye = Math.Max(y1, y2);
            for (int i = 0; i < dx; i++)
            {
                for (int j = yb; j < ye; j++) squares[i, j] = true;
                switch (build())
                {
                    case 1: if (yb > 0) yb--; break;
                    case -1: if (yb < maxy) yb++; break;
                }
                switch (build())
                {
                    case 1: if (ye < maxy) ye++; break;
                    case -1: if (ye > 0) ye--; break;
                }
                if (ye < yb) break;
            }
            return squares;
        }
    }
}
