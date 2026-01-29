//Data
using BackEnd.Data.User;
//DTO
using BackEnd.DTO.Auth;
//Model
using BackEnd.Model.Auth;
//Common
using BackEnd.Common.ErrorMSGAuth;
using BackEnd.Common.SucessoMSGAuth;
using BackEnd.Data.Context;

namespace BackEnd.Service.Auth
{
    public class AuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        //Metedo responsavel por Registrar o usuario.
        public async Task<(string msg, string id)> AuthRegister(RegistrarDTO authdate)
        {
            var dados = _db.Users;
            //se existi pessoa com o mesmo email.
            if (dados.Any(info => info.Email == authdate.Email))
                return (ErrorMSGAuth.ERROR_EMAIL_EM_USO, "");
            else if (dados.Any(info => info.Name == authdate.Name))
                return (ErrorMSGAuth.ERROR_NOME_EM_USO, "");
            else // nenhum dados repetido...
            {
                string ID = Guid.NewGuid().ToString("N");
                var user = new UserModel()
                {
                    Email = authdate.Email,
                    Name = authdate.Name,
                    Password = authdate.Password,
                    ID = ID,
                    Role = "USER" // por padrao geral sera USER
                };
                //registra no banco de dados
                _db.Add(user);
                await _db.SaveChangesAsync();
                return (SucessoMSGAuth.CONTA_REGISTRADA, ID);
            }
        }

        public async Task<UserModel?> GetUserID(string ID)
        {
            var dados = _db.Users; // dados
            // index do usuario de acordo com ID
            var User = dados.FirstOrDefault(info => info.ID == ID);
            return User;
        }

        public async Task<UserModel?> GetUserIndex(int index)
        //essa funcao vai retorna as informacao de um usuario atraves do index
        {
            var dados = _db.Users.ToList();
            if (index < 0 || index >= dados.Count) return null;
            return dados[index];
        }

        public async Task<(string msg, int index)> CheckLoginUser(LoginDTO login)
        // Ele vai ver se o usuario existir.
        // ele pode retorna:
        // email errado
        // usuario nao encontrado.
        {
            var dados = _db.Users.ToList();
            int indexUser = dados.FindIndex(info =>
                info.Email == login.Email &&    // email é igual
                info.Password == login.Password // senha tambem é igual
            );          
            return indexUser != -1
                ? (SucessoMSGAuth.CONTA_ENCONTRADA, indexUser) // deu certo
                : (ErrorMSGAuth.ERROR_USER_NOFIND, 0); // nao deu certo
        }
    }
}
