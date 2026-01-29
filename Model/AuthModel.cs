using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Model.Auth
{
    //usuario informacao, tambem usado pra indentifica a tabela no db.
    [Table("users")]
    public class UserModel
    {
        [Column("id")]
        public string ID { set; get; } = "";
        [Column("name")]
        public string Name { set; get; } = "";
        [Column("email")]
        public string Email { set; get; } = "";
        [Column("password")]
        public string Password { set; get; } = "";
        [Column("role")]
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
}