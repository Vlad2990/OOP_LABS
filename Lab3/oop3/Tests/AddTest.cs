using Application.Service;
using DTOs;

namespace Tests
{
    public class AddTest
    {
        StudentService studentService = new();
        [Fact]
        public async void Test()
        {
            StudentDTO test = new StudentDTO("Test", 10);
            await studentService.AddAsync(test);
            var list = await studentService.ListAllAsync();
            Assert.True(test.name == list[^1].name && test.grade == list[^1].grade);
        }
    }
}