using oop2;
using System.Drawing.Drawing2D;

namespace oop2
{
    public class Document : ICloneable
    {
        private List<List<FormatChar>> lines = new();
        public string? name;
        public string? extension;
        public StorageType? located;
        public User Admin { get; private set; }

        public Dictionary<User, IRoleStrategy> Observers { get; } = new();

        public Document(string name, User admin)
        {
            this.name = name;
            Admin = admin;
            SomeChange += Admin.AddNews;
        }

        public string GetFullName()
        {
            return name + extension;
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
        public bool CanEdit(string name)
        {
            User user = Observers.Keys.FirstOrDefault(x => x.Name == name);
            if (name == Admin.Name) return true;
            return user != null ? Observers[user].CanEdit : throw new UnauthorizedAccessException();
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
            if (lines[top].Count == 0 && lines.Count > 1)
            {
                lines.RemoveAt(top);
            }
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

        public void Change(string news)
        {
            SomeChange.Invoke(news);
        }

        public void AddObserver(User admin, User user, IRoleStrategy role)
        {
            if (admin != Admin) throw new UnauthorizedAccessException();
            if (Observers.ContainsKey(user))
            {
                if (Observers[user] == role) return;
                Observers.Remove(user);
            }
            Observers.Add(user, role);
            SomeChange += user.AddNews;
        }

        public void RemoveObserver(User admin, User user)
        {
            if (admin != Admin) throw new UnauthorizedAccessException();
            if (!Observers.ContainsKey(user)) return;
            SomeChange -= user.AddNews;
            Observers.Remove(user);
        }

        public delegate void Changes(string news);
        public event Changes SomeChange;


        public object Clone()
        {
            var newDoc = new Document(name, Admin)
            {
                extension = extension,
                located = located
            };

            newDoc.lines = lines
                .Select(line => line
                    .Select(fc => new FormatChar(fc.c) { decs = [.. fc.decs] })
                    .ToList())
                .ToList();

            foreach (var observer in Observers)
            {
                newDoc.Observers.Add(observer.Key, observer.Value);
            }

            return newDoc;
        }
    }
}
