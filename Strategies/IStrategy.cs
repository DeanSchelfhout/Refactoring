using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSquareMaster.Strategies
{
    public interface IStrategy
    {
        int[,] Conquer(int nEmpires, int turns);
    }
}
