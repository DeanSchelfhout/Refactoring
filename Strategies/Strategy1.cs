using ConsoleAppSquareMaster;
using System;
using System.Collections.Generic;

namespace ConsoleAppSquareMaster.Strategies;
public class Strategy1 : IStrategy
{
    private Random random = new Random(1);

    public int[,] Conquer(bool[,] world, int empireID, int turns)
    {
        int maxX = world.GetLength(0);
        int maxY = world.GetLength(1);
        int[,] worldEmpires = InitializeWorldEmpires(world, maxX, maxY);
        Dictionary<int, List<(int, int)>> empires = InitializeEmpires(world, empireID, worldEmpires);

        for (int t = 0; t < turns; t++)
        {
            foreach (var empire in empires)
            {
                int index = random.Next(empire.Value.Count);
                (int x, int y) = empire.Value[index];
                ExpandEmpire(worldEmpires, empires, empire.Key, x, y, maxX, maxY);
            }
        }

        return worldEmpires;
    }

    private void ExpandEmpire(int[,] worldEmpires, Dictionary<int, List<(int, int)>> empires, int empireId, int x, int y, int maxX, int maxY)
    {
        int direction = random.Next(4);
        switch (direction)
        {
            case 0: if (x < maxX - 1 && worldEmpires[x + 1, y] == 0) { AddCell(worldEmpires, empires, empireId, x + 1, y); } break;
            case 1: if (x > 0 && worldEmpires[x - 1, y] == 0) { AddCell(worldEmpires, empires, empireId, x - 1, y); } break;
            case 2: if (y < maxY - 1 && worldEmpires[x, y + 1] == 0) { AddCell(worldEmpires, empires, empireId, x, y + 1); } break;
            case 3: if (y > 0 && worldEmpires[x, y - 1] == 0) { AddCell(worldEmpires, empires, empireId, x, y - 1); } break;
        }
    }

    private void AddCell(int[,] worldEmpires, Dictionary<int, List<(int, int)>> empires, int empireId, int x, int y)
    {
        worldEmpires[x, y] = empireId;
        empires[empireId].Add((x, y));
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
}
