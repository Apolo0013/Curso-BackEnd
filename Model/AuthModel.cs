using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Model.Auth
{
    //usuario informacao, tambem usado pra indentifica a tabela no db.
    public class UserModel
    {
        public string ID { set; get; } = "";
        public string Name { set; get; } = "";
        public string Email { set; get; } = "";
        public string Password { set; get; } = "";
        public string Role { set; get; } = "";
    }


    //retorno das rota /auth/login e /auth/registrar
    public class ReturnAuthModel
    {
        public bool Sucesso { set; get; } = false;
        public string Code { set; get; } = "";
    }

    //retorno da rota /auth/user
    public class TUser
    {
        public string Role { set; get; } = "";
        public string Nome { set; get; } = "";
        public string ID { set; get; } = "";
    }


    //Servi ao db
    [Table("users")]
    public class DbUser
    {
        [Column("id")]
        public string ID { get; set; } = "";
        [Column("name")]
        public string Name { set; get; } = "";
        [Column("email")]
        public string Email { set; get; } = "";
        [Column("password")]
        public string Password { set; get; } = "";
        [Column("role")]
        public string Role { set; get; } = "";
    }

    [Table("users_courses")]
    public class DbUserCourses
    {
        [Key]
        [Column("idUser")]
        public string IdUser { set; get; } = "";
        [Column("idCourse")]
        public string IdCourse { set; get; } = "";
        [Column("purchasedAt")]
        public DateTime PurchasedAt { set; get; }
    }
}