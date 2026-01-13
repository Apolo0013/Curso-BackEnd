namespace BackEnd.DTO.Auth
{
    //auth/login receber
    public class LoginDTO
    {
        public string Email { set; get; } = "";
        public string Senha { set; get; } = "";
    }

    //auth/registrar receber
    public class RegistrarDTO
    {
        public string Nome { set; get; } = "";
        public string Email { set; get; } = "";
        public string Senha { set; get; } = "";
    }
}