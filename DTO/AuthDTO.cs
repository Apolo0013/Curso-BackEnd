namespace BackEnd.DTO.Auth
{
    //auth/login receber
    public class LoginDTO
    {
        public string Email { set; get; } = "";
        public string Password { set; get; } = "";
    }

    //auth/registrar receber
    public class RegistrarDTO
    {
        public string Name { set; get; } = "";
        public string Email { set; get; } = "";
        public string Password { set; get; } = "";
    }
}