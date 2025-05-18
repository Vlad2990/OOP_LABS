using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    internal class PlainText : ITextDecorator
    {
        public string GetFormattedText(char c)
        {
            return c.ToString();
        }
    }
}
