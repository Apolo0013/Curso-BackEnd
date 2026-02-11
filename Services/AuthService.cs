//DTO
using BackEnd.DTO.Auth;
//Model
using BackEnd.Model.Auth;
//Common
using BackEnd.Common.Auth.Error;
using BackEnd.Common.Auth.Sucesso;
using BackEnd.Data.Context;
using AutoMapper;
using System.Text.Json;

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
                return (ErrorAuth.ERROR_EMAIL_EM_USO, "");
            else if (dados.Any(info => info.Name == authdate.Name))
                return (ErrorAuth.ERROR_NOME_EM_USO, "");
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
                return (SucessoAuth.CONTA_REGISTRADA, ID);
            }
        }

        public async Task<UserModel?> GetUserID(string ID)
        {
            var dados = _db.Users; // dados
            // index do usuario de acordo com ID
            var User = dados.FirstOrDefault(info => info.ID == ID);
            if (User == null) return null;
            else
            {
                //convertendo DbUser para UserModel.
                var userModel = new UserModel()
                {
                    Email = User.Email,
                    Password = User.Password,
                    Name = User.Name,
                    ID = User.ID,
                    Role = User.Role
                };
                return userModel;
            }
            
        }

        public async Task<UserModel?> GetUserIndex(int index)
        //essa funcao vai retorna as informacao de um usuario atraves do index
        {
            var dados = _db.Users.ToList();
            if (index < 0 || index >= dados.Count) return null;
            var User = dados[index]; // pegando ousuario
            return new UserModel() // colocando para dbUser -> UserModel
            {
                Email = User.Email,
                Password = User.Password,
                Name = User.Name,
                ID = User.ID,
                Role = User.Role
            };
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
                ? (SucessoAuth.CONTA_ENCONTRADA, indexUser) // deu certo
                : (ErrorAuth.ERROR_USER_NOFIND, 0); // nao deu certo
        }
    }
}
