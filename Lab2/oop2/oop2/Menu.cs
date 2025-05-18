using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace oop2
{
    public class Menu
    {
        int left = 0;
        int top = 0;
        int clipLeft = 0;
        int clipRight = 0;
        int clipStart = 0;
        bool IsBold = false;
        bool IsItalic = false;
        bool IsUnderline = false;
        private DocumentHandler documentHandler = new();
        private SettingsManager settingsManager = SettingsManager.Instance;
        public Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Add user");
                Console.WriteLine("2 - Log in");
                Console.WriteLine("Esc - End");

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        AddUser();
                        break;
                    case ConsoleKey.D2:
                        UserMenu();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private void AddUser()
        {
            Console.Clear();
            Console.WriteLine("Write user name");
            string? username = Console.ReadLine();
            if (!documentHandler.AddUser(username))
            {
                Console.WriteLine("There is already a user with that name. Press any button");
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("User has been added. Press any button");
                Console.ReadKey();
                return;
            }
        }
        
        private void UserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Input user name or write '1' to end");
                string userName = Console.ReadLine();
                if (userName == "1") return;
                if (!documentHandler.SetUser(userName))
                {
                    Console.WriteLine("Wrong name");
                    Console.ReadKey(true);
                    continue;
                }
                StartMenu();
            }
        }

        private void StartMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintStartMenu();
                var key = Console.ReadKey(true);
                string filename = string.Empty;
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        OpenFile();
                        break;

                    case ConsoleKey.D2:
                        Console.Clear();
                        CreateEmpty();
                        break;

                    case ConsoleKey.D3:
                        Console.Clear();
                        Delete();
                        break;

                    case ConsoleKey.D4:
                        Console.Clear();
                        ChangeRole();
                        break;

                    case ConsoleKey.D5:
                        Console.Clear();
                        ViewNews();
                        break;

                    case ConsoleKey.D6:
                        ChangeColor();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private void ViewNews()
        {
            var news = documentHandler.GetNews();
            if (news.Count == 0)
            {
                Console.WriteLine("There are no news. Press any button");
                Console.ReadKey(true);
                return;
            }
            foreach(var n in news)
            {
                Console.WriteLine(n);
            }
            Console.WriteLine();
            Console.WriteLine("Press any button");
            Console.ReadKey(true);
            return;
        }

        private void PrintStartMenu()
        {
            Console.WriteLine("Hey, " + documentHandler.GetUser().Name + '!');
            Console.WriteLine("1 - Open");
            Console.WriteLine("2 - Create new empty");
            Console.WriteLine("3 - Delete");
            Console.WriteLine("4 - Give access to file");
            Console.WriteLine("5 - View file history");
            Console.WriteLine("6 - Change theme");
            Console.WriteLine("Esc - Change user");
        }
        private void CreateEmpty()
        {
            Console.Clear();
            documentHandler.Create();
            Edit();
        }
        private void OpenFile()
        {
            while (true)
            {
                var files = documentHandler.GetFiles();
                if (files.Count != 0)
                {
                    Console.WriteLine("Your files:");
                    foreach (var file in files)
                        Console.WriteLine(file);
                    Console.WriteLine();
                }
                Console.WriteLine("Input file name");
                string filename = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Load from:");
                Console.WriteLine("1 - Local");
                Console.WriteLine("2 - Db");
                Console.WriteLine("3 - Cloud");
                bool stop = false;
                StorageType type = StorageType.LocalFile;
                while (!stop)
                {

                    ConsoleKeyInfo key2 = new();
                    try
                    {
                        key2 = Console.ReadKey(true);
                    }
                    catch (System.InvalidOperationException)
                    {
                        continue;
                    }
                    switch (key2.Key)
                    {
                        case ConsoleKey.D1:
                            type = StorageType.LocalFile;
                            stop = true;
                            break;

                        case ConsoleKey.D2:
                            type = StorageType.Db;
                            stop = true;
                            break;

                        case ConsoleKey.D3:
                            type = StorageType.Firebase;
                            stop = true;
                            break;

                        case ConsoleKey.Escape:
                            return;
                    }
                }
                try
                {
                    documentHandler.DocExist(filename);
                }
                catch (FileNotFoundException) 
                {
                    Console.WriteLine("File not found. Press 1 to input another name or any diff key to stop");
                    if (Console.ReadKey(true).Key != ConsoleKey.D1)
                    {
                        break;
                    }
                }
                if(!documentHandler.Open(filename, type))
                {
                    Console.WriteLine("There is no file with that name in " + type.ToString());
                    Console.WriteLine("Press any button");
                    Console.ReadKey(true);
                    return;
                }
                try
                {
                    if (!documentHandler.Edit())
                    {
                        Console.WriteLine("You cant view or edit this file. Press any button");
                        Console.ReadKey(true);
                        return;
                    }
                    Edit();
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    View();
                    return;
                }
            }
        }

        private void Edit()
        {
            top = 0;
            left = 0;
            Console.SetCursorPosition(left, top);
            SetConsole();
            while (true)
            {
                ConsoleKeyInfo key = new();
                try
                {
                    key = Console.ReadKey(true);
                }
                catch (System.InvalidOperationException)
                {
                    continue;
                }

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow when key.Modifiers == ConsoleModifiers.Shift:
                        if (left == 0)
                        {
                            break;
                        }
                        if (clipLeft == clipRight && clipLeft == clipStart)
                        {
                            clipStart = left;
                        }
                        left--;
                        if (left > clipStart) clipRight--;
                        else clipLeft--;
                            break;
                    case ConsoleKey.RightArrow when key.Modifiers == ConsoleModifiers.Shift:
                        if (left >= documentHandler.MaxLeft(top) && top == documentHandler.MaxLine() - 1) break;
                        if (clipLeft == clipRight && clipLeft == clipStart)
                        {
                            clipStart = left;
                        }
                        left++;
                        if (left > clipStart) clipRight++;
                        else clipLeft++;
                        break;
                    case ConsoleKey.X when key.Modifiers == ConsoleModifiers.Shift:
                        if (clipLeft != clipRight)
                        {
                            int start = Math.Min(clipLeft, clipRight);
                            int end = Math.Max(clipLeft, clipRight);
                            int length = end - start;

                            documentHandler.Cut(top, start, length);
                            SetConsole();

                            clipLeft = clipRight = clipStart = left = start;
                        }
                        break;
                    case ConsoleKey.C when key.Modifiers == ConsoleModifiers.Shift:
                        if (clipLeft != clipRight)
                        {
                            int start = Math.Min(clipLeft, clipRight);
                            int end = Math.Max(clipLeft, clipRight);
                            int length = end - start;

                            documentHandler.Copy(top, start, length);

                            clipLeft = clipRight = clipStart = left = start;
                        }
                        break;
                    case ConsoleKey.V when key.Modifiers == ConsoleModifiers.Shift:
                        documentHandler.Paste(top, left);
                        SetConsole();
                        break;
                    case ConsoleKey.UpArrow:
                        if (top > 0) top--;
                        clipLeft = clipRight = clipStart = left;
                        break;
                    case ConsoleKey.DownArrow:
                        if (top < Console.WindowHeight - 1 && top < documentHandler.MaxLine() - 1)
                        {
                            if (documentHandler.MaxLine() == top + 1 && left > documentHandler.MaxLeft(top) - 1)
                            {
                                left = documentHandler.MaxLeft(top);
                            }
                            top++;
                        }
                        clipLeft = clipRight = clipStart = left;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (left > 0) left--;
                        else if (left == 0 && top > 0)
                        {
                            top--;
                            left = 175;
                        }
                        clipLeft = clipRight = clipStart = left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (left == Console.WindowWidth - 1 && top != documentHandler.MaxLine() - 1)
                        {
                            left = 0;
                            top++;
                            break;
                        }
                        else if (left >= documentHandler.MaxLeft(top) && top == documentHandler.MaxLine() - 1) break;
                        left++;
                        clipLeft = clipRight = clipStart = left;
                        break;

                    case ConsoleKey.D1 when key.Modifiers == ConsoleModifiers.Shift:
                        if(IsBold)
                        {
                            IsBold = false;
                            break;
                        }
                        IsBold = true;
                        break;

                    case ConsoleKey.D2 when key.Modifiers == ConsoleModifiers.Shift:
                        if (IsItalic)
                        {
                            IsItalic = false;
                            break;
                        }
                        IsItalic = true;
                        break;
                    case ConsoleKey.D3 when key.Modifiers == ConsoleModifiers.Shift:
                        if (IsUnderline)
                        {
                            IsUnderline = false;
                            break;
                        }
                        IsUnderline = true;
                        break;

                    case ConsoleKey.S when key.Modifiers == ConsoleModifiers.Shift:
                        Save();
                        break;

                    case ConsoleKey.Z when key.Modifiers == ConsoleModifiers.Control:
                        documentHandler.Undo();
                        SetConsole();
                        break;

                    case ConsoleKey.Y when key.Modifiers == ConsoleModifiers.Control:
                        documentHandler.Redo();
                        SetConsole();
                        break;

                    case ConsoleKey.Delete:
                        if (documentHandler.MaxLeft(top) == left) break;
                        documentHandler.DeleteText(top, left, 1);
                        SetConsole();
                        break;
                    case ConsoleKey.Backspace:
                        if (left == 0 && top == 0) break;
                        else if (left == 0 && top > 0)
                        {
                            left = 175;
                            top--;
                        }
                        else left--;
                        documentHandler.DeleteText(top, left, 1);
                        SetConsole();
                        clipLeft = clipRight = clipStart = left;
                        break;

                    case ConsoleKey.Tab:
                        for (int i = 0; i < 4; ++i)
                        {
                            documentHandler.InsertText(' ', top, left, IsBold, IsItalic, IsUnderline);
                            if (left == 175)
                            {
                                left = 0;
                                top++;
                            }
                            else left++;
                            SetConsole();

                            clipLeft = clipRight = clipStart = left;
                        }
                        break;

                    case ConsoleKey.Enter:
                        documentHandler.InsertText('\n', top, left);
                        top++;
                        left = 0;
                        clipLeft = clipRight = clipStart = left;
                        break;
                    case ConsoleKey.Escape:
                        documentHandler.KillUnsave();
                        return;
                    default:
                        documentHandler.InsertText(key.KeyChar, top, left, IsBold, IsItalic, IsUnderline);
                        if (left == 175)
                        {
                            left = 0;
                            top++;
                        }
                        else left++;
                        SetConsole();

                        clipLeft = clipRight = clipStart = left;
                        break;
                }
                if (top >= documentHandler.MaxLine() && top != 0) top = documentHandler.MaxLine() - 1;
                if (left >= documentHandler.MaxLeft(top)) left = documentHandler.MaxLeft(top);
                Console.SetCursorPosition(left, top);
            }
        }
        private void View()
        {
            while (true)
            {
                SetConsole();
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape) return;
            }
        }

        private void ChangeColor()
        {
            Console.Clear();
            Console.WriteLine("1 - Set black");
            Console.WriteLine("2 - Set red");
            Console.WriteLine("3 - Set white");
            Console.WriteLine("4 - Set blue");
            Console.WriteLine("5 - Set green");

            
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        settingsManager.SetColor(ConsoleColor.Black);
                        return;
                    case ConsoleKey.D2:
                        settingsManager.SetColor(ConsoleColor.Red);
                        return;
                    case ConsoleKey.D3:
                        settingsManager.SetColor(ConsoleColor.White);
                        return;
                    case ConsoleKey.D4:
                        settingsManager.SetColor(ConsoleColor.Blue);
                        return;
                    case ConsoleKey.D5:
                        settingsManager.SetColor(ConsoleColor.Green);
                        return;

                }
            }
        }    
        private void ChangeRole()
        {
            while (true)
            {
                Console.Clear();
                var files = documentHandler.GetFiles();
                if (files.Count == 0)
                {
                    Console.WriteLine("You don't have any files. Press any key");
                    Console.ReadKey(true);
                    return;
                }
                Console.WriteLine("Your files:");
                foreach(var file in files)
                    Console.WriteLine(file);

                Console.WriteLine();
                Console.WriteLine("Write file name or '1' to stop");
                string filename = Console.ReadLine();
                if (filename == "1") return;
                Console.WriteLine();
                Console.WriteLine("Write user name or '1' to stop");
                string username = Console.ReadLine();
                if (username == "1") return;
                if (username == documentHandler.GetUser().Name)
                {
                    Console.WriteLine("You already have everything you need. Press any button");
                    Console.ReadKey(true);
                    return;
                }
                Console.Clear();
                Console.WriteLine("Press 1 to set view permission");
                Console.WriteLine("Press 2 to set edit permission");
                Console.WriteLine("Press 3 to remove premission");

                bool stop = false;
                IRoleStrategy role = null;
                while (!stop) {
                var key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            role = new ViewerStrategy();
                            stop = true;
                            break;
                        case ConsoleKey.D2:
                            role = new EditorStrategy();
                            stop = true;
                            break;

                        case ConsoleKey.D3:
                            role = null;
                            stop = true;
                            break;
                    }
                }
                try
                {
                    if (!documentHandler.GiveAccess(filename, username, role))
                    {
                        Console.WriteLine("There are no user with this name. Press any button");
                        Console.ReadKey(true);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Permission granted successfully. Press any key");
                        Console.ReadKey(true);
                        return;
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Wrong file name. Press any key");
                    Console.ReadKey(true);
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("You cant give access to this file. Press any key");
                    Console.ReadKey(true);
                    return;
                }
            }
        }
        private void Delete()
        {
            while (true)
            {
                var files = documentHandler.GetFiles();
                if (files.Count != 0)
                {
                    Console.WriteLine("Your files:");
                    foreach (var file in files)
                        Console.WriteLine(file);
                    Console.WriteLine();
                }
                Console.WriteLine("Input file name");
                string filename = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Delete from");
                Console.WriteLine("1 - Local");
                Console.WriteLine("2 - Db");
                Console.WriteLine("3 - Cloud");
                bool stop = false;
                StorageType type = StorageType.LocalFile;
                while (!stop)
                {

                    ConsoleKeyInfo key2 = new();
                    try
                    {
                        key2 = Console.ReadKey(true);
                    }
                    catch (System.InvalidOperationException)
                    {
                        continue;
                    }
                    switch (key2.Key)
                    {
                        case ConsoleKey.D1:
                            type = StorageType.LocalFile;
                            stop = true;
                            break;

                        case ConsoleKey.D2:
                            type = StorageType.Db;
                            stop = true;
                            break;

                        case ConsoleKey.D3:
                            type = StorageType.Firebase;
                            stop = true;
                            break;

                        case ConsoleKey.Escape:
                            return;
                    }
                }
                try
                {
                    if (!documentHandler.Remove(filename, type))
                    {
                        Console.WriteLine("There is no file with that name in " + type.ToString());
                        Console.WriteLine("Press any button");
                        Console.ReadKey(true);
                        return;
                    }
                    Console.WriteLine("File deleted. Press any button");
                    Console.ReadKey(true);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("File not found. Press any button");
                    Console.ReadKey(true);
                    return;
                }
            }
        }
        private void Save()
        {
            Console.Clear();
            Console.WriteLine("Input file name without extension");
            string filename = Console.ReadLine();
            Console.WriteLine("1 - Local");
            Console.WriteLine("2 - Db");
            Console.WriteLine("3 - Cloud");
            bool stop = false;
            StorageType type = StorageType.LocalFile;
            while (!stop)
            {

                ConsoleKeyInfo key2 = new();
                try
                {
                    key2 = Console.ReadKey(true);
                }
                catch (System.InvalidOperationException)
                {
                    continue;
                }
                switch (key2.Key)
                {
                    case ConsoleKey.D1:
                        type = StorageType.LocalFile;
                        stop = true;
                        break;

                    case ConsoleKey.D2:
                        type = StorageType.Db;
                        stop = true;
                        break;

                    case ConsoleKey.D3:
                        type = StorageType.Firebase;
                        stop = true;
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
            Console.Clear();
            Console.WriteLine("1 - PlainText");
            Console.WriteLine("2 - Markdown");
            Console.WriteLine("3 - RichText");
            bool stop2 = false;
            while (!stop2)
            {

                ConsoleKeyInfo key3 = new();
                try
                {
                    key3 = Console.ReadKey(true);
                }
                catch (System.InvalidOperationException)
                {
                    continue;
                }
                switch (key3.Key)
                {
                    case ConsoleKey.D1:
                        documentHandler.Save(filename, 1, type);
                        stop2 = true;
                        SetConsole();
                        break;

                    case ConsoleKey.D2:
                        documentHandler.Save(filename, 2, type);
                        stop2 = true;
                        SetConsole();
                        break;

                    case ConsoleKey.D3:
                        documentHandler.Save(filename, 3, type);
                        stop2 = true;
                        SetConsole();
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        public void SetConsole()
        {
            Console.Clear();
            Console.Write(documentHandler.GetText());
        }
    }
}
