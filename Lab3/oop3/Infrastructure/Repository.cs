using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace Infrastructure
{
    public class Repository
    {
        private readonly string path;

        public Repository() 
        {
            string p = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder = Path.Combine(p,"Students");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string file = Path.Combine(folder, "students.json");
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            path = file;
        }
        public async Task<List<Student>> GetStudentsAsync()
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    List<Student>? students = await JsonSerializer.DeserializeAsync<List<Student>>(fs);
                    return students;
                }
            }
            catch
            {
                return new List<Student>();
            }
        }
        public async Task AddAsync(Student student)
        {
            var students = await GetStudentsAsync();
            student.Id = students.Count + 1;
            students.Add(student);

            await SaveAsync(students);
        }
        public async Task<Student> GetByIdAsync(int id)
        {
            var students = await GetStudentsAsync();
            if (students.Count <= id) throw new IndexOutOfRangeException();
            return students[id];
        }

        public async Task UpdeteAsync(int id, Student student)
        {
            var students = await GetStudentsAsync();
            if (students.Count <= id) throw new IndexOutOfRangeException();
            students[id] = student;

            await SaveAsync(students);
        }

        private async Task SaveAsync(List<Student> students)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await JsonSerializer.SerializeAsync<List<Student>>(fs, students);
            }
        }

    }
}
