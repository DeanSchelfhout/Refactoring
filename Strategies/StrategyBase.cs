using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public abstract class StrategyBase
    {
        public bool[,] world;

        public int[,] worldempires;
        public int maxx, maxy;
        public Random random = new Random(1);

        public StrategyBase(bool[,] world)
        {
            this.world = world;
            maxx = world.GetLength(0);
            maxy = world.GetLength(1);
            worldempires = new int[maxx, maxy];
            for (int i = 0; i < world.GetLength(0); i++) for (int j = 0; j < world.GetLength(1); j++) if (world[i, j]) worldempires[i, j] = 0; else worldempires[i, j] = -1;
        }
        public void pickEmpty(List<(int, int)> empire, int index, int e)
        {
            List<(int, int)> n = new List<(int, int)>();
            if (IsValidPosition(empire[index].Item1 - 1, empire[index].Item2)
                && worldempires[empire[index].Item1 - 1, empire[index].Item2] == 0) n.Add((empire[index].Item1 - 1, empire[index].Item2));
            if (IsValidPosition(empire[index].Item1 + 1, empire[index].Item2)
                && worldempires[empire[index].Item1 + 1, empire[index].Item2] == 0) n.Add((empire[index].Item1 + 1, empire[index].Item2));
            if (IsValidPosition(empire[index].Item1, empire[index].Item2 - 1)
                && worldempires[empire[index].Item1, empire[index].Item2 - 1] == 0) n.Add((empire[index].Item1, empire[index].Item2 - 1));
            if (IsValidPosition(empire[index].Item1, empire[index].Item2 + 1)
                && worldempires[empire[index].Item1, empire[index].Item2 + 1] == 0) n.Add((empire[index].Item1, empire[index].Item2 + 1));
            int x = random.Next(n.Count);
            if (n.Count > 0)
            {
                empire.Add(n[x]);
                worldempires[n[x].Item1, n[x].Item2] = e;
            }
        }
        public int FindWithMostEmptyNeighbours(int e, List<(int, int)> empire)
        {
            List<int> indexes = new List<int>();
            int n = 0;
            int calcN;
            for (int i = 0; i < empire.Count; i++)
            {
                calcN = EmptyNeighbours(e, empire[i].Item1, empire[i].Item2);
                if (calcN >= n)
                {
                    indexes.Clear();
                    n = calcN;
                    indexes.Add(i);
                }
            }
            return indexes[random.Next(indexes.Count)];
        }
        public int EmptyNeighbours(int empire, int x, int y)
        {
            int n = 0;
            if (IsValidPosition(x - 1, y) && worldempires[x - 1, y] == 0) n++;
            if (IsValidPosition(x + 1, y) && worldempires[x + 1, y] == 0) n++;
            if (IsValidPosition(x, y - 1) && worldempires[x, y - 1] == 0) n++;
            if (IsValidPosition(x, y + 1) && worldempires[x, y + 1] == 0) n++;
            return n;
        }
        public bool IsValidPosition(int x, int y)
        {
            if (x < 0) return false;
            if (x >= world.GetLength(0)) return false;
            if (y < 0) return false;
            if (y >= world.GetLength(1)) return false;
            return true;
        }
    }
}
