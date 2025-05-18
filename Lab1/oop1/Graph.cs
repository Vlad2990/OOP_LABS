using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace oop1
{
    internal class Graph
    {
        public Graph() 
        {
            graphHandler.FindShape += Find;
            graphHandler.NewItem += AddItem;
            graphHandler.SaveToFile += Save;
            graphHandler.Load += Load;
            graphHandler.Action += SaveAction;
            graphHandler.Undo += UndoAction;
            graphHandler.Redo += RedoAction;
        }
        private List<Shape> Shapes = new List<Shape>();
        public void Find(int left, int top)
        {
            foreach (Shape shape in Shapes)
            {
                if (shape.Select(left, top))
                {
                    return;
                }    
            }
        }
        private GraphHandler graphHandler = new GraphHandler();
        private Serializer Serializer = new Serializer();
        private History History = new History();

        public void AddItem(Shape shape)
        {
            if (shape != null)
            {
                Shapes.Add(shape);
                shape.Change += Print;
                shape.Delete += RemoveItem;
            }
            Print();
        }
        public void Start()
        {
            graphHandler.StartMenu();
        }
        public void Print()
        {
            Console.Clear();
            foreach (Shape shape in Shapes)
                shape.Print();
        }
        public void RemoveItem()
        {
            foreach(Shape shape in Shapes)
            {
                if (shape.IsSelected())
                {
                    shape.UnSelected();
                    shape.Change -= Print;
                    shape.Delete -= RemoveItem;
                    Shapes.Remove(shape);
                    Print();
                    return;
                }
            }
        }
        public void Save(string filename)
        {
            Serializer.SaveToFile(filename, Shapes);
        }
        public void Load(string filename)
        {
            Shapes = Serializer.LoadFromFile(filename);
            foreach (Shape shape in Shapes)
            {
                shape.Change += Print;
                shape.Delete += RemoveItem;
            }
            SaveAction();
            Print();
        }
        public void SaveAction()
        {
            History.AddAction(Shapes);
        }
        public void UndoAction()
        {
            Shapes = History.Undo();
            Remake();
        }
        public void RedoAction()
        {
            Shapes = History.Redo();
            Remake();
        }
        public void Remake()
        {
            foreach (Shape shape in Shapes)
            {
                shape.Change += Print;
                shape.Delete += RemoveItem;
            }
            Print();
        }
    }
}
