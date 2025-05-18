using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace oop1
{
    [Serializable]
    public abstract class Shape : ICloneable
    {
        [JsonProperty]
        protected int height = 0;
        [JsonProperty]
        protected int width = 0;
        [JsonProperty]
        protected int x = 0;
        [JsonProperty]
        protected int y = 0;
        [JsonProperty]
        protected double aspectRatio = 1.9;
        protected bool selected = false;
        [JsonProperty]
        protected ColoredChar[,] array;
        List<ConsoleColor> colors = new List<ConsoleColor>
        {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Gray,
            ConsoleColor.Yellow,
            ConsoleColor.Cyan
        };

        public Shape(int height, int width, int x, int y)
        {
            this.height = height;
            this.width = width;
            array = new ColoredChar[height, width];
            this.x = x;
            this.y = y;
        }
        [JsonConstructor]
        protected Shape()
        {

        }

        public void Print()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (array[i,j] == null || array[i, j].c == ' ') continue;
                    Console.SetCursorPosition(x + j, y + i);
                    Console.ForegroundColor = array[i, j].color;
                    Console.Write(array[i, j].c); 
                }
            }
        }

        public bool Select(int left, int top)
        {
            bool stop = false;
            if (left >= x && left < x + width && top >= y && top < y + height && array[top - y, left - x].c != ' ')
            {
                selected = true;
                stop = true;
                Selected();
                ShapeControl();
            }
            return stop;
        }
        public virtual void Selected()
        {
            Change.Invoke();
        }
        public virtual void UnSelected()
        {
            selected = false;
            Change.Invoke();
        }
        public void ShapeControl()
        {
            
            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (y - 1 >= 0) --y;
                        Change.Invoke();
                        break;
                    case ConsoleKey.DownArrow:
                        if (y + 1 + height <= 43) ++y;
                        Change.Invoke();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (x - 1 >= 0) --x;
                        Change.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        if (x + 1 + height <= 149) ++x;
                        Change.Invoke();
                        break;
                    case ConsoleKey.D1:
                        ChangeColor(0);
                        break;
                    case ConsoleKey.D2:
                        ChangeColor(1);
                        break;
                    case ConsoleKey.D3:
                        ChangeColor(2);
                        break;
                    case ConsoleKey.D4:
                        ChangeColor(3);
                        break;
                    case ConsoleKey.D5:
                        ChangeColor(4);
                        break;
                    case ConsoleKey.Delete:
                        Delete.Invoke();
                        return;
                    case ConsoleKey.Escape:
                        UnSelected();
                        return;
                }
            }
        }

        public void ChangeColor(int k)
        {
            if (array[height / 2, width / 2].color == colors[k]) return;

            for (int i = 0; i < height ; i++)
                for (int j = 0; j < width ; j++)
                    array[i, j].color = colors[k];

            Change.Invoke();
        }

        public abstract object Clone();

        public bool IsSelected()
        {
            return selected;
        }


        public delegate void SomeChanges();

        public event SomeChanges Change;

        public event SomeChanges Delete;
    }
}
