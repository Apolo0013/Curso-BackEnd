namespace BackEnd.DTO.Course
{
        public class CourseAuthorDTO
    {
        public string Name { set; get; } = "";
        public string SrcAvatar { set; get; } = "";
        public string About { set; get; } = "";
    }


    public class CourseDTO
    {
        public string Id { get; set; } = "";
        public string Title { set; get; } = "";
        public CourseAuthorDTO Author { set; get; } = new();
        public string Summary { set; get; } = "";
        public string Description { set; get; } = "";
        public decimal Price { set; get; } = 0;
        public string ThumbnailUrl { set; get; } = "";
        public string[] LearningOutcomes { set; get; } = Array.Empty<string>();
        public string[] TargetAudience { set; get; } = Array.Empty<string>();
        public string[] Prerequisites { set; get; } = Array.Empty<string>();
        public string[] CompletionBenefits { set; get; } = Array.Empty<string>();

    }

    //Conteudo dos cursos
    public class CourseContentDTO()
    {
        public string IdCourse { set; get; } = "";
        public List<ModuleDTO> Modules { set; get; } = new();
    }

    public class ModuleDTO
    {
        public string IdModule { set; get; } = "";
        public string Title { set; get; } = "";
        public string Description { set; get; } = "";
        public int Order { set; get; }
        public List<ClassesDTO> Classes { set; get; } = new();
    }

    public class ClassesDTO
    {
        public string IdClass { set; get; } = "";
        public string Title { set; get; } = "";
        public string Description { set; get; } = "";
        public int DurationInSeconds { set; get; }
        public int Order { set; get; }
        public string Video { set; get; } = "";
    }


    //retorno das rotas courses
    public class APIResponseCourse<T>
    {
        public bool Sucesso { set; get; } = false;
        public string Code { set; get; } = "";
        public T? Data { set; get; }
    }

}