using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    class ItalicText : ITextDecorator
    {
        public string GetFormattedText(char c)
        {
            return $"\x1B[3m{c}\x1B[23m";
        }
    }
}
