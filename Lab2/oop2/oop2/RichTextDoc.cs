using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    class RichTextDoc : IFormatAdapter
    {
        public string Convert(Document document)
        {
            return document.Read();
        }
    }
}