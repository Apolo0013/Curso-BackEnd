using System.Text.Json;
//Model
using BackEnd.Model.Course;

namespace BackEnd.Data.Course
{
    class CourseRepo
    {
        private readonly string PATHJSON = "";

        public CourseRepo(IWebHostEnvironment env)
        {
            PATHJSON = Path.Combine(
                env.ContentRootPath,
                "Repositories",
                "Course.json"
            );
        }

        public static async Task<List<CourseModel>> Read()
        {
            string PATHJSON = @"C:\Users\apolo\Downloads\Curso\Curso-backEnd\Repositories\Course.json";
            string jsonstring = await File.ReadAllTextAsync(PATHJSON);
            //Deserializando.
            var dados = JsonSerializer.Deserialize<List<CourseModel>>(jsonstring);
            //Confia nunca sera null
            return dados!;
        }
    }
}
