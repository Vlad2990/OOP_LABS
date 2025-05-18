using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace oop1
{
    internal class GraphHandler
    {
        public GraphHandler() { }

        public void StartMenu()
        {
            while (true)
            {
                Console.WriteLine("1 - Start new");
                Console.WriteLine("2 - Load file");
                var key = Console.ReadKey(true);
                switch (key.Key)
                {

                    case ConsoleKey.D1:
                        Console.Clear();
                        Menu();
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Input file name");
                        string fileName = Console.ReadLine();
                        fileName += ".json";
                        if (!File.Exists(fileName))
                        {
                            Console.Clear();
                            Console.WriteLine("Wrong name");
                            break;
                        }
                        Console.Clear();
                        Load.Invoke(fileName);
                        Menu();
                        break;

                }
            }
        }

        public void Menu()
        {
            while (true)
            {
                int height;
                int width;
                int x1, y1, x2, y2, x3, y3, r;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(150, 0);
                Console.WriteLine("1 - Add rectangle");
                Console.SetCursorPosition(150, 1);
                Console.WriteLine("2 - Add circle");
                Console.SetCursorPosition(150, 2);
                Console.WriteLine("3 - Add triangle");
                Console.SetCursorPosition(150, 3);
                Console.WriteLine("4 - Move/delete/change");
                Console.SetCursorPosition(150, 4);
                Console.WriteLine("5 - Save and end");
                Console.SetCursorPosition(150, 5);
                Console.WriteLine("6 - End");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.WriteLine("Rectangle");
                        Console.WriteLine("x1(>0 && <150):");
                        x1 = Check.CheckInt(0, 149);
                        while (x1 == -1)
                        {
                            x1 = Check.CheckInt(0, 149);
                        }
                        Console.WriteLine("y1(>0 && < 44):");
                        y1 = Check.CheckInt(0, 43);
                        while (y1 == -1)
                        {
                            y1 = Check.CheckInt(0, 43);
                        }
                        Console.WriteLine("x2:");
                        x2 = Check.CheckInt(0, 149);

                        if (Math.Abs(x2 - x1) < 3) x2 = -1;
                        while (x2 == -1)
                        {
                            x2 = Check.CheckInt(0, 149);
                            if (x2 != -1 && Math.Abs(x2 - x1) < 3)
                            {
                                Console.WriteLine("Wrong input");
                                x2 = -1;
                            }
                        }
                        Console.WriteLine("y2:");
                        y2 = Check.CheckInt(0, 43);
                        if (Math.Abs(y2 - y1) < 3) y2 = -1;
                        while (y2 == -1)
                        {
                            y2 = Check.CheckInt(0, 43);
                            if (y2 != -1 && Math.Abs(y2 - y1) < 3)
                            {
                                Console.WriteLine("Wrong input");
                                y2 = -1;
                            }
                        }
                        Rect rect = new Rect(x1, y1, x2, y2);
                        NewItem.Invoke(rect);
                        Action.Invoke();
                        break;

                    case ConsoleKey.D2:
                        Console.Clear();
                        Console.WriteLine("Circle");
                        Console.WriteLine("x center(>2):");
                        x1 = Check.CheckInt(2, 145);
                        while (x1 == -1)
                        {
                            x1 = Check.CheckInt(2, 145);
                        }
                        Console.WriteLine("y center(>2):");
                        y1 = Check.CheckInt(2, 40);
                        while (y1 == -1)
                        {
                            y1 = Check.CheckInt(2, 40);
                        }
                        Console.WriteLine("Radius(>2):");
                        r = Check.CheckInt(2, 40);
                        if (r != -1 && (r + x1 > 145 || r + y1 > 40))
                        {
                            Console.WriteLine("Wrong input");
                            y2 = -1;
                        }
                        while (r == -1)
                        {
                            r = Check.CheckInt(2, 40);
                            if (r != -1 && (r + x1 > 145 || r + y1 > 40))
                            {
                                Console.WriteLine("Wrong input");
                                y2 = -1;
                            }
                        }
                        Circle circle = new Circle(x1, y1, r);
                        NewItem.Invoke(circle);
                        Action.Invoke();
                        break;

                    case ConsoleKey.D3:
                        Console.Clear();
                        Console.WriteLine("Triangle");
                        Console.WriteLine("x1:");
                        x1 = Check.CheckInt(0, 149); 
                        while (x1 == -1)
                        {
                            x1 = Check.CheckInt(0, 149);
                        }
                        Console.WriteLine("y1:");
                        y1 = Check.CheckInt(0, 43);
                        while (y1 == -1)
                        {
                            y1 = Check.CheckInt(0, 43);
                        }
                        Console.WriteLine("x2:");
                        x2 = Check.CheckInt(0, 149);
                        while (x2 == -1)
                        {
                            x2 = Check.CheckInt(0, 149);
                        }
                        Console.WriteLine("y2:");
                        y2 = Check.CheckInt(0, 43);
                        while (x2 == -1)
                        {
                            y2 = Check.CheckInt(0, 43);
                        }
                        Console.WriteLine("x3:");
                        x3 = Check.CheckInt(0, 149);
                        while (x3 == -1)
                        {
                            x3 = Check.CheckInt(0, 149);
                        }
                        Console.WriteLine("y3:");
                        y3 = Check.CheckInt(0, 43);
                        while (y3 == -1)
                        {
                            y3 = Check.CheckInt(0, 43);
                        }
                        Triangle triangle = null;
                        try
                        {
                            triangle = new Triangle(x1, y1, x2, y2, x3, y3);
                        }
                        catch(Exception ex)
                        {
                            NewItem.Invoke(triangle);
                            Console.SetCursorPosition(150, 13);
                            Console.WriteLine("Triangle with such");
                            Console.SetCursorPosition(150, 14);
                            Console.WriteLine("coordinates cannot");
                            Console.SetCursorPosition(150, 15);
                            Console.WriteLine("exist");
                            break;
                        }
                        NewItem.Invoke(triangle);
                        Action.Invoke();
                        break;

                    case ConsoleKey.D4:
                        ConsoleMove();
                        Action.Invoke();
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        Console.WriteLine("Input file name");
                        string fileName = Console.ReadLine();
                        fileName += ".json";
                        SaveToFile.Invoke(fileName);
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.Y:
                        if (key.Modifiers == ConsoleModifiers.Control)
                        {
                            Redo.Invoke();
                        }
                        break;
                    case ConsoleKey.Z:
                        if (key.Modifiers == ConsoleModifiers.Control)
                        {
                            Undo.Invoke();
                        }
                        break;
                    case ConsoleKey.D6:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void ConsoleMove()
        {
            int left = 0;
            int top = 0;
            

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(150, 6);
                Console.WriteLine("--------------------------");
                Console.SetCursorPosition(150, 7);
                Console.WriteLine("Press Space to select");
                Console.SetCursorPosition(150, 8);
                Console.WriteLine("Move using arrows keys");
                Console.SetCursorPosition(150, 9);
                Console.WriteLine("1-5 - Change color");
                Console.SetCursorPosition(150, 10);
                Console.WriteLine("Delete - Delete");
                Console.SetCursorPosition(150, 11);
                Console.WriteLine("Escape - Deselect/Escape");
                Console.SetCursorPosition(left, top);
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (top > 0) top--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (top < Console.WindowHeight - 1) top++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (left > 0) left--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (left < Console.WindowWidth - 1) left++;
                        break;
                    case ConsoleKey.Spacebar:
                        FindShape.Invoke(left, top);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
                Console.SetCursorPosition(left, top);
            }
        }


        public delegate void SpacePush(int left, int top);

        public event SpacePush FindShape;

        public delegate void NewShape(Shape shape);

        public event NewShape NewItem;

        public delegate void SaveOrLoad(string file);

        public event SaveOrLoad SaveToFile;

        public event SaveOrLoad Load;

        public delegate void SomeAction();

        public event SomeAction Action;

        public event SomeAction Undo;
        public event SomeAction Redo;
    }
}
