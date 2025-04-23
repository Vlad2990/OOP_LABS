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
            UserMenu();
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
                        if (!documentHandler.GetUser().Role.CanEdit) break;
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
                        GetFileHistory();
                        break;

                    case ConsoleKey.D6:
                        ChangeColor();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
        private void PrintStartMenu()
        {
            Console.WriteLine("1 - Open");
            if (documentHandler.GetUser().Role.CanEdit)
                Console.WriteLine("2 - Create new empty");
            else Console.WriteLine();
            Console.WriteLine("3 - Delete");
            Console.WriteLine("4 - Change user role");
            Console.WriteLine("5 - See file history");
            Console.WriteLine("6 - Change theme");
            Console.WriteLine("Esc - Change user");
        }
        private void CreateEmpty()
        {
            Console.Clear();
            documentHandler.Open();
            Edit();
        }
        private void OpenFile()
        {
            while (true)
            {
                Console.WriteLine("Input file name");
                string filename = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Load from:");
                Console.WriteLine("1 - Local");
                Console.WriteLine("2 - Db");
                Console.WriteLine("3 - Cloude");
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
                    Console.Clear();
                    documentHandler.Open(filename, type);
                    if (documentHandler.GetUser().Role.CanEdit) Edit();
                    else View();
                    break;
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found. Press 1 to input another name or any diff key to stop");
                    if (Console.ReadKey(true).Key != ConsoleKey.D1)
                    {
                        break;
                    }
                }
            }
        }

        private void GetFileHistory()
        {
            while (true)
            {
                Console.WriteLine("Input file name");
                string filename = Console.ReadLine();
                try
                {
                    Console.Clear();
                    var his = documentHandler.GetHistory(filename);
                    foreach(var line in his)
                        Console.WriteLine(line);
                    Console.ReadKey(true);
                    break;
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found. Press 1 to input another name or any diff key to stop");
                    if (Console.ReadKey(true).Key != ConsoleKey.D1)
                    {
                        break;
                    }
                }
            }
        }
        private void Edit()
        {
            top = 0;
            left = 0;
            Console.SetCursorPosition(left, top);
            documentHandler.SetConsole();
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
                            documentHandler.SetConsole();

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
                        documentHandler.SetConsole();
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
                        documentHandler.SetConsole();
                        break;

                    case ConsoleKey.Y when key.Modifiers == ConsoleModifiers.Control:
                        documentHandler.Redo();
                        documentHandler.SetConsole();
                        break;

                    case ConsoleKey.Delete:
                        if (documentHandler.MaxLeft(top) == left) break;
                        documentHandler.DeleteText(top, left, 1);
                        documentHandler.SetConsole();
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
                        documentHandler.SetConsole();
                        clipLeft = clipRight = clipStart = left;
                        break;

                    case ConsoleKey.Enter:
                        documentHandler.InsertText('\n', top, left);
                        top++;
                        left = 0;
                        clipLeft = clipRight = clipStart = left;
                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        documentHandler.InsertText(key.KeyChar, top, left, IsBold, IsItalic, IsUnderline);
                        if (left == 175)
                        {
                            left = 0;
                            top++;
                        }
                        else left++;
                            documentHandler.SetConsole();

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
                documentHandler.SetConsole();
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
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Write user name or '1' to stop");
                string username = Console.ReadLine();
                if (username == "1") return;
                if (!documentHandler.UserExist(username))
                {
                    Console.WriteLine("Wrong user name");
                    continue;
                }
                Console.Clear();
                Console.WriteLine("Press 1 to set view permission");
                Console.WriteLine("Press 2 to set edit permission");

                bool stop = false;

                while (!stop) {
                var key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D1:
                            if (!documentHandler.ChangeRole(username, new ViewerStrategy()))
                            {
                                Console.WriteLine("You cant change user roles");
                            }
                            stop = true;
                            break;
                        case ConsoleKey.D2:
                            if (!documentHandler.ChangeRole(username, new EditorStrategy()))
                            {
                                Console.WriteLine("You cant change user roles");
                            }
                            stop = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void Delete()
        {
            while (true)
            {
                Console.WriteLine("Input file name");
                string filename = Console.ReadLine();
                try
                {
                    documentHandler.Remove(filename);
                    Console.WriteLine("File deleted. Press any button");
                    Console.ReadKey(true);
                    break;
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found. Press 1 to input another name or any diff key to stop");
                    if (Console.ReadKey(true).Key != ConsoleKey.D1)
                    {
                        break;
                    }
                }
            }
        }
        private void Save()
        {
            Console.Clear();
            Console.WriteLine("1 - Local");
            Console.WriteLine("2 - Db");
            Console.WriteLine("3 - Cloude");
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
                        documentHandler.Save(1, type);
                        stop2 = true;
                        break;

                    case ConsoleKey.D2:
                        documentHandler.Save(2, type);
                        stop2 = true;
                        break;

                    case ConsoleKey.D3:
                        documentHandler.Save(3, type);
                        stop2 = true;
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
