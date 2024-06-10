using Microsoft.AspNetCore.Mvc;
using StudentApi.Controllers;
using StudentApi.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StudentApi.Tests
{
    public class StudentsControllerTests
    {
        private List<Student> GetTestStudents()
        {
            return new List<Student>
            {
                new Student { Id = 1, Name = "Jan Kowalski", Age = 21 },
                new Student { Id = 2, Name = "Marek nowak", Age = 23 }
            };
        }

        private StudentsController GetControllerWithStudents(List<Student> students = null)
        {
            return new StudentsController(students ?? GetTestStudents());
        }

        [Fact]
        public void GetStudents_ReturnsOkResult_WithListOfStudents()
        {
            var controller = GetControllerWithStudents();
            var result = controller.GetStudents();

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Student>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetStudent_ReturnsOkResult_WithStudent(int id)
        {
            var controller = GetControllerWithStudents();

            var result = controller.GetStudent(id);

            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Student>(actionResult.Value);
            Assert.Equal(id, returnValue.Id);
        }

        [Theory]
        [InlineData(3)]
        public void GetStudent_ReturnsNotFoundResult(int id)
        {
            var controller = GetControllerWithStudents();

            var result = controller.GetStudent(id);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void PostStudent_ReturnsCreatedAtActionResult_WithNewStudent()
        {
            var controller = GetControllerWithStudents();
            var newStudent = new Student { Name = "Anna Nowak", Age = 22 };

            var result = controller.PostStudent(newStudent);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Student>(actionResult.Value);
            Assert.Equal(3, returnValue.Id);
            Assert.Equal("Anna Nowak", returnValue.Name);
            Assert.Equal(22, returnValue.Age);
        }

        [Fact]
        public void PutStudent_ReturnsNoContentResult()
        {
            var controller = GetControllerWithStudents();
            var updatedStudent = new Student { Id = 1, Name = "Jan Kowalski Updated", Age = 22 };
            var result = controller.PutStudent(1, updatedStudent);

            Assert.IsType<NoContentResult>(result);
            var actionResult = controller.GetStudent(1).Result as OkObjectResult;
            Assert.NotNull(actionResult); 
            var student = actionResult.Value as Student;
            Assert.NotNull(student); 
            Assert.Equal("Jan Kowalski Updated", student.Name);
            Assert.Equal(22, student.Age);
        }

        [Fact]
        public void PutStudent_ReturnsNotFoundResult()
        {
            var controller = GetControllerWithStudents();
            var updatedStudent = new Student { Id = 3, Name = "Non Existent", Age = 22 };
            var result = controller.PutStudent(3, updatedStudent);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteStudent_ReturnsNoContentResult()
        {
            var controller = GetControllerWithStudents();
            var result = controller.DeleteStudent(1);
            Assert.IsType<NoContentResult>(result);
            var students = controller.GetStudents().Result as OkObjectResult;
            var studentList = students.Value as List<Student>;
            Assert.Null(studentList.FirstOrDefault(s => s.Id == 1));
        }

        [Fact]
        public void DeleteStudent_ReturnsNotFoundResult()
        {
            var controller = GetControllerWithStudents();
            var result = controller.DeleteStudent(3);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}