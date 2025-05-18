using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using oop1;

namespace UnitTests
{
    public class SerializerTest
    {
        private const string TestFile = "test_shapes.json";

        [Fact]
        public void Test_SaveAndLoadShapes()
        {
            List<Shape> shapes = new List<Shape> { new Triangle(0, 0, 5, 5, 10, 0) };
            Serializer serializer = new Serializer();
            serializer.SaveToFile(TestFile, shapes);
            List<Shape> loadedShapes = serializer.LoadFromFile(TestFile);
            Assert.Equal(shapes.Count, loadedShapes.Count);
        }
    }
}
