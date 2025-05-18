using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Commands
{
    public class ViewCommand : Command
    {
        public ViewCommand(StudentService studentService) : base(studentService) { }
        public override async Task<List<StudentDTO>> Execute()
        {
            return await _studentService.ListAllAsync();
        }
    }
}
