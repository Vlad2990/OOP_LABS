using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Commands
{
    public abstract class Command
    {
        protected readonly StudentService _studentService = null!;
        protected Command(StudentService studentService)
        {
            _studentService = studentService;
        }

        public abstract Task<List<StudentDTO>> Execute();
    }
}
