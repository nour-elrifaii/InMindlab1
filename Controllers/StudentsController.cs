using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ObjectMapperService _objectMapperService;
    public StudentsController(ObjectMapperService mapperService)
    {
        _objectMapperService = mapperService;
    }
    private static List<Student> students = new()
    {
        new Student { Id = 1, Name = "Tarek", Email = "tarek@gmail.com" },
        new Student { Id = 2, Name = "Reine", Email = "reine@gmail.com" }
    };
    

    [HttpGet]
    public IActionResult GetAllStudents()
    {
        return Ok(students);
    }

    [HttpGet("student/{id}")]
    public IActionResult GetStudentById([FromRoute] long id)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        return Ok(student);
    }

    [HttpGet("search")]
    public IActionResult GetStudentByName([FromQuery] string name)
    {
        var student = students.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        return Ok(student);
    }

    [HttpGet("date")]
    public IActionResult GetDate([FromHeader(Name = "Accept-Language")] string language)
    {
        var culture = CultureInfo.GetCultureInfo(language);
        var newDate = DateTime.Now.ToString("D", culture);
        return Ok(newDate);
    }

    
    [HttpPost("student")]
    public IActionResult NewName([FromBody] Student newstudent)
    {
        if (newstudent == null)
        {
            return BadRequest();
        }
        var student = students.FirstOrDefault(x => x.Id == newstudent.Id);
        student.Name = newstudent.Name;
        student.Email = newstudent.Email;
        return Ok(student);
    }
    
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
    }

    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] FileUploadDto dto)
    {
        var file = dto.File;
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var path = Path.Combine("wwwroot/uploads", file.FileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }
        catch(IOException e)
        {
            return StatusCode(500, "There was an error uploading the file.");
        }
        return Ok(new { path });
    }

    
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteStudent([FromRoute] long id)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        students.Remove(student);
        return Ok($"student {id} has been deleted");
    }

    [HttpPost("mapStudentToPerson")]
    public IActionResult MapStudentToPerson([FromBody] Student student)
    {
        if (student == null)
            return BadRequest("Student is null");

        Person person = _objectMapperService.Map<Student, Person>(student);
        return Ok(person);
    }
    

}

