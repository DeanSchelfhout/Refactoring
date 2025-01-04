using ConsoleAppSquareMaster.Strategies;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class World
    {
        public static void PrintWorld(World world)
        {
            for (int j = 0; j < world.Grid.GetLength(1); j++)
            {
                for (int x = 0; x < world.Grid.GetLength(0); x++)
                {
                    char ch;
                    if (world.Grid[x, j]) ch = '*'; else ch = ' ';
                    Console.Write(ch);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Printed world " + world.WorldNumber);
        }
        public static void PrintConqueredWorld(int[,] ww,int worldNumber)
        {
            for (int x = 0; x < ww.GetLength(1); x++)
            {
                for (int j = 0; j < ww.GetLength(0); j++)
                {
                    string ch;
                    switch (ww[j, x])
                    {
                        case -1: ch = " "; break;
                        case 0: ch = "."; break;
                        default: ch = ww[j, x].ToString(); break;
                    }
                    Console.Write(ch);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Printed conquered world " + worldNumber);
        }
        public static void PrintImage(int[,] ww, int worldNumber, int gameNumber)
        {
            BitmapWriter bmw = new BitmapWriter();
            bmw.DrawWorld(ww, worldNumber,gameNumber);
        }
        public async static Task<Game> SimulateAsync(World world,int gameNumber)
        {
            IStrategy strategy1 = new Strategy1();
            IStrategy strategy2 = new Strategy2();
            IStrategy strategy3 = new Strategy3();

            int[,] ww = null;

            double worldCount = 0;

            for (int y = 0; y < world.Grid.GetLength(0); y++)
            {
                for (int x = 0; x < world.Grid.GetLength(1); x++)
                {
                    if (world.Grid[y, x])
                    {
                        worldCount++;
                    }
                }
            }

            Game game = new Game();
            game.GameNumber = gameNumber;
            List<EmpireStatistics> empireStatisticsList = new List<EmpireStatistics>();

            for (int i = 0; i < world.Empires.Count; i++)
            {
                EmpireStatistics empireStatistics = new EmpireStatistics();
                empireStatistics.Strategy = world.Empires[i].Strategy;
                empireStatistics.Name = world.Empires[i].Name;
                empireStatistics.EmpireID = world.Empires[i].EmpireID;
                (int[,] worldEmpires, int x, int y) strat = default;
                switch (world.Empires[i].Strategy)
                {
                    case 1:
                         strat = strategy1.Conquer(world.Grid, world.Empires[i].EmpireID, 5000);
                        break;
                    case 2:
                         strat = strategy2.Conquer(world.Grid, world.Empires[i].EmpireID, 5000);
                        break;
                    case 3:
                        strat = strategy3.Conquer(world.Grid, world.Empires[i].EmpireID, 5000);
                        break;
                }
                ww = strat.worldEmpires;
                empireStatistics.StartPosition = (strat.x, strat.y);
                empireStatisticsList.Add(empireStatistics);
            }

            for (int i = 0; i < empireStatisticsList.Count; i++)
            {
                for (int j = 0; j < world.Empires.Count; j++)
                {
                    if (empireStatisticsList[i].EmpireID == world.Empires[j].EmpireID)
                    {
                        double conqueredCount = 0;
                        for (int y = 0; y < ww.GetLength(0); y++)
                        {
                            for (int x = 0; x < ww.GetLength(1); x++)
                            {
                                if (ww[y, x] == world.Empires[i].EmpireID)
                                {
                                    conqueredCount++;
                                }
                            }
                        }
                        double percentage = (conqueredCount / worldCount) * 100;
                        empireStatisticsList[i].WorldConquered = Math.Round(percentage, 2) + "%";
                        empireStatisticsList[i].CellsConquered = Convert.ToInt32(conqueredCount);
                    }
                }
            }

            game.Empires = empireStatisticsList;

            //PrintConqueredWorld(ww, world.WorldNumber);
            PrintImage(ww, world.WorldNumber,gameNumber);
            Console.WriteLine($"Done Simulating World {world.WorldNumber} Game {gameNumber}");
            return game;
        }

        [BsonId]
        public int WorldNumber { get; set; }
        public string Name { get; set; }
        public int Maxx { get; set; }
        public int Maxy { get; set; }
        public double Coverage { get; set; }
        public bool[,] Grid { get; set; }
        public int Algorythm { get; set; }
        public List<Empire> Empires { get; set; }
    }
}
