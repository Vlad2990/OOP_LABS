using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace oop2
{
    class PlainTextDoc : IFormatAdapter
    {
        public string Convert(Document document)
        {
            Regex AnsiEscapeRegex = new Regex(@"\x1B\[[0-9;]*[mK]");

            var content = AnsiEscapeRegex.Replace(document.Read(), string.Empty);
            return content.Replace("**", "").Replace("__", "");
        }
    }
}
