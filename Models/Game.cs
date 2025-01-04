using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Models
{
    public class Game
    {
        [BsonId]
        public int GameNumber{ get; set; }
        public List<EmpireStatistics> Empires{ get; set; }
    }
}
