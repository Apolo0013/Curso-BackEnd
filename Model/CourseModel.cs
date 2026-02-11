using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BackEnd.Model.Course
{
    public class CourseAuthor
    {
        public string Name { set; get; } = "";
        public string SrcAvatar { set; get; } = "";
        public string About { set; get; } = "";
    }

    public class CourseModel
    {
        public string Id { get; set; } = "";
        public string Title { set; get; } = "";
        public CourseAuthor Author { set; get; } = new();
        public string Summary { set; get; } = "";
        public string Description { set; get; } = "";
        public decimal Price { set; get; } = 0;
        public string ThumbnailUrl { set; get; } = "";
        public string[] LearningOutcomes { set; get; } = Array.Empty<string>();
        public string[] TargetAudience { set; get; } = Array.Empty<string>();
        public string[] Prerequisites { set; get; } = Array.Empty<string>();
        public string[] CompletionBenefits { set; get; } = Array.Empty<string>();

    }

    //retorno das rotas courses
    public class ReturnCourseModel<T>
    {
        public bool Sucesso { set; get; } = false;
        public string Code { set; get; } = "";
        public T? Data { set; get; }
    }


    //para servi o postgre
    //Cursos
    [Table("courses")]
    public class DbCourses
    {
        [Column("id")]
        public string ID { get; set; } = "";
        [Column("title")]
        public string Title { get; set; } = "";
        [Column("author_name")]
        public string Author_Name { get; set; } = "";
        [Column("author_srcAvatar")]
        public string Author_srcAvatar { get; set; } = "";
        [Column("author_about")]
        public string Author_about { get; set; } = "";
        [Column("summary")]
        public string Summary { get; set; } = "";
        [Column("description")]
        public string Description { get; set; } = "";
        [Column("price")]
        public decimal Price { get; set; }
        [Column("thumbnailUrl")]
        public string ThumbnailUrl { get; set; } = "";
        [Column("learningOutcomes")]
        public string[] LearningOutcomes { get; set; } = Array.Empty<string>();
        [Column("targetAudience")]
        public string[] TargetAudience { get; set; } = Array.Empty<string>();
        [Column("prerequisites")]
        public string[] Prerequisites { get; set; } = Array.Empty<string>();
        [Column("completionBenefits")]
        public string[] CompletionBenefits { get; set; } = Array.Empty<string>();
    }

    //Modulos
    [Table("modules")]
    public class DbModules
    {
        [Column("id")]
        public string Id { set; get; } = "";
        [Column("idCourse")]
        public string IdCourse { set; get; } = "";
        [Column("title")]
        public string Title { set; get; } = "";
        [Column("description")]
        public string Description { set; get; } = "";
        [Column("position")]
        public int Position { set; get; }
    }

    //Classes / Aulas
    [Table("classes")]
    public class DbClasses
    {
        [Column("id")]
        public string Id { set; get; } = "";
        [Column("idModule")]
        public string IdModule { set; get; } = "";
        [Column("position")]
        public int Position { set; get; }
        [Column("description")]
        public string Description { set; get; } = "";
        [Column("durationInSeconds")]
        public int DurationInSeconds { set; get; }
        [Column("video")]
        public string Video { set; get; } = "";
    }
}
