using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    class BoldText : ITextDecorator
    {
        public string GetFormattedText(char c)
        {
            return $"\x1B[0m{c}\x1B[22m";
        }
    }
}
