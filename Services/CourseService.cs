using System.Text.Json;
using BackEnd.Data.Context;
using BackEnd.Model.Auth;
using BackEnd.Model.Course;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BackEnd.Service.Course
{
    public class CourseService
    {
        private readonly AppDbContext _db;

        public CourseService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CourseModel>> GetCourseInfomation()
        {
            var Dbcourses = _db.Courses.ToList();
            //coloca para de DbCourse -> CourseModel
            var courses = Dbcourses.Select(info =>
                new CourseModel()
                {
                    Id = info.ID,
                    Title = info.Title,
                    Author = new CourseAuthor()
                    {
                        Name = info.Author_Name,
                        SrcAvatar = info.Author_srcAvatar,
                        About = info.Author_about
                    },
                    Price = info.Price,
                    ThumbnailUrl = info.ThumbnailUrl,
                    Summary = info.Summary,
                    Description = info.Description,
                    LearningOutcomes = info.LearningOutcomes,
                    CompletionBenefits = info.CompletionBenefits,
                    Prerequisites = info.Prerequisites,
                    TargetAudience = info.TargetAudience
                }
            ).ToList();
            return courses;
        }

        public async Task<List<CourseContentModel>> GetCourseContent(string idUser)
        //aqui ele vai busca 
        {
            //pegando os cursos que o usuario "comprou"
            var userCourses = await _db.UserCourses.Where(info => info.IdUser == idUser).ToListAsync();
            //lista dos cursos
            var listCourses = new List<CourseContentModel>();
            foreach (var iCourse in userCourses)
            {
                var idCourse = iCourse.IdCourse; // id do cursos
                //lista dos modulos
                var listModule = new List<ModuleModel>();
                //Filtrando os modulo de acordo com id do curso.
                var modules = await _db.Modules.Where(x => x.IdCourse == idCourse).ToListAsync();
                //temos os modulos do cursos, agora vamos pecorrer essa lista para add na lista que sera retorna
                foreach (var iModule in modules)
                {
                    //pegando as aulas/classes
                    var classes = await _db.Classes
                        .Where(x => x.IdModule == iModule.Id)
                        .Select(x => new ClassesModel()
                        {
                            IdClass = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            DurationInSeconds = x.DurationInSeconds,
                            Video = x.Video,
                            Order = x.Position
                        })
                        .ToListAsync();
                    //Add na lista
                    listModule.Add(new()
                    {
                        IdModule = iModule.Title,
                        Title = iModule.Title,
                        Description = iModule.Description,
                        Order = iModule.Position,
                        Classes = classes
                    });
                }
                //add na lista dos cursos
                listCourses.Add(new()
                {
                    IdCourse = idCourse,
                    Modules = listModule
                });
                //limpando a lista de modulos para os proximos modulo, de outro cursos
                listModule = new();
            }
            return listCourses;
        }

        public async Task<CourseModel?> GetCourseID(string id)
        {
            var dados = _db.Courses.ToList();
            var courseDB = dados.FirstOrDefault(info => info.ID == id);
            if (courseDB == null) return null;
            else // encontrou.
            {
                //conventendo de DbCouser para CourseModel
                var course = new CourseModel()
                {
                    Id = courseDB.ID,
                    Title = courseDB.Title,
                    Author = new CourseAuthor()
                    {
                        Name = courseDB.Author_Name,
                        About = courseDB.Author_about,
                        SrcAvatar = courseDB.Author_srcAvatar
                    },
                    Summary = courseDB.Summary,
                    Description = courseDB.Description,
                    Price = courseDB.Price,
                    ThumbnailUrl = courseDB.ThumbnailUrl,
                    TargetAudience = courseDB.TargetAudience,
                    CompletionBenefits = courseDB.CompletionBenefits,
                    LearningOutcomes = courseDB.LearningOutcomes,
                    Prerequisites = courseDB.Prerequisites,
                };
                return course;
            }
        }

        public async Task AddCoursesUser(string idUser, string idCourse)
        //Essa funcao vai add o curso "comprado" pelo usuario
        {
            //Add no banco de dados
            _db.Add(new DbUserCourses()
            {
                IdCourse = idCourse,
                IdUser = idUser,
                //aqui era pra "coloca" ele
            });
            //salanvando
            await _db.SaveChangesAsync();
        }

        public async Task<List<DbUserCourses>> GetCoursesUser(string idUser)
        {
            //pegando os dados do banco
            var userCourses = _db.UserCourses; 
            //retornando a lista somente dos cursos que o usuario tem.
            return userCourses.Where(info => info.IdUser == idUser).ToList();
        }
    }
}