using ConsoleAppSquareMaster.Models;
using ConsoleAppSquareMaster.Strategies;
using ConsoleAppSquareMaster.Worlds;
using MongoDB.Driver;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleAppSquareMaster
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("SquareMaster");

            var worldCollection = database.GetCollection<World>("World");
            await worldCollection.DeleteManyAsync(FilterDefinition<World>.Empty);

            var worldStatisticsCollection = database.GetCollection<WorldStatistics>("WorldStatistics");
            await worldStatisticsCollection.DeleteManyAsync(FilterDefinition<WorldStatistics>.Empty);

            var StrategyStatisticsCollection = database.GetCollection<StrategyStatistics>("StrategyStatistics");
            await StrategyStatisticsCollection.DeleteManyAsync(FilterDefinition<StrategyStatistics>.Empty);

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                int worldNumber = i;

                tasks.Add(Task.Run(async () =>
                {
                    World world = new World();
                    world = await GenerateWorldAsync(worldNumber);
                    world.Empires = await GenerateEmpiresAsync();
                    await worldCollection.InsertOneAsync(world);
                    Console.WriteLine("Done generating world " + world.WorldNumber);
                    //World.PrintWorld(world);

                    List<Task<Game>> gameTasks = new List<Task<Game>>();
                    for (int j = 1; j <= 3; j++)
                    {
                        int gameNumber = j;
                        gameTasks.Add(World.SimulateAsync(world, gameNumber));
                    }

                    Game[] games = await Task.WhenAll(gameTasks);

                    WorldStatistics worldStatistics = new WorldStatistics();
                    worldStatistics.WorldNumber = world.WorldNumber;
                    worldStatistics.WorldName = world.Name;
                    worldStatistics.Games = games.ToList();

                    await worldStatisticsCollection.InsertOneAsync(worldStatistics);
                }));
            }

            await Task.WhenAll(tasks);

            List<StrategyStatistics> strategyStatistics = new List<StrategyStatistics>();
            StrategyStatistics strategy1Statistics = new StrategyStatistics();
            StrategyStatistics strategy2Statistics = new StrategyStatistics();
            StrategyStatistics strategy3Statistics = new StrategyStatistics();

            var statistics = World.GetStrategyStatistics();
            strategy1Statistics.AVGCellsConquered = statistics.c1 / 30;
            strategy1Statistics.CellsConquered = statistics.c1;
            strategy1Statistics.AVGWorldConquered = $"{Math.Round((statistics.w1 / 30), 2)}%";
            strategy2Statistics.AVGCellsConquered = statistics.c2 / 30;
            strategy2Statistics.CellsConquered = statistics.c2;
            strategy2Statistics.AVGWorldConquered = $"{Math.Round((statistics.w2 / 30), 2)}%";
            strategy3Statistics.AVGCellsConquered = statistics.c3 / 30;
            strategy3Statistics.CellsConquered = statistics.c3;
            strategy3Statistics.AVGWorldConquered = $"{Math.Round((statistics.w3 / 30), 2)}%";

            strategy1Statistics.Strategy = 1;
            strategy2Statistics.Strategy = 2;
            strategy3Statistics.Strategy = 3;

            strategyStatistics.Add(strategy1Statistics);
            strategyStatistics.Add(strategy2Statistics);
            strategyStatistics.Add(strategy3Statistics);

            await StrategyStatisticsCollection.InsertManyAsync(strategyStatistics);

            Console.WriteLine("Program Complete!");
        }

        static async Task<List<Empire>> GenerateEmpiresAsync()
        {
            int nEmpires = new Random().Next(4, 9);

            List<Empire> empires = new List<Empire>();
            for (int i = 0; i < nEmpires; i++)
            {
                Empire empire = new Empire();
                empire.EmpireID = i + 1;
                empire.Name = "Empire" + (i + 1);

                int s = new Random().Next(1, 4);

                switch (s)
                {
                    case 1:
                        empire.Strategy = 1;
                        break;
                    case 2:
                        empire.Strategy = 2;
                        break;
                    case 3:
                        empire.Strategy = 3;
                        break;
                }
                empires.Add(empire);
            }
            return empires;
        }

        static async Task<World> GenerateWorldAsync(int wNumber)
        {
            World world = new World();
            world.WorldNumber = wNumber + 1;
            Random random = new Random(10);
            IBuildWorld buildWorld1 = new BuildWorld1(random);
            IBuildWorld buildWorld2 = new BuildWorld2(random);

            double coverage = new Random().NextDouble();
            int maxy = new Random().Next(50, 150);
            int maxx = new Random().Next(50, 150);

            int worldbuilder = new Random().Next(1, 3);
            bool[,] w;
            if (worldbuilder == 1)
            {
                w = buildWorld1.BuildWorld(maxy, maxx, coverage);
            }
            else
            {
                w = buildWorld2.BuildWorld(maxy, maxx, coverage);
            }
            world.Grid = w;
            world.Maxx = maxx;
            world.Maxy = maxy;
            world.Coverage = coverage;
            world.Name = "World" + (wNumber + 1);
            world.Algorythm = worldbuilder;

            return world;
        }
    }
}
