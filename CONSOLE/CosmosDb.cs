using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using Azure.Cosmos;
using System.Collections.Generic;
using System;
using System.Net;

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

        public async Task<ItemResponse<T>> ReplaceItemAsync<T> (T Object, string id, string partitionKey, string containerId, string databaseId)
        {
            var container = _client.GetContainer(databaseId, containerId);
            var resposta = await container.ReplaceItemAsync<T>(Object, id, new PartitionKey(partitionKey));            
            return resposta;
        }

        public async Task<ItemResponse<T>> AddItemAsync<T>(T Object, string id, string partitionKey, string containerId, string databaseId)
        {
            var container = _client.GetContainer(databaseId, containerId);

            try
            {
                var item = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
            {
                 return await container.CreateItemAsync<T>(Object, new PartitionKey(partitionKey));
            }

            return await container.UpsertItemAsync<T>(Object, new PartitionKey(partitionKey));

        }
    }
}