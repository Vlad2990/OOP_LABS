using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Commands
{
    public class EditCommand : Command
    {
        private readonly StudentDTO _student;
        private readonly int _id;
        public EditCommand(StudentService studentService, StudentDTO studentDTO, int id) : base(studentService)
        {
            _student = studentDTO;
            _id = id;
        }
        public override async Task<List<StudentDTO>> Execute()
        {
            await _studentService.ChangeAsync(_student, _id);
            return await _studentService.ListAllAsync();
        }
    }
}