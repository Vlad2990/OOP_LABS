using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop1
{
    public class ColoredChar : ICloneable
    {
        public char c;
        public ConsoleColor color;
        public ColoredChar(char c, ConsoleColor color)
        {
            this.c = c;
            this.color = color;
        }
        public ColoredChar()
        {
            c = ' ';
            color = ConsoleColor.White;
        }
        public object Clone()
        {
            return new ColoredChar(c, color);
        }
    }
}
