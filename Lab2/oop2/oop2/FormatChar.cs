using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public class FormatChar
    {
        public List<ITextDecorator> decs = new();
        public char c { get; private set; }
        public FormatChar(char c)
        {
            this.c = c;
            decs.Add(new PlainText());
        }
        public void AddDec(ITextDecorator dec) => decs.Add(dec);

        public string GetChar()
        {
            string ch = "";
            foreach (var decorator in decs)
                ch = decorator.GetFormattedText(c);
            return ch;
        }
    }
}
