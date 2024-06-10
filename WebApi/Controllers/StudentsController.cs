using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly List<Student> _students;

        public StudentsController(List<Student> students = null)
        {
            _students = students ?? new List<Student>
            {
                new Student { Id = 1, Name = "Jan Kowalski", Age = 21 },
                new Student { Id = 2, Name = "Marek nowak", Age = 23 }
            };
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            return Ok(_students);
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> PostStudent(Student student)
        {
            student.Id = _students.Max(s => s.Id) + 1;
            _students.Add(student);

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);//
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id, Student student)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            _students.Remove(student);

            return NoContent();
        }
    }
}