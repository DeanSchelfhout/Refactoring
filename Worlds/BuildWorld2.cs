using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Worlds
{
    public class BuildWorld2 : WorldBase, IBuildWorld
    {
        public BuildWorld2(Random random) : base(random)
        {
        }

        public bool[,] BuildWorld(int maxy, int maxx, double coverage)
        {
            bool[,] squares = new bool[maxx, maxy];
            int seeds = 5;
            int coverageRequired = (int)(coverage * maxx * maxy);
            int currentCoverage = 0;
            int x, y;
            List<(int, int)> list = new();
            for (int i = 0; i < seeds; i++)
            {
                x = random.Next(maxx); y = random.Next(maxy);
                if (!list.Contains((x, y))) { list.Add((x, y)); currentCoverage++; squares[x, y] = true; }

            }
            int index;
            int direction;
            while (currentCoverage < coverageRequired)
            {
                index = random.Next(list.Count);
                direction = random.Next(4);
                switch (direction)
                {
                    case 0:
                        if ((list[index].Item1 < maxx - 1) && !squares[list[index].Item1 + 1, list[index].Item2])
                        {
                            squares[list[index].Item1 + 1, list[index].Item2] = true;
                            list.Add((list[index].Item1 + 1, list[index].Item2));
                            currentCoverage++;
                        }
                        break;
                    case 1:
                        if ((list[index].Item1 > 0) && !squares[list[index].Item1 - 1, list[index].Item2])
                        {
                            squares[list[index].Item1 - 1, list[index].Item2] = true;
                            list.Add((list[index].Item1 - 1, list[index].Item2));
                            currentCoverage++;
                        }
                        break;
                    case 2:
                        if ((list[index].Item2 < maxy - 1) && !squares[list[index].Item1, list[index].Item2 + 1])
                        {
                            squares[list[index].Item1, list[index].Item2 + 1] = true;
                            list.Add((list[index].Item1, list[index].Item2 + 1));
                            currentCoverage++;
                        }
                        break;
                    case 3:
                        if ((list[index].Item2 > 0) && !squares[list[index].Item1, list[index].Item2 - 1])
                        {
                            squares[list[index].Item1, list[index].Item2 - 1] = true;
                            list.Add((list[index].Item1, list[index].Item2 - 1));
                            currentCoverage++;
                        }
                        break;
                }
            }
            return squares;
        }
    }
}
