using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using CONSOLE.Model;
using COSMOSDB_MANUTENCAO;
using NUnit.Framework;

namespace TEST
{
    public class TestCosmosDb
    {
        private Mapper _mapper;

        [SetUp]
        public void Setup()
        {

            _mapper = new Mapper(new MapperConfiguration( cfg => {
                cfg.CreateMap<AgenteViewModel<ClienteViewModel>, AgenteViewModel<NovoClienteViewModel>>()                                
                .ForPath(d => d.documento.idade, o => o.MapFrom( s => int.Parse(s.documento.idade.ToString())))
                .ForPath(d => d.documento.nome, o => o.MapFrom( s => s.documento.nome));
            }));

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
            var resposta = await client.GetItensAsync<AgenteViewModel<NovoClienteViewModel>>("Agente", "poc-db", "SELECT * FROM c");
            //Assert
            await TestContext.Out.WriteLineAsync(resposta[0].documento.idade.ToString());
            Assert.IsTrue(resposta.Count > 0, "A lista retornada é vazia");                        
            Assert.IsNotEmpty(resposta[0].documento.nome);
        }

        [Test]
        public async Task Substitui_Um_Objeto_Alterando_Seu_Tipo()
        {
            //Arrage
            var client = new CosmosDb();
            var listaItens = await client.GetItensAsync<AgenteViewModel<ClienteViewModel>>("Agente", "poc-db", "SELECT * FROM c");
            var listaItensNovos = new List<AgenteViewModel<NovoClienteViewModel>>();
            //Act
            listaItens.ToList().ForEach( async i => {

                var novoAgente = _mapper.Map<AgenteViewModel<NovoClienteViewModel>>(i);                
                var resp = await client.ReplaceItemAsync<AgenteViewModel<NovoClienteViewModel>>(novoAgente, novoAgente.id, novoAgente.tipo, "Agente", "poc-db");                
                listaItensNovos.Add(resp);

            });
            //Assert
            Assert.IsTrue(listaItens.Count() > 0);

        }

        [Test]
        public async Task Inclui_Novo_Objeto_Banco() 
        {
            //Arrage
            var client = new CosmosDb();
            var cliente = new AgenteViewModel<NovoClienteViewModel>() {
                documento = new NovoClienteViewModel(new Faker().Person.FullName, new Faker().Random.Int(0, 90)),
                id = Guid.NewGuid().ToString(),
                tipo = "Cliente"                
            };
            //Act
            var resposta = await client.AddItemAsync<AgenteViewModel<NovoClienteViewModel>>(cliente, cliente.id, cliente.tipo, "Agente", "poc-db");
            //Assert

            Assert.Contains(resposta.GetRawResponse().Status, new int[] { (int)HttpStatusCode.Accepted, (int)HttpStatusCode.Created });
        


        }
    }
}