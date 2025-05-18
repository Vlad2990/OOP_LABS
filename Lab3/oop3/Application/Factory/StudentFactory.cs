using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factory
{
    public static class StudentFactory
    {
        public static Student Create(StudentDTO studentDTO)
        {
            return new Student(studentDTO.name, studentDTO.grade);
        }
    }
}
