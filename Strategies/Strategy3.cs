using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Strategy3 : StrategyBase, IStrategy
    {
        public Strategy3(bool[,] world) : base(world)
        {
        }

        public int[,] Conquer(int nEmpires, int turns)
        {
            Dictionary<int, List<(int, int)>> empires = new();

            int x, y;
            for (int i = 0; i < nEmpires; i++)
            {
                bool ok = false;
                while (!ok)
                {
                    x = random.Next(maxx); y = random.Next(maxy);
                    if (world[x, y])
                    {
                        ok = true;
                        worldempires[x, y] = i + 1;
                        empires.Add(i + 1, new List<(int, int)>() { (x, y) });
                    }
                }
            }
            int index;
            for (int i = 0; i < turns; i++)
            {
                for (int e = 1; e <= nEmpires; e++)
                {
                    index = random.Next(empires[e].Count);
                    pickEmpty(empires[e], index, e);
                }
            }
            return worldempires;
        }
    }
}
