using Azure.Cosmos;

namespace COSMOSDB_MANUTENCAO
{
    public static class FactoryCosmosClient
    {
        private static string EndpointUrl = "https://poc-cosmos-estudo.documents.azure.com:443/";
        private static string Key = "aviB5I5jwULOWLkWbW6oXq3R3yn2IGen7y4oOBpFIXmFx4huCv3xZBtqszLZLe9LIUvT9QFgCctaiArLsUxpqQ==";
        public static CosmosClient  Get(){
            return new CosmosClient(EndpointUrl, Key);
        }
    }
}