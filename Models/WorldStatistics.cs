using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class WorldStatistics
    {
        [BsonId]
        public int WorldNumber { get; set; }
        public string WorldName{ get; set; }
        public List<Game> Games { get; set; }
    }
}
