using Domain.Dto;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController
    {
        private  StudentService _studentsService;

        public StudentController(StudentService studentService)
        {
            _studentsService = studentService;
        }


        [HttpGet("GetWithDapper")]
        public  async Task<List<StudentDto>> GetStudents()
        {
            return  await  _studentsService.GetStudents();
        }
    
        [HttpGet("GetWithoutDapper")]
        public async Task<List<StudentDto>> GetStudentsWithoutDapper()
        {
            return await _studentsService.GetStudentsWithoutDapper();
        }
        
        [HttpGet("GetWithEF")]
        public async Task<List<StudentDto>> GetStudentWithEF()
        {
            return  await _studentsService.GetStudentWithEnt();
        }

        }
}

