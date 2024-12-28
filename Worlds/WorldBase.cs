using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Worlds
{
    public abstract class WorldBase
    {
        public Random random;
        public int maxRandom = 10;
        public int chanceExtra = 6;
        public int chanceLess = 3;

        protected WorldBase(Random random)
        {
            this.random = random;
        }

        public int build()
        {
            int x = random.Next(maxRandom);
            if (x > chanceExtra) return 1;
            if (x < chanceLess) return -1;
            return 0;
        }
    }
}
