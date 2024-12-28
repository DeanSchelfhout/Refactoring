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
            var w= buildWorld2.BuildWorld(maxy, maxx, coverage);
            world.Grid = w;
            world.Maxx = maxx;
            world.Maxy = maxy;
            world.Coverage = coverage;
            world.Name = "World" + (i + 1);
           
                collection.InsertOne(world);
            }

            List<World> worlds = collection.Find(FilterDefinition<World>.Empty).ToList();
            for (int i = 0; i < worlds.Count; i++)
            {
                var w = worlds[i].Grid;

                int nEmpires = new Random().Next(4, 8);
               

                IStrategy strategy3 = new Strategy3(w);
                IStrategy strategy2 = new Strategy2(w);
                IStrategy strategy1 = new Strategy1(w);

                int[,] ww = strategy3.Conquer(1, 25000);

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
                            empire.Strategy = strategy1;
                            break;
                        case 2:
                            empire.Strategy = strategy2;
                            break;
                        case 3:
                            empire.Strategy = strategy3;
                            break;
                    }
                }

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
                //for (int i = 0; i < ww.GetLength(1); i++)
                //{
                //    for (int j = 0; j < ww.GetLength(0); j++)
                //    {
                //        string ch;
                //        switch (ww[j, i])
                //        {
                //            case -1: ch = " "; break;
                //            case 0: ch = "."; break;
                //           default: ch = ww[j, i].ToString(); break;
                //      }
                //      Console.Write(ch);
                //    }
                //    Console.WriteLine();
                //}
                //BitmapWriter bmw = new BitmapWriter();
                //bmw.DrawWorld(ww);
            }
        }
    }
}
