using Xunit;
using Moq;
using oop2;

namespace UnitTests
{
    public class DocumentHandlerTests
    {
        private readonly Mock<Document> _mockDocument;
        private readonly DocumentHandler _handler;

        public DocumentHandlerTests()
        {
            _mockDocument = new Mock<Document>();
            _handler = new DocumentHandler();
            _handler.SetUser("Admin");
        }
        [Fact]
        public void UndoTest()
        {
            _handler.Open();
            _handler.InsertText('a', 0, 0);
            _handler.Undo();
            Assert.Equal(0, _handler.MaxLeft(0));
        }
        [Fact]
        public void RedoTest()
        {
            _handler.Open();
            _handler.InsertText('a', 0, 0);
            _handler.Undo();
            _handler.Redo();
            Assert.Equal(1, _handler.MaxLeft(0));
        }

        [Fact]
        public void CutTest()
        {
            _handler.Open();
            _handler.InsertText('a', 0, 0);
            _handler.InsertText('b', 0, 1);
            _handler.Cut(0, 0, 2);
            Assert.Equal(0, _handler.MaxLeft(0));
        }

    }
}