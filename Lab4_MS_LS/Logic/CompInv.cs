using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_MS_LS.Logic
{
    class CompInv<T> : IComparer<T> where T : Priority_Operation
    {
        // Реализуем интерфейс IComparer<T>
        public int Compare(T x, T y)
        {
            if (x.priority < y.priority)
                return 1;
            if (x.priority > y.priority)
                return -1;
            return 0;
        }
    }

}
