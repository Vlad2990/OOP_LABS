using Xunit;
using oop1;
using System.Collections.Generic;

namespace UnitTests
{
    public class HistoryTest
    {
        [Fact]
        public void Test_UndoRedo()
        {
            History history = new History();
            List<Shape> shapes = new List<Shape> { new Triangle(0, 0, 5, 5, 10, 0) };

            history.AddAction(shapes);
            history.Undo();

            // После Undo список должен быть пустым
            Assert.Equal(0, history.Undo().Count);

            history.Redo();

            // После Redo фигуры должны вернуться
            Assert.Equal(1, history.Redo().Count);
        }

        [Fact]
        public void Test_UndoWithoutHistory()
        {
            History history = new History();

            // Если истории нет, Undo не должно ломать программу
            var exception = Record.Exception(() => history.Undo());
            Assert.Null(exception);
        }
    }

}
