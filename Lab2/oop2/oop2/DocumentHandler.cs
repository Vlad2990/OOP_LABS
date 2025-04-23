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
        public DocumentHandler()
        {
            users.AddRange([new User("Admin", new AdminStrategy()),
                            new User("Editor", new EditorStrategy()),
                            new User("Viewer", new ViewerStrategy())]);
            if (!File.Exists("test.txt"))
            {
                File.Create("test.txt");
            }
            docs.Add(new Document("test.txt"));
        }
            

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

        public bool ChangeRole(string name, IRoleStrategy role)
        {
            foreach (var user in users)
                if (user.Name == name)
                {
                    return user.ChangeRole(role, currUser);
                }
            return false;
        }
        public void SetConsole()
        {
            Console.Clear();
            Console.Write(currDoc.Read());
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
        public void Save(int type, StorageType storageType)
        {
            Console.Clear();
            var strategy = StorageFactory.CreateStrategy(storageType);
            Console.WriteLine("Input file name");
            string filename = Console.ReadLine();
            IFormatAdapter adapter = null;
            switch (type)
            {
                case 1:
                    filename += ".txt";
                    adapter = new PlainTextDoc();
                    break;

                case 2:
                    filename += ".md";
                    adapter = new MarkdownDoc();
                    break;

                case 3:
                    filename += ".rtf";
                    adapter = new RichTextDoc();
                    break;
            }
            string content = adapter.Convert(currDoc);
            string old = content;
            if (storageType == StorageType.LocalFile)
            {
                try
                {
                    currDoc = DocExist(filename);
                }
                catch (FileNotFoundException)
                {
                    File.Create(filename).Close();
                    Document doc = new Document(filename);
                    doc.Write(content);
                    docs.Add(doc);
                    currDoc = doc;
                }
                finally
                {
                    currDoc.Edit(currUser.Name);
                    strategy.Save(filename, content);

                    currDoc.ReWrite(content);
                    Console.Clear();
                    Console.Write(old);
                    
                }
                SetConsole();
                return;
            }
            strategy.Save(filename, content);
            Console.Clear();
            Console.Write(old);
        }
        public List<FormatChar> GetTextRange(int top, int left, int length)
        {
            return currDoc.ReadLine(top).Skip(left).Take(length).ToList();
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
        public List<string> GetHistory(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();
            List<string> history = new List<string>();
            foreach (var his in currDoc.History) history.Add(his.ToString());
            return history;
        }
        public void Open(string filename, StorageType storageType)
        {
            if (storageType == StorageType.LocalFile)
            {
                currDoc = DocExist(filename);
                return;
            }
            var strategy = StorageFactory.CreateStrategy(storageType);
            var content = strategy.Load(filename);
            Document doc = new Document(filename);
            doc.Write(content);
        }
        public Document DocExist(string filename)
        {
            foreach(var doc in docs)
            {
                if (doc.name == filename)
                    return doc;
            }
            throw new FileNotFoundException();
        }
        public void Open()
        {
            Document document = new();
            currDoc = document;
        }
        public void Remove(string filename)
        {
            DocExist(filename);
            foreach(var doc in docs)
            {
                docs.Remove(doc);
            }
            File.Delete(filename);
        }

    }
}
