using Xunit;
using oop2;
using System.Collections.Generic;

namespace UnitTests
{
    public class DocumentTests
    {
        [Fact]
        public void WriteSplitsTextTest()
        {
            var doc = new Document();
            string text = "Line1\nLine2";

            doc.Write(text);

            Assert.Equal(2, doc.MaxLine());
            Assert.Equal(5, doc.MaxLeft(0));
            Assert.Equal(5, doc.MaxLeft(1));
        }

        [Fact]
        public void InsertTest()
        {
            var doc = new Document();
            var chars = new List<FormatChar> { new FormatChar('a'), new FormatChar('b') };

            doc.Insert(0, 0, chars);

            Assert.Equal(2, doc.MaxLeft(0));
            Assert.Equal('a', doc.ReadLine(0)[0].c);
            Assert.Equal('b', doc.ReadLine(0)[1].c);
        }
        [Fact]
        public void RemoveTest()
        {
            var doc = new Document();
            doc.Write("abc");

            doc.Remove(0, 1, 1);

            Assert.Equal(2, doc.MaxLeft(0));
            Assert.Equal('a', doc.ReadLine(0)[0].c);
            Assert.Equal('c', doc.ReadLine(0)[1].c);
        }

        [Fact]
        public void ReadAllTextTest()
        {
            var doc = new Document();
            doc.Write("Line1\nLine2");

            var result = doc.Read();

            Assert.Equal("Line1\r\nLine2\r\n", result);
        }
    }
}