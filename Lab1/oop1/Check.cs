using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop1
{
    internal class Check
    {
        public static int CheckInt()
        {
            int res = 0;
            bool IsValid = false;
            while (!IsValid)
            {
                if (int.TryParse(Console.ReadLine(), out res))
                {
                    return res;
                }
                else
                {
                    Console.WriteLine("Wrong input");
                    return -1;
                }
            }
            return res;
        }
        public static int CheckInt(int min, int max)
        {
            int res = 0;

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out res))
                {
                    if (res > max || res < min)
                    {
                        Console.WriteLine("Wrong input");
                        continue;
                    }
                    return res;
                }
                else
                {
                    Console.WriteLine("Wrong input");
                    return -1;
                }
            }
        }
    }
}
