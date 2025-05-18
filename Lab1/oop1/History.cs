using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oop1
{
    public class History
    {
        private Stack<List<Shape>> undo = new Stack<List<Shape>>();
        private Stack<List<Shape>> redo = new Stack<List<Shape>>();
        private List<Shape> curr = new List<Shape>();

        public History() { }

        public void AddAction(List<Shape> shapes)
        {
            List<Shape> shapesCopy = shapes.Select(s => (Shape)s.Clone()).ToList();
            undo.Push(curr);
            curr = shapesCopy;
            redo.Clear();
        }

        public List<Shape> Undo()
        {
            if (undo.Count == 0) return curr;
            List<Shape> shapesCopy = curr.Select(s => (Shape)s.Clone()).ToList();
            redo.Push(shapesCopy);
            curr = undo.Pop();
            return curr;
        }

        public List<Shape> Redo()
        {
            if (redo.Count == 0) return curr;
            List<Shape> shapesCopy = curr.Select(s => (Shape)s.Clone()).ToList();
            undo.Push(shapesCopy);
            curr = redo.Pop(); 
            return curr;
        }
    }
}
