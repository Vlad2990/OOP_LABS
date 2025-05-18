using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace oop1
{
    [Serializable]
    internal class Rect : Shape
    {

        public Rect(int x1, int y1, int x2, int y2)
        {
            width = Math.Abs(x1 - x2);
            height = Math.Abs(y1 - y2);
            array = new ColoredChar[height, width];
            if (x1 > x2)
            {
                int temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                int temp = y1;
                y1 = y2;
                y2 = temp;
            }
            x = x1;
            y = y1;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((i == j && i == 0) || (i == 0 && j == width - 1)
                        || (i == height - 1 && j == width - 1) || (i == height - 1 && j == 0))
                    {
                        array[i, j] = new ColoredChar(' ', ConsoleColor.White);
                        continue;
                    }
                    if (j == 0 || j == width - 1)
                    {
                        array[i, j] = new ColoredChar('|', ConsoleColor.White);
                        continue;
                    }
                    if (i == 0 || i == height - 1)
                    {
                        array[i, j] = new ColoredChar('-', ConsoleColor.White);
                        continue;
                    }

                    array[i, j] = new ColoredChar('0', ConsoleColor.White);
                }
            }
        }
        [JsonConstructor]
        protected Rect()
        {

        }

        public override void Selected()
        {
            for (int i = 0; i < height; ++i)
            {
                array[i, 0].color = ConsoleColor.Blue;
                array[i, width - 1].color = ConsoleColor.Blue;
            }

            for (int i = 0; i < width; ++i)
            {
                array[0, i].color = ConsoleColor.Blue;
                array[height - 1, i].color = ConsoleColor.Blue;
            }
            base.Selected();
        }
        public override void UnSelected()
        {
            ConsoleColor color = array[height / 2, width / 2].color;
            for (int i = 0; i < height; ++i)
            {
                array[i, 0].color = color;
                array[i, width - 1].color = color;
            }

            for (int i = 0; i < width; ++i)
            {
                array[0, i].color = color;
                array[height - 1, i].color = color;
            }
            base.UnSelected();
        }
        public override object Clone()
        {
            Rect clone = new Rect();
            clone.x = x;
            clone.y = y;
            clone.width = width;
            clone.height = height;
            clone.array = new ColoredChar[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    clone.array[i, j] = (ColoredChar)array[i, j].Clone();
                }
            }

            return clone;
        }
    }
}
