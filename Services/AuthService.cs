//Data
using BackEnd.Data.User;
//DTO
using BackEnd.DTO.Auth;
//Model
using BackEnd.Model.User;
//Common
using BackEnd.Common.ErrorMSGAuth;
using BackEnd.Common.SucessoMSGAuth;
using System.Diagnostics.Eventing.Reader;
using System.Data;

namespace BackEnd.Service.Auth
{
    public class AuthService
    {
        private readonly UserRepo _repoUser;

        public AuthService(UserRepo repo)
        {
            _repoUser = repo;
        }

        //Metedo responsavel por Registrar o usuario.
        public async Task<string> AuthRegister(RegistrarDTO authdate)
        {
            var dados = await _repoUser.ReadDB();
            //se existi pessoa com o mesmo email.
            if (dados.Any(info => info.Email == authdate.Email))
                return ErrorMSGAuth.ERROR_EMAIL_EM_USO;
            else if (dados.Any(info => info.Nome == authdate.Nome))
                return ErrorMSGAuth.ERROR_EMAIL_EM_USO;
            else // nenhum dados repetido...
            {
                return "";
            }
        }

        public async Task<UserModel?> GetUserID(string ID)
        {
            var dados = await _repoUser.ReadDB(); // dados
            // index do usuario de acordo com ID
            int indexUser = dados.FindIndex(info => info.ID == ID);
            return indexUser == -1
                ? null // esse usuario nao existi 
                : dados[indexUser]; // retorna o usuario.
        }

        public async Task<UserModel?> GetUserIndex(int index)
        //essa funcao vai retorna as informacao de um usuario atraves do index
        {
            var dados = await _repoUser.ReadDB();
            if (index < 0 || index >= dados.Count) return null;
            return dados[index];
        }

        public async Task<(string msg, int index)> CheckLoginUser(LoginDTO login)
        // Ele vai ver se o usuario existir.
        // ele pode retorna:
        // email errado
        // usuario nao encontrado.
        {
            var dados = await _repoUser.ReadDB();
            int indexUser = dados.FindIndex(info =>
                info.Email == login.Email && // email é igual
                info.Senha == login.Senha    // senha tambem é igual
            );          
            return indexUser != -1
                ? (SucessoMSGAuth.CONTA_ENCONTRADA, indexUser) // deu certo
                : (ErrorMSGAuth.ERROR_USER_NOFIND, 0); // nao deu certo
        }

        public async Task<bool> UserExits(string id)
        //Vai retorna volor bool se o usuario existi no banco de dados ou nao.
        {
            var dadosUser = await _repoUser.ReadDB(); // lendo o banco de dados
            bool find = dadosUser.Any(info => info.ID == id); // true se ele o id exist no banco de dados/json
            return find; //retornando o resultado
        }
    }
}
