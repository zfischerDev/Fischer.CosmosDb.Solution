using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using Fischer.CosmosDb.Solution.PolarisCdbLibraries.Objects;

namespace Fischer.CosmosDb.Solution.PolarisCdbLibraries
{
    public class PolarisCdbDataLibrary
    {
        private static readonly string polarisCosmosDbEndpointUri = @"https://localhost:8081";
        private static readonly string polarisCosmosDbPrimaryKey = @"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private CosmosClient cosmosClient;
        private Database cosmosDatabase;
        private Container cosmosContainer;
        private string polarisDatabaseId = "PolarisCdb";
        private string polarisAccountHolderContainerId = "PolarisAccountHolder";
        private string polarisTransactionContainerId = "PolarisTransaction";

        #region CosmosDb communications
        #endregion
        #region Json
        public string SerializeFormattedJsonToString(PolarisAccountHolder accountHolder)
        {
            //Can also use var here, I just chose to explicitly declare it
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                //allows text to be indented to look nicer
                WriteIndented = true
            };
            string jasonString = JsonSerializer.Serialize<PolarisAccountHolder>(accountHolder, jsonOptions);
            return jasonString;
        }
        #endregion
        public async Task AddAccountHolderToCosmosDb(PolarisAccountHolder accountHolder)
        {
            try
            {
                cosmosClient = new CosmosClient(polarisCosmosDbEndpointUri,polarisCosmosDbPrimaryKey);
                //string jsonString = SerializeFormattedJsonToString(accountHolder);
                cosmosContainer = cosmosClient.GetContainer(polarisDatabaseId, polarisAccountHolderContainerId);
                ItemResponse<PolarisAccountHolder> accountHolderResponse =
                    await cosmosContainer.CreateItemAsync<PolarisAccountHolder>(accountHolder,
                        new PartitionKey(accountHolder.AccountGuid.ToString()));
            }
            catch (Exception exception)
            {
                //Console.WriteLine(e);
                throw exception;
            }
        }
    }
}
