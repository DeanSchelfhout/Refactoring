using ConsoleAppSquareMaster.Models;
using ConsoleAppSquareMaster.Strategies;
using ConsoleAppSquareMaster.Worlds;
using MongoDB.Driver;

namespace ConsoleAppSquareMaster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("SquareMaster");
            var collection = database.GetCollection<World>("World");
            collection.DeleteMany(FilterDefinition<World>.Empty);


            for (int i = 0; i < 10; i++)
            {
            World world = new World();
            world.WorldNumber = i +1;
            Random random = new Random(10);
            IBuildWorld buildWorld1 = new BuildWorld1(random);
            IBuildWorld buildWorld2 = new BuildWorld2(random);

            double coverage = new Random().NextDouble();
            int maxy = new Random().Next(50,150);
            int maxx = new Random().Next(50, 150);

            int worldbuilder = new Random().Next(1,3);
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
            world.Name = "World" + (i + 1);
            world.Algorythm = worldbuilder;
           
            collection.InsertOne(world);
            }

            List<World> worlds = collection.Find(FilterDefinition<World>.Empty).ToList();
            for (int i = 0; i < worlds.Count; i++)
            {
                var w = worlds[i].Grid;
                int[,] ww = new int[w.GetLength(0), w.GetLength(1)];
                for (int x = 0; x < w.GetLength(0); x++)
                {
                    for (int j = 0; j < w.GetLength(1); j++)
                    {
                        ww[x, j] = (w[x, j]) ? 1 : -1;
                    }
                }
                int nEmpires = new Random().Next(3, 6);

                IStrategy strategy3 = new Strategy3();
                IStrategy strategy2 = new Strategy2();
                IStrategy strategy1 = new Strategy1();

                List<Empire> empires = new List<Empire>();
                for (int j = 0; j < nEmpires; j++)
                {
                    Empire empire = new Empire();
                    empire.EmpireID = j+1;
                    empire.Name = "Empire"+(j+1);

                    int s = new Random().Next(1, 3);

                    switch (s)
                    {
                        case 1:
                            empire.Strategy = 1;
                            ww = strategy1.Conquer(w,empire.EmpireID,5000);
                            break;
                        case 2:
                            empire.Strategy = 2;
                            ww = strategy2.Conquer(w, empire.EmpireID, 5000);
                            break;
                        case 3:
                            empire.Strategy = 3;
                            ww = strategy3.Conquer(w, empire.EmpireID, 5000);
                            break;
                    }
                    empires.Add(empire);
                }
                worlds[i].Empires = empires;
                //To print the world to Console:
                //for (int j = 0; j < w.GetLength(1); j++)
                //{
                //    for (int x = 0; x < w.GetLength(0); x++)
                //    {
                //        char ch;
                //        if (w[x, j]) ch = '*'; else ch = ' ';
                //        Console.Write(ch);
                //    }
                //    Console.WriteLine();
                //}

                //To print the conquered world to console + jpg:
                //for (int x = 0; x < ww.GetLength(1); x++)
                //{
                //    for (int j = 0; j < ww.GetLength(0); j++)
                //    {
                //        string ch;
                //        switch (ww[j, x])
                //        {
                //            case -1: ch = " "; break;
                //            case 0: ch = "."; break;
                //            default: ch = ww[j, x].ToString(); break;
                //        }
                //        Console.Write(ch);
                //    }
                //    Console.WriteLine();
                //}
                BitmapWriter bmw = new BitmapWriter();
                bmw.DrawWorld(ww, worlds[i].WorldNumber);
                Console.WriteLine("Done simulating world " + worlds[i].WorldNumber);
            }
        }
    }
}
