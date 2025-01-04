using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class Empire
    {
        [BsonId]
        public int EmpireID { get; set; }
        public string Name { get; set; }
        public int Strategy { get; set; }
        public bool[,] StartPosition { get; set; }
        public int Coverage { get; set; }
        public double WorldCoverage { get; set; }
    }
}
