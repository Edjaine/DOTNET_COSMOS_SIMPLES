using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using Azure.Cosmos;
using System.Collections.Generic;
using System;

namespace COSMOSDB_MANUTENCAO
{
    public class CosmosDb
    {
        private CosmosClient _client;

        public CosmosDb()
        {
            _client = FactoryCosmosClient.Get();
        }
        public async Task<ContainerResponse> CreateContainer(string containerId, string partitionKey, string databaseId)
        {            
            var pk =$"/{partitionKey.ToLower()}" ;
            return await _client.GetDatabase(databaseId).CreateContainerIfNotExistsAsync(containerId,pk);            
        }        

        public async Task<IList<T>> GetItensAsync<T>(string containerId, string databaseId, string query) 
        {
            var container = _client.GetContainer(databaseId, containerId);
            var queryDefinition = new QueryDefinition(query);            
            var itens = new List<T>();

            await foreach (var item in  container.GetItemQueryIterator<T>(queryDefinition)){
                itens.Add(item);
            }
            
            return itens;
        }
    }
}