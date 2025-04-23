using oop2;
using System.Drawing.Drawing2D;

namespace oop2
{
    public class Document
    {
        private List<List<FormatChar>> lines = new();
        public string? name;
        public List<DocumentEditRecord> History { get; } = new();

        public Document(string name)
        {
            this.name = name;
        }

        public Document()
        {

        }

        public void Write(string text)
        {
            lines = SplitToLines(text);
        }

        public void ReWrite(string text)
        {
            lines.Clear();
            lines = SplitToLines(text);
        }

        public void Edit(string name)
        {
            History.Add(new DocumentEditRecord(name));
        }

        public int MaxLine() => lines.Count;

        public int MaxLeft(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= lines.Count) return 0;
            return lines[lineIndex].Count;
        }

        public string Read()
        {
            var result = new System.Text.StringBuilder();
            foreach (var line in lines)
            {
                foreach (var formatChar in line)
                {
                    if (formatChar.c != '\n')
                    {
                        result.Append(formatChar.GetChar());
                    }
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public List<FormatChar> ReadLine(int top)
        {
            if (top < 0 || top >= lines.Count) return new List<FormatChar>();
            return lines[top];
        }
        public void Insert(int top, int left, List<FormatChar> formatChars)
        {
            if (top < 0 || top > lines.Count) return;
            if (left < 0) return;
            if (formatChars == null || formatChars.Count == 0) return;

            if (top == lines.Count)
            {
                lines.Add(new List<FormatChar>());
            }

            if (left > lines[top].Count) return;

            foreach (var formatChar in formatChars)
            {
                if (formatChar.c == '\n')
                {
                    var remainingChars = lines[top].Skip(left).ToList();
                    lines[top].RemoveRange(left, lines[top].Count - left);
                    lines.Insert(top + 1, remainingChars);
                    return;
                }

                if (left > 0 && lines[top].Count > 0 && left <= lines[top].Count)
                {
                    var prevChar = lines[top][left - 1];
                    if (prevChar.c == '\n')
                    {
                        continue;
                    }
                }

                lines[top].Insert(left, formatChar);
                left++;
            }
        }

        public void Remove(int top, int left, int length)
        {
            if (top < 0 || top >= lines.Count) return;
            if (left < 0 || left >= lines[top].Count) return;

            lines[top].RemoveRange(left, length);
        }

        private List<List<FormatChar>> SplitToLines(string text)
        {
            var result = new List<List<FormatChar>>();
            var currentLine = new List<FormatChar>();

            foreach (char c in text)
            {
                if (c == '\n')
                {
                    result.Add(currentLine);
                    currentLine = new List<FormatChar>();
                }
                else
                {
                    currentLine.Add(new FormatChar(c));
                }
            }
            result.Add(currentLine);

            return result;
        }
    }
}