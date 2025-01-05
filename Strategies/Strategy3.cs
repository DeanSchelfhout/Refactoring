using ConsoleAppSquareMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleAppSquareMaster.Strategies
{
    public class Strategy3 : IStrategy
    {
        private Random random = new Random();

        public (int[,] worldEmpires, int x, int y) Conquer(bool[,] world, int empireID, int turns)
        {
            int maxX = world.GetLength(0);
            int maxY = world.GetLength(1);
            int x = 0, y = 0; // To track the start position
            int[,] worldEmpires = InitializeWorldEmpires(world, maxX, maxY);
            var empires = InitializeEmpires(world, empireID, worldEmpires);

            // Get the start position for the first empire
            if (empires.ContainsKey(empireID) && empires[empireID].Count > 0)
            {
                (x, y) = empires[empireID][0];
            }

            for (int t = 0; t < turns; t++)
            {
                foreach (var empire in empires)
                {
                    PickAndExpand(empire.Value, empire.Key, worldEmpires, maxX, maxY);
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

        private void PickAndExpand(List<(int, int)> empireCells, int empireId, int[,] worldEmpires, int maxX, int maxY)
        {
            var candidates = new List<(int, int)>();

            foreach (var (x, y) in empireCells)
            {
                AddIfValid(x + 1, y, candidates, worldEmpires, maxX, maxY);
                AddIfValid(x - 1, y, candidates, worldEmpires, maxX, maxY);
                AddIfValid(x, y + 1, candidates, worldEmpires, maxX, maxY);
                AddIfValid(x, y - 1, candidates, worldEmpires, maxX, maxY);
            }

            if (candidates.Count > 0)
            {
                var (newX, newY) = candidates[random.Next(candidates.Count)];
                worldEmpires[newX, newY] = empireId;
                empireCells.Add((newX, newY));
            }
        }

        private void AddIfValid(int x, int y, List<(int, int)> candidates, int[,] worldEmpires, int maxX, int maxY)
        {
            if (x >= 0 && x < maxX && y >= 0 && y < maxY && worldEmpires[x, y] == 0)
            {
                candidates.Add((x, y));
            }
        }
    }
}
