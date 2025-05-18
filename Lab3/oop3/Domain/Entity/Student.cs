using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Student
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Grade { get; set; }

        public Student(string name, int grade)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (grade < 0 || grade > 10) throw new ArgumentOutOfRangeException(nameof(grade));
            Name = name;
            Grade = grade;
        }
    }
}
