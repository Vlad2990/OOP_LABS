using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop2
{
    public class DeleteCommand : ICommand
    {
        private readonly Document _document;
        private readonly int _top;
        private readonly int _left;
        private readonly int _count;
        private readonly List<FormatChar> _deletedText;

        public DeleteCommand(Document document, int top, int left, int count)
        {
            _document = document;
            _top = top;
            _left = left;
            _count = count;
            if (left + count >= 175) return;
            _deletedText = _document.ReadLine(top).GetRange(left, count);
        }

        public void Execute()
        {
            _document.Remove(_top, _left, _count);
        }

        public void Undo()
        {
            _document.Insert(_top, _left, _deletedText);
        }
    }
}
