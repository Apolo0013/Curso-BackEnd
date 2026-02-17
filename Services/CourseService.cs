using BackEnd.Data.Context;
using BackEnd.Model.Auth;
using Microsoft.EntityFrameworkCore;
//DTO
using BackEnd.DTO.Course;
using System.Text.Json;
using BackEnd.Model.Course;
using System.Collections.Frozen;

namespace BackEnd.Service.Course
{
    public class CourseService
    {
        private readonly AppDbContext _db;

        public CourseService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CourseDTO>> GetCourseInfomation()
        {
            var Dbcourses = _db.Courses.ToList();
            //coloca para de DbCourse -> CourseModel
            var courses = Dbcourses.Select(info =>
                new CourseDTO()
                {
                    Id = info.ID,
                    Title = info.Title,
                    Author = new CourseAuthorDTO()
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

        public async Task<List<CourseContentDTO>> GetCourseContent(string idUser)
        //aqui ele vai busca 
        {
            //pegando os cursos que o usuario "comprou"
            var userCourses = await _db.UserCourses.Where(info => info.IdUser == idUser).ToListAsync();
            //lista dos cursos
            var listCourses = new List<CourseContentDTO>();
            foreach (var iCourse in userCourses)
            {
                var idCourse = iCourse.IdCourse; // id do cursos
                //lista dos modulos
                var listModule = new List<ModuleDTO>();
                //Filtrando os modulo de acordo com id do curso.
                var modules = await _db.Modules.Where(x => x.IdCourse == idCourse).ToListAsync();
                //temos os modulos do cursos, agora vamos pecorrer essa lista para add na lista que sera retorna
                foreach (var iModule in modules)
                {
                    //pegando as aulas/classes
                    var classes = await _db.Classes
                        .Where(x => x.IdModule == iModule.Id)
                        .Select(x => new ClassesDTO()
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

        public async Task<CourseDTO?> GetCourseID(string id)
        {
            var dados = _db.Courses.ToList();
            var courseDB = dados.FirstOrDefault(info => info.ID == id);
            if (courseDB == null) return null;
            else // encontrou.
            {
                //conventendo de DbCouser para CourseModel
                var course = new CourseDTO()
                {
                    Id = courseDB.ID,
                    Title = courseDB.Title,
                    Author = new CourseAuthorDTO()
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

        public async Task<bool> AddCoursesUser(string idUser, string idCourse)
        //Essa funcao vai add o curso "comprado" pelo usuario
        {
            //Add no banco de dados
            _db.Add(new DbUserCourses()
            {
                IdCourse = idCourse,
                IdUser = idUser,
            });
            await _db.SaveChangesAsync();
            //pegando id do module ondem esta o a primeira aula
            var modules = await _db.Modules.ToListAsync();
            var FirstModule = modules
                .Where(x => x.IdCourse == idCourse)
                .FirstOrDefault(x => x.Position == 1);
            string? idModule = FirstModule!.Id ?? null;
            if (idModule == null) return false; // falando que algo deu errado
            //pegando o id do class/aula
            var classes = await _db.Classes.ToListAsync();
            var FirstClass = classes
                .Where(x => x.IdModule == idModule)
                .FirstOrDefault(x => x.Position == 1);
            string? idClass = FirstClass!.Id ?? null;
            //caso for null
            if (idClass == null) return false;
            //add a primeira aula dele
            _db.Add(new DbClassesProgress()
            {
                Id = Guid.NewGuid().ToString(),
                IdCourse = idCourse,
                IdUser = idUser,
                IdModule = idModule,
                IdClass = idClass
            });
            //salvando
            await _db.SaveChangesAsync();
            //deu tudo certo
            return true;
        }

        public async Task<List<DbUserCourses>> GetCoursesUser(string idUser)
        {
            //pegando os dados do banco
            var userCourses = _db.UserCourses;
            //retornando a lista somente dos cursos que o usuario tem.
            return userCourses.Where(info => info.IdUser == idUser).ToList();
        }

        public async Task<List<DbClassesProgress>> GetCourseProgress(string idCourse, string idUser)
        {
            var dataProgress = await _db.ClassesProgress.ToListAsync();
            Console.WriteLine(JsonSerializer.Serialize(dataProgress.Select(x => x.IdCourse)));
            Console.WriteLine(dataProgress.Any(x => x.IdCourse == idCourse));
            Console.WriteLine(dataProgress.Any(x => x.IdUser == idUser));
            return dataProgress.Where(x => x.IdCourse == idCourse && x.IdUser == idUser).ToList();
        }
    
        public async Task CompletedClass(BodyCompletedClass body)
        // EXPLICACAO: Aqui vamos receber aula que ele "completou", ele ja tem essa aula, ele pode assistir, etc, mas quando ele fala que "assistiu" vamos liberar a proxima aula
        //EX: Usuario contem aula 1. *Por padrao ao comprar o curso, a primeira aula sera liberar para assistir
        // Em aula 1 ele vai aperta o botao de "marca como concluida". A parti disso, vamos ver qual é proxima aula a se liberar pelo order/position.
        //Visao: aula 1 liberar aula 2, aula 2 liberar aula 3... Assim por diante
        {
            //Add a aula "concluida"
            var classes = await _db.Classes.ToListAsync();
            var FirstClasses = classes.FirstOrDefault(x => x.Id == body.IdClass);
            if (FirstClasses == null) return;
            //proximo position/order
            var nextPosition = FirstClasses.Position + 1;
            //Agora vamos pegar o nextPosition pegar a class conrespondente
            //*Aqui preciso que position seja igual ao nextPosition e IdModule seja o mesmo que foi fornecido.
            var FirstclasseTarget = classes
            .FirstOrDefault(
                x => x.Position == nextPosition 
                && x.IdModule == body.IdModule
            );
            //caso seja null
            if (FirstclasseTarget == null) return;
            await _db.AddAsync(new DbClassesProgress()
            {
                Id = Guid.NewGuid().ToString(),
                IdClass = FirstclasseTarget.Id,
                IdModule = FirstclasseTarget.IdModule,
                IdCourse = body.IdCourse,
                IdUser = body.IdUser
            });
            //salvando no bnaco
            await _db.SaveChangesAsync();
        }
    }
}