using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
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

    [HttpGet("id:{id}")]
    public IActionResult GetStudentById([FromRoute] long id)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
            return NotFound();

        return Ok(student);
    }

    [HttpGet("search")]
    public IActionResult GetStudentByName([FromQuery] string name)
    {
        name = name.ToLower();
        var student = students.FirstOrDefault(s => s.Name.ToLower().Contains(name));
        return Ok(student);
    }
    
    [HttpGet("date")]
    public IActionResult GetDate([FromHeader(Name= "Accept-Language")] string language)
    {
        var availableCultures = new[] { "en-US", "es-ES", "fr-FR" };
        if (!availableCultures.Contains(language))
        {
            return NotFound("Invalid language");
        }

        try
        {
            var culture = CultureInfo.GetCultureInfo(language);
            var newDate = DateTime.Now.ToString("D", culture);
            return Ok(newDate);
        }
        catch (CultureNotFoundException)
        {
            return NotFound("Invalid language");
        }
    }
    
    [HttpPost("name")]
    public IActionResult PostName([FromBody] NewStudent stud)
    {
        var student = students.FirstOrDefault(s => s.Id == stud.Id);
        if (student == null)
            return NotFound();
        
        student.Name = stud.Name;
        student.Email = stud.Email;
        return Ok(student);
    }
    
    public class FileUploadDto
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }

    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] FileUploadDto dto)
    {
        var file = dto.File;
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var path = Path.Combine("wwwroot/uploads", file.FileName);
        Directory.CreateDirectory(Path.GetDirectoryName(path));

        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { path });
    }

    
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteStudent([FromRoute] long id)
    {
        var student = students.FirstOrDefault(s => s.Id == id);
        if (student == null)
            return NotFound();
        students.Remove(student);
        return Ok($"student {id} has been deleted");
    }
    

}

public class NewStudent
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}

