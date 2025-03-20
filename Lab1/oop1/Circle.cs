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
    internal class Circle : Shape
    {
        public Circle(int x1, int y1, int r)
        {
            x = x1 - r;
            y = y1 - r;
            int d = r * 2;
            height = d;
            width = d;
            array = new ColoredChar[d, d];
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    if (((double)(i - r) * (double)(i - r) * aspectRatio + (double)(j - r) * (double)(j - r)) < r * r)
                    {
                        array[i, j] = new ColoredChar('*', ConsoleColor.White);
                    }
                    else
                    {
                        array[i, j] = new ColoredChar(' ', ConsoleColor.White);
                    }
                }
            }
            RemoveSpace();
        }
        [JsonConstructor]
        protected Circle()
        {
        }

        public override void Selected()
        {
            array[height / 2, width / 2].color = ConsoleColor.Blue;

            base.Selected();
        }
        public override void UnSelected()
        {
            ConsoleColor color = array[height / 2 + 1, width / 2 + 1].color;
            array[height / 2, width / 2].color = color;
            base.UnSelected();
        }
        private void RemoveSpace()
        {
            bool flag = true;
            List<int> num = new List<int>();
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (array[i, j].c != ' ')
                    {
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    num.Add(i);
                }
            }
            ColoredChar[,] newArray = new ColoredChar[height - num.Count, width];
            int k = 0;
            for (int i = 0; i < height; ++i)
            {
                if (num.Contains(i)) continue;
                for (int j = 0; j < width; ++j)
                {
                    newArray[k, j] = array[i, j];
                }
                ++k;
            }

            height -= num.Count;
            array = new ColoredChar[height, width];
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    array[i, j] = newArray[i, j];
                }
            }
        }
    public override object Clone()
        {
            Circle clone = new Circle();
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
