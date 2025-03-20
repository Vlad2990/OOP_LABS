using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace oop1
{
    [Serializable]
    public class Triangle : Shape
    {
        public Triangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            if (!IsTriangleExists(x1, y1, x2, y2, x3, y3)) throw new Exception();
            x = Math.Min(x1, Math.Min(x2, x3));
            y = Math.Min(y1, Math.Min(y2, y3));
            width = Math.Max(x1, Math.Max(x2, x3)) - x + 1; 
            height = Math.Max(y1, Math.Max(y2, y3)) - y + 1;

            array = new ColoredChar[height, width];

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    array[i, j] = new ColoredChar();
                }
            }
            DrawLine(x1 - x, y1 - y, x2 - x, y2 - y);
            DrawLine(x1 - x, y1 - y, x3 - x, y3 - y);
            DrawLine(x3 - x, y3 - y, x2 - x, y2 - y);
            Fill();
        }

        [JsonConstructor]
        protected Triangle() { }
        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            int x = x1, y = y1;
            int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? 1 : -1;
            int dy = Math.Abs(y2 - y1), sy = y1 < y2 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;

            while (true)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    array[y, x] = new ColoredChar('*', ConsoleColor.White);
                }

                if (x == x2 && y == y2) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x += sx; }
                if (e2 < dy) { err += dx; y += sy; }
            }
        }
        private static bool IsTriangleExists(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            int area = x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);
            return area != 0; 
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
        private void Fill()
        {
            int k = 0, q = 0;
            for (int i = 1; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if (array[i,j].c == '*')
                    {
                        k = j;
                        j = width - 1;
                        while (array[i,j].c != '*')
                        {
                            --j;
                        }
                        q = j;
                        for (; k != q; ++ k)
                        {
                            array[i, k].c = '*';
                        }
                    }
                }
            }
        }
        public override object Clone()
        {
            Triangle clone = new Triangle();
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
