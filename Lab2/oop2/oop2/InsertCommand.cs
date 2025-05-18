using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public class InsertCommand : ICommand
    {
        private readonly Document _document;
        private readonly int _top;
        private readonly int _left;
        private readonly int _count;
        private readonly List<FormatChar> _text;

        public InsertCommand(Document document, int top, int left, List<FormatChar> text)
        {
            _document = document;
            _top = top;
            _left = left;
            _text = text;
            _count = text.Count;
        }

        public void Execute()
        {
            _document.Insert(_top, _left, _text);
        }

        public void Undo()
        {
            _document.Remove(_top, _left, _count);
        }
    }
}
