using System;

namespace oop1
{
    class Program
    {
        static void Main()
        {
            Console.SetWindowSize(176, 44);
            Console.SetBufferSize(176, 44);

            Graph graph = new Graph();
            graph.Start();
        }
    }
}