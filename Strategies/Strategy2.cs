using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Strategy2 : StrategyBase, IStrategy
    {
        public Strategy2(bool[,] world) : base(world)
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
            int direction;
            for (int i = 0; i < turns; i++)
            {
                for (int e = 1; e <= nEmpires; e++)
                {
                    index = FindWithMostEmptyNeighbours(e, empires[e]);
                    direction = random.Next(4);
                    x = empires[e][index].Item1;
                    y = empires[e][index].Item2;
                    switch (direction)
                    {
                        case 0:
                            if (x < maxx - 1 && worldempires[x + 1, y] == 0)
                            {
                                worldempires[x + 1, y] = e;
                                empires[e].Add((x + 1, y));
                            }
                            break;
                        case 1:
                            if (x > 0 && worldempires[x - 1, y] == 0)
                            {
                                worldempires[x - 1, y] = e;
                                empires[e].Add((x - 1, y));
                            }
                            break;
                        case 2:
                            if (y < maxy - 1 && worldempires[x, y + 1] == 0)
                            {
                                worldempires[x, y + 1] = e;
                                empires[e].Add((x, y + 1));
                            }
                            break;
                        case 3:
                            if (y > 0 && worldempires[x, y - 1] == 0)
                            {
                                worldempires[x, y - 1] = e;
                                empires[e].Add((x, y - 1));
                            }
                            break;
                    }
                }
            }
            return worldempires;
        }
    }
}
