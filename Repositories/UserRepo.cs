//DTO
using System.Text.Json;
using BackEnd.Model.Auth;

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

        // ele vai add os dados, ele receber apenas o user pra add.
        public async Task AddDB(UserModel user)
        {
            // lendo
            var dados = await ReadDB();
            // alterando / add
            dados.Add(user);
            //transformando o dados em string
            var dadosString = JsonSerializer.Serialize(dados, new JsonSerializerOptions() {WriteIndented = true});
            //escrevendo...
            await File.WriteAllTextAsync(PATHJSON, dadosString);
        }
    }
}
