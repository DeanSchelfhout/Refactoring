using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class World
    {
        [BsonId]
        public int WorldNumber { get; set; }
        public string Name { get; set; }
        public int Maxx { get; set; }
        public int Maxy { get; set; }
        public double Coverage { get; set; }
        public bool[,] Grid { get; set; }
    }
}
