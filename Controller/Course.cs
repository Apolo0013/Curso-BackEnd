using BackEnd.Service.Course;
using Microsoft.AspNetCore.Mvc;
//model
using BackEnd.Model.Course;
using BackEnd.Model.Auth;
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


    [HttpGet("get/information")]
    public async Task<IActionResult> GetCourseInfomation()
    //Essa rota so vai retorna as informacao sobre o curso, como title, id, descricao, informacao sobre o autor do cursos etc...
    {
        var courses = await _service.GetCourseInfomation();
        return Ok(new ReturnCourseModel<List<CourseModel>>()
        {
            Code = SucessoCourse.SUCESSO_CURSO_ENCONTRADO,
            Sucesso = true,
            Data = courses
        });
    }

    [HttpGet("get/content")]
    public async Task<IActionResult> GetCourseContent([FromQuery] string idUser)
    //Essa rota vai retorna o conteudo em si dos cursos, como modulo e aulas.
    {
        
        var courses = await _service.GetCourseContent(idUser);
        if (courses.Count == 0) // caso esteja vazio
        {
            return Ok(new ReturnCourseModel<object>()
            {
                Sucesso = false,
                Code = "",
                Data = null
            });
        }
        //ele tem cursos comprado
        else
        {
            return Ok(new ReturnCourseModel<List<CourseContentModel>>()
            {
                Code = "",
                Sucesso = true,
                Data = courses
            });
        }
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

