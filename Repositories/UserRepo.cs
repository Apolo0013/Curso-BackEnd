//DTO
using System.Text.Json;
using BackEnd.Model.User;

namespace BackEnd.Data.User
{
    public class UserRepo
    {
        private readonly string PATHJSON = "";

        public UserRepo(IWebHostEnvironment env)
        {
            PATHJSON = Path.Combine(
                env.ContentRootPath,
                "Repositories",
                "Dados.json"
            );
        }

        public async Task<List<UserModel>> ReadDB()
        {
            //lendo o json.
            string jsonstring = await File.ReadAllTextAsync(PATHJSON);
            //convertendo...
            var json = JsonSerializer.Deserialize<List<UserModel>>(jsonstring);
            return json!;
        }
    }
}
