using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Adapters;
using Application.Factory;

namespace Application.Service
{
    public class StudentService
    {
        private readonly Repository _repository = new();

        public async Task AddAsync(StudentDTO studentDTO)
        {
            try
            {
                Student student = StudentFactory.Create(studentDTO);
                await _repository.AddAsync(student);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StudentDTO>> ListAllAsync()
        {
            var students = await _repository.GetStudentsAsync();
            List<StudentDTO> result = new List<StudentDTO>();
            foreach (var student in students)
            {
                result.Add(new StudentDTO(student.Name, student.Grade));
            }
            return result;
        }
        public async Task<StudentDTO> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            return new StudentDTO(student.Name, student.Grade);
        }
        public async Task ChangeAsync(StudentDTO studentDTO, int id)
        {
            var student = StudentFactory.Create(studentDTO);
            student.Id = id;
            await _repository.UpdeteAsync(id, student);
        }
    }
}
