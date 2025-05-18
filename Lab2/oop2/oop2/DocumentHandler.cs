using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TextCopy;

namespace oop2
{
    public class DocumentHandler
    {
        Document currDoc = null;
        CommandManager commandManager = new();
        User currUser = null;
        List<User> users = new List<User>();
        List<Document> docs = new List<Document>();
        List<FormatChar> clipboard = new List<FormatChar>();

        public bool SetUser(string name)
        {
            foreach (var user in users)
                if (user.Name == name)
                {
                    currUser = user;
                    return true;
                }
            return false;
        }

        public User GetUser()
        {
            return currUser;
        }
        public User GetUser(string name)
        {
            foreach (var user in users)
                if (user.Name == name)
                {
                    return user;
                }
            return null;
        }
        public bool UserExist(string name)
        {
            foreach (var user in users)
                if (user.Name == name)
                {
                    return true;
                }
            return false;
        }
        public List<string> GetFiles()
        {
            return [.. docs.Where(doc => doc.Admin == currUser).Select(doc => doc.GetFullName() + " in " + doc.located)];
        }

        public void KillUnsave()
        {
            if (string.IsNullOrEmpty(currDoc.extension))
                docs.Remove(currDoc);
        }
        public bool AddUser(string name)
        {
            if (UserExist(name)) return false;

            users.Add(new User(name));
            return true;
        }
        public string GetText()
        {
            return currDoc.Read();
        }
        public int MaxLine()
        {
            return currDoc.MaxLine();
        }
        public int MaxLeft(int top)
        {
            return currDoc.MaxLeft(top);
        }
        public void InsertText(char text, int top, int left, bool IsBold = false, bool IsItalic = false, bool IsUnderline = false)
        {
            var ch = new FormatChar(text);
            if (IsBold) ch.AddDec(new BoldText());
            if (IsItalic) ch.AddDec(new ItalicText());
            if (IsUnderline) ch.AddDec(new UnderlineText());

            var command = new InsertCommand(currDoc, top, left, new List<FormatChar> { ch });
            commandManager.AddCommand(command);
        }
        public void InsertText(List<FormatChar> text, int top, int left)
        {
            var command = new InsertCommand(currDoc, top, left, text);
            commandManager.AddCommand(command);
        }

        public void DeleteText(int top, int left, int length)
        {
            var command = new DeleteCommand(currDoc, top, left, length);
            commandManager.AddCommand(command);
        }
        public void Undo()
        {
            commandManager.Undo();
        }
        public void Redo()
        {
            commandManager.Redo();
        }
        public void Save(string filename, int type, StorageType storageType)
        {
            Console.Clear();
            var strategy = StorageFactory.CreateStrategy(storageType);
            string ext = "";
            IFormatAdapter adapter = null;
            switch (type)
            {
                case 1:
                    ext = ".txt";
                    adapter = new PlainTextDoc();
                    break;

                case 2:
                    ext = ".md";
                    adapter = new MarkdownDoc();
                    break;

                case 3:
                    ext = ".rtf";
                    adapter = new RichTextDoc();
                    break;
            }
            string content = adapter.Convert(currDoc);
            Document doc = null;
            if ((currDoc.GetFullName() != filename + ext && !string.IsNullOrEmpty(currDoc.extension)) || (currDoc.located != storageType && currDoc.located != null))
            {
                doc = new Document(filename, currUser);
                doc.Write(content);
                docs.Add(doc);
                currDoc = doc;
            }
            else
            {
                docs.RemoveAll(d => d.GetFullName() == filename + ext && d.located == storageType);

                doc = (Document)currDoc.Clone();
                docs.Add(doc);
            }
            doc.name = filename;
            doc.extension = ext;
            doc.located = storageType;
            strategy.Save(doc.GetFullName(), content);
            string news = "File changed by" + currUser.Name;
            doc.Change(news);
        }

        public void Create()
        {
            Document doc = new Document("", currUser);
            currDoc = doc;
        }
        public List<FormatChar> GetTextRange(int top, int left, int length)
        {
            return currDoc.ReadLine(top).Skip(left).Take(length).ToList();
        }

        public bool GiveAccess(string filename, string name, IRoleStrategy role)
        {
            User user = GetUser(name);

            if (user != null)
            {
                Document document = DocExist(filename);
                if (role == null)
                {
                    document.RemoveObserver(currUser, user);
                    return true;
                }
                document.AddObserver(currUser, GetUser(name), role);
                return true;
            }
            else return false;
        }

        public void Cut(int top, int left, int length)
        {
            clipboard = currDoc.ReadLine(top).GetRange(left, length);

            DeleteText(top, left, length);
        }
        public void Copy(int top, int left, int length)
        {
            clipboard = currDoc.ReadLine(top).GetRange(left, length);
        }
        public void Paste(int top, int left)
        {
            if (clipboard == null || clipboard.Count == 0) return;
            InsertText(clipboard, top, left);
        }
        
        public bool Open(string filename, StorageType storageType)
        {
            var strategy = StorageFactory.CreateStrategy(storageType);
            try
            {
                strategy.Load(filename);
            }
            catch 
            {
                return false;
            }
            currDoc = (Document)docs.Find(d => d.GetFullName() == filename && d.located == storageType).Clone();
            return true;
        }
        public bool Edit()
        {
            bool can = false;
            try
            {
                can = currDoc.CanEdit(currUser.Name);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            if (can) return true;
            else throw new UnauthorizedAccessException();
        }
        public Document DocExist(string filename)
        {
            Document doc = docs.Find(d => d.GetFullName() == filename);
            return doc == null ? throw new FileNotFoundException() : doc;
        }
        public bool Remove(string filename, StorageType storageType)
        {
            DocExist(filename);
            int count = docs.Count;
            foreach(var doc in docs)
            {
                if (doc.GetFullName() == filename && doc.located == storageType)
                {
                    docs.Remove(doc);
                    break;
                }
            }
            if (count == docs.Count) return false;
            var strategy = StorageFactory.CreateStrategy(storageType);
            strategy.Delete(filename);
            return true;
        }

        public List<string> GetNews()
        {
            return currUser.News;
        }
    }
}
