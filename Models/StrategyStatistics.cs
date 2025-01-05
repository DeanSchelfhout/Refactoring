using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class StrategyStatistics
    {
        [BsonId]
        public int Strategy { get; set; }
        public int AVGCellsConquered { get; set; }
        public string AVGWorldConquered { get; set; }
    }
}
