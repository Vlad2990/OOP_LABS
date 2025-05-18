using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace oop2
{
    class MarkdownDoc : IFormatAdapter
    {
        public string Convert(Document document)
        {
            Regex AnsiEscapeRegex = new Regex(@"\x1B\[([0-9;]*)m");

            var result = AnsiEscapeRegex.Replace(document.Read(), match =>
            {
                var codes = match.Groups[1].Value.Split(';');
                var markdown = new StringBuilder();

                foreach (var code in codes)
                {
                    if (string.IsNullOrEmpty(code)) continue;

                    switch (code)
                    {
                        case "1":
                            markdown.Append("**");
                            break;
                        case "3":
                            markdown.Append("*");
                            break;
                        case "4":
                            markdown.Append("__");
                            break;
                        case "22":
                            markdown.Append("**");
                            break;
                        case "23":
                            markdown.Append("*");
                            break;
                        case "24":
                            markdown.Append("__");
                            break;
                    }
                }

                return markdown.ToString();
            });

            return result;
        }
    }
}