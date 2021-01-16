using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
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
        //private static readonly string polarisCosmosDbEndpointUri = @"<ADD URI HERE>";
        //private static readonly string polarisCosmosDbPrimaryKey = @"<ADD PRIMAY KEY HERE>";
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

        #region AccountHolder

        public async Task<List<PolarisAccountHolder>> GetAllAccountHolders(string polarisCosmosDbEndpointUri, string polarisCosmosDbPrimaryKey)
        {
            List<PolarisAccountHolder> polarisAccountHolderList = new List<PolarisAccountHolder>();
            try
            {
                cosmosClient = new CosmosClient(polarisCosmosDbEndpointUri, polarisCosmosDbPrimaryKey);
                cosmosContainer = cosmosClient.GetContainer(polarisDatabaseId, polarisAccountHolderContainerId);

                string sqlString = "SELECT acctHldrTable.AccountGuid,acctHldrTable.AccountHolder, " +
                                   "acctHldrTable.AccountType FROM acctHldrTable";
                QueryDefinition selectAccountDefinition = new QueryDefinition(sqlString);
                FeedIterator<PolarisAccountHolder> feedResultIterator =
                    cosmosContainer.GetItemQueryIterator<PolarisAccountHolder>(selectAccountDefinition);
                //Iterate through the results
                while (feedResultIterator.HasMoreResults)
                {
                    FeedResponse<PolarisAccountHolder> feedResponse = await feedResultIterator.ReadNextAsync();

                    foreach (var polarisAccountHolder in feedResponse)
                    {
                        polarisAccountHolderList.Add(polarisAccountHolder);
                    }
                }
 
                return polarisAccountHolderList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task AddAccountHolderToCosmosDb(PolarisAccountHolder accountHolder, string polarisCosmosDbEndpointUri, string polarisCosmosDbPrimaryKey)
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

        #endregion

        #region Transaction

        public async Task<List<PolarisTransaction>> GetAllTransactions(string polarisCosmosDbEndpointUri, string polarisCosmosDbPrimaryKey)
        {
            List<PolarisTransaction> polarisTransactionList = new List<PolarisTransaction>();
            try
            {
                cosmosClient = new CosmosClient(polarisCosmosDbEndpointUri, polarisCosmosDbPrimaryKey);
                cosmosContainer = cosmosClient.GetContainer(polarisDatabaseId, polarisTransactionContainerId);

                string sqlString = "SELECT * FROM acctTransactionTable";
                QueryDefinition selectTransactionDefinition = new QueryDefinition(sqlString);
                FeedIterator<PolarisTransaction> feedResultIterator =
                    cosmosContainer.GetItemQueryIterator<PolarisTransaction>(selectTransactionDefinition);
                //Iterate through the results
                while (feedResultIterator.HasMoreResults)
                {
                    FeedResponse<PolarisTransaction> feedResponse = await feedResultIterator.ReadNextAsync();

                    foreach (var polarisTransaction in feedResponse)
                    {
                        polarisTransactionList.Add(polarisTransaction);
                    }
                }

                return polarisTransactionList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddTransactionToCosmosDb(PolarisTransaction accountTransaction, string polarisCosmosDbEndpointUri, string polarisCosmosDbPrimaryKey)
        {
            try
            {
                cosmosClient = new CosmosClient(polarisCosmosDbEndpointUri, polarisCosmosDbPrimaryKey);
                //string jsonString = SerializeFormattedJsonToString(accountHolder);
                cosmosContainer = cosmosClient.GetContainer(polarisDatabaseId, polarisTransactionContainerId);
                ItemResponse<PolarisTransaction> accountTransactionResponse =
                    await cosmosContainer.CreateItemAsync<PolarisTransaction>(accountTransaction,
                        new PartitionKey(accountTransaction.TransactionGuid.ToString()));
            }
            catch (Exception exception)
            {
                //Console.WriteLine(e);
                throw exception;
            }
        }

        #endregion
    }
}
