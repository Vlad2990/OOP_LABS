using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Commands
{
    public class AddCommand : Command
    {
        private readonly StudentDTO _student;
        public AddCommand(StudentService studentService, StudentDTO student) : base(studentService)
        {
            _student = student;
        }

        public override async Task<List<StudentDTO>> Execute()
        { 
            await _studentService.AddAsync(_student);
            return await _studentService.ListAllAsync();
        }
    }
}
