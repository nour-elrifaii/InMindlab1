using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class StudentService
    {
        private static List<Student> students = new()
        {
            new Student { StudentId = 1, Name = "Tarek", Email = "tarek@gmail.com" },
            new Student { StudentId = 2, Name = "Reine", Email = "reine@gmail.com" }
        };

        public List<Student> GetAllStudents()
        {
            return students;
        }

        public Student? GetStudentById(long id)
        {
            return students.FirstOrDefault(s => s.StudentId == id);
        }

        public Student? GetStudentByName(string name)
        {
            return students.FirstOrDefault(s => s.Name.ToLower().Contains(name.ToLower()));
        }

        public string GetFormattedDate(string language)
        {
            var availableCultures = new[] { "en-US", "es-ES", "fr-FR" };

            if (!availableCultures.Contains(language))
            {
                throw new ArgumentException("Invalid language");
            }

            var culture = System.Globalization.CultureInfo.GetCultureInfo(language);
            return DateTime.Now.ToString("D", culture);
        }

        public Student? UpdateStudent(long id, string name, string email)
        {
            var student = students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return null;

            student.Name = name;
            student.Email = email;
            return student;
        }

        public bool DeleteStudent(long id)
        {
            var student = students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return false;

            students.Remove(student);
            return true;
        }

        public void AddStudent(Student student)
        {
            students.Add(student);
        }
    }
}