using Xunit;
using oop1;

namespace UnitTests
{
    public class ConstructorTest
    {
        [Fact]
        public void Test_CreateTriangle_Valid()
        {
            Triangle t = new Triangle(0, 0, 5, 5, 10, 0);
            Assert.NotNull(t);
        }

        [Fact]
        public void Test_CreateTriangle_Invalid()
        {
            Assert.Throws<Exception>(() => new Triangle(0, 0, 1, 1, 2, 2));
        }

    }
}