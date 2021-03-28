using Azure.Cosmos;

namespace COSMOSDB_MANUTENCAO
{
    public static class FactoryCosmosClient
    {
        private static string EndpointUrl = "https://poc-cosmos-estudo.documents.azure.com:443/";
        private static string Key = "nLr2rvdupS22xr6dyz3dX3GToJt2lo4Utha7xCgBvShol4uBWtjAXdnvtm3ju2FgvkyUHmsZ2Nq5vdgJfjjy7w==";
        public static CosmosClient  Get(){
            return new CosmosClient(EndpointUrl, Key);
        }
    }
}