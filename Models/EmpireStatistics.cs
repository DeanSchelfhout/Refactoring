using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class EmpireStatistics
    {
        [BsonId]
        public int EmpireID { get; set; }
        public string Name { get; set; }
        public int Strategy { get; set; }
        public (int x, int y) StartPosition { get; set; }
        public int CellsConquered { get; set; }
        public string WorldConquered { get; set; }
       
    }
}
