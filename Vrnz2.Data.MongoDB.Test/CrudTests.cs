using System.Threading.Tasks;
using Vrnz2.Data.MongoDB.Entities.Base;
using Xunit;
using Newtonsoft.Json;

namespace Vrnz2.Data.MongoDB.Test
{
    public class CrudTests
    {
        [Fact]
        public async Task Add()
        {
            var bla = "{'Nome':'Carlos'}";

            string json2 = @"{'Name':'Mike'}";
            var customer2 = JsonConvert.DeserializeAnonymousType(json2, new { Name = "" });


            var connectionString = "mongodb://challenge_user:challenge202012@localhost:32772/Challenge";

            var client = new Client
            {
                Name = "Pedro dos Testes",
                Cpf = "308.929.540-78",
                Estate = "PR"
            };

            using (var mongo = new MongoDB(connectionString, "Client", "Challenge"))
            {
                await mongo.Add(client);
            }
        }
    }

    public class Client
        : BaseMongoDbEntity
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Estate { get; set; }
    }
}
