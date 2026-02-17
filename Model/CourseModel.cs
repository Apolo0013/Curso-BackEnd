using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BackEnd.Model.Course
{
    public record BodyCompletedClass (
        string IdUser,
        string IdCourse,
        string IdModule,
        string IdClass
    );

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
        [Column("title")]
        public string Title { set; get; } = "";
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
    
    //Progresso...
    //Progressos das aulas
    [Table("users_completed_classes")]
    public class DbClassesProgress
    {
        [Key]
        [Column("id")]
        public string Id { set; get; } = Guid.NewGuid().ToString();
        [Column("idUser")]
        public string IdUser { set; get; } = "";
        [Column("idCourse")]
        public string IdCourse { set; get; } = "";
        [Column("idModule")]
        public string IdModule { set; get; } = "";
        [Column("idClass")]
        public string IdClass { set; get; } = "";
    }
}
