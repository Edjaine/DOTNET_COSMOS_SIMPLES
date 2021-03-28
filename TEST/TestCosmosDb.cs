using System.Threading.Tasks;
using CONSOLE.Model;
using COSMOSDB_MANUTENCAO;
using NUnit.Framework;

namespace TEST
{
    public class TestCosmosDb
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Testa_Criacao_Container()
        {
            //Arrange
            var client = new CosmosDb();
            //Act
            var resposta = await client.CreateContainer("Agente", "tipo", "poc-db");
            //Assert
            Assert.Contains(resposta.GetRawResponse().Status, new int[]{200, 201});
        }
        [Test]
        public async Task Testa_Leitura_dos_Dados() 
        {
            //Arrange
            var client = new CosmosDb();
            //Act
            var resposta = await client.GetItensAsync<Agente<Cliente>>("Agente", "poc-db", "SELECT * FROM c");
            //Assert
            Assert.IsTrue(resposta.Count > 0, "A lista retornada é vazia");
            Assert.IsNotEmpty(resposta[0].documento.nome);
        }
    }
}