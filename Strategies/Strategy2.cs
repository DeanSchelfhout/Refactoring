using ConsoleAppSquareMaster;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Strategy2 : IStrategy
    {
        private Random random = new Random();

        public (int[,] worldEmpires,int x,int y) Conquer(bool[,] world, int empireID, int turns)
        {
            int maxX = world.GetLength(0);
            int maxY = world.GetLength(1);
            int x = 0, y = 0;
            int[,] worldEmpires = InitializeWorldEmpires(world, maxX, maxY);
            Dictionary<int, List<(int, int)>> empires = InitializeEmpires(world, empireID, worldEmpires);

            for (int t = 0; t < turns; t++)
            {
                foreach (var empire in empires)
                {
                    int index = FindWithMostEmptyNeighbours(empire.Value, worldEmpires, maxX, maxY);
                    (x, y) = empire.Value[index];
                    ExpandEmpire(worldEmpires, empires, empire.Key, x, y, maxX, maxY);
                }
            }

            return (worldEmpires, x, y);
        }

        private int[,] InitializeWorldEmpires(bool[,] world, int maxX, int maxY)
        {
            int[,] worldEmpires = new int[maxX, maxY];
            for (int i = 0; i < maxX; i++)
                for (int j = 0; j < maxY; j++)
                    worldEmpires[i, j] = world[i, j] ? 0 : -1;
            return worldEmpires;
        }

        private Dictionary<int, List<(int, int)>> InitializeEmpires(bool[,] world, int nEmpires, int[,] worldEmpires)
        {
            var empires = new Dictionary<int, List<(int, int)>>();
            int maxX = world.GetLength(0), maxY = world.GetLength(1);
            for (int i = 1; i <= nEmpires; i++)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = random.Next(maxX), y = random.Next(maxY);
                    if (world[x, y] && worldEmpires[x, y] == 0)
                    {
                        worldEmpires[x, y] = i;
                        empires[i] = new List<(int, int)> { (x, y) };
                        placed = true;
                    }
                }
            }
            return empires;
        }

        private int FindWithMostEmptyNeighbours(List<(int, int)> empireCells, int[,] worldEmpires, int maxX, int maxY)
        {
            int maxEmpty = -1;
            List<int> candidates = new();

            for (int i = 0; i < empireCells.Count; i++)
            {
                var (x, y) = empireCells[i];
                int emptyCount = CountEmptyNeighbours(x, y, worldEmpires, maxX, maxY);

                if (emptyCount > maxEmpty)
                {
                    maxEmpty = emptyCount;
                    candidates.Clear();
                    candidates.Add(i);
                }
                else if (emptyCount == maxEmpty)
                {
                    candidates.Add(i);
                }
            }

            return candidates[random.Next(candidates.Count)];
        }

        private int CountEmptyNeighbours(int x, int y, int[,] worldEmpires, int maxX, int maxY)
        {
            int count = 0;

            if (x > 0 && worldEmpires[x - 1, y] == 0) count++;
            if (x < maxX - 1 && worldEmpires[x + 1, y] == 0) count++;
            if (y > 0 && worldEmpires[x, y - 1] == 0) count++;
            if (y < maxY - 1 && worldEmpires[x, y + 1] == 0) count++;

            return count;
        }

        private void ExpandEmpire(int[,] worldEmpires, Dictionary<int, List<(int, int)>> empires, int empireId, int x, int y, int maxX, int maxY)
        {
            var directions = new List<(int dx, int dy)>
            {
                (1, 0), (-1, 0), (0, 1), (0, -1)
            };

            foreach (var (dx, dy) in directions.OrderBy(_ => random.Next()))
            {
                int newX = x + dx, newY = y + dy;
                if (newX >= 0 && newX < maxX && newY >= 0 && newY < maxY && worldEmpires[newX, newY] == 0)
                {
                    worldEmpires[newX, newY] = empireId;
                    empires[empireId].Add((newX, newY));
                    return;
                }
            }
        }
    }
}
