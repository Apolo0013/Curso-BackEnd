using BackEnd.Service.Course;
using Microsoft.AspNetCore.Mvc;
//model
using BackEnd.Model.Course;
//Mensagem/Common
using BackEnd.Common.Course.Error;
using BackEnd.Common.Course.Sucesso;

[ApiController]
[Route("course")]
public class CourseController : ControllerBase
{
    //service
    private readonly CourseService _service;

    public CourseController(CourseService service)
    {
        _service = service;
    }

    [HttpGet("users/add")]
    //Rota: responsavel por add o curso que o usuario esta fazendo.
    public async Task<IActionResult> AddCoursesUsers([FromQuery] string idUser, [FromQuery] string idCourse)
    {
        await _service.AddCoursesUser(idUser, idCourse);
        return Ok("fodasi");
    }

    [HttpGet("users/get")]
    public async Task<IActionResult> GetCoursesUsers([FromQuery] string idUser)
    {
        var listsCourses = await _service.GetCoursesUser(idUser);
        return Ok(listsCourses);
    }


    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _service.GetAllCourses();
        return Ok(new ReturnCourseModel<List<CourseModel>> ()
        {
            Code = SucessoCourse.SUCESSO_CURSO_ENCONTRADO,
            Sucesso = true,
            Data = courses
        });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(string id)
    {
        var course = await _service.GetCourseID(id); // buscando o curso
        //caso se nulo.
        if (course == null) return BadRequest(new ReturnCourseModel<object>()
        {
            Code = ErrorCourse.ERROR_COURSE_NO_FIND,
            Sucesso = false
        });
        else // senao, deu certo
            return Ok(new ReturnCourseModel<CourseModel>()
            {
                Code = SucessoCourse.SUCESSO_CURSO_ENCONTRADO,
                Sucesso = true,
                Data = course
            });
    }
}

