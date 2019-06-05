using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Shared.Protocol;
using System;
using System.Configuration;

namespace SecureBlob
{
    public static class SharedAccessSignaturesAccount
    {

        public static string GetAccountSASToken()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobStorageConnectionString"]);

            // create a new access policy for the account 

            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write | SharedAccessAccountPermissions.List,
                Services = SharedAccessAccountServices.Blob | SharedAccessAccountServices.File,
                ResourceTypes = SharedAccessAccountResourceTypes.Service,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
                Protocols = SharedAccessProtocol.HttpsOrHttp,
                SharedAccessStartTime=DateTime.UtcNow
            };

            // return the SAS token

            return storageAccount.GetSharedAccessSignature(policy);
        }

        public static void useAccountSAS(string sasToken)
        {
            // create new storage credentials using SAS token 
            StorageCredentials storageCredentials = new StorageCredentials(sasToken);
            // use these credentials and the account name to create a blob service client
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, "uneecopswebappcdn1", endpointSuffix: null, useHttps: true);
            CloudBlobClient blobClientWithSAS = cloudStorageAccount.CreateCloudBlobClient();

            // now set the service properties for the blob client created with the SAS

            blobClientWithSAS.SetServiceProperties(new ServiceProperties()
            {
                  
                // hour metrics 
                 HourMetrics = new MetricsProperties()
                 {
                      MetricsLevel=MetricsLevel.ServiceAndApi,
                      RetentionDays=7,
                      Version="1.0"
                 },
                  MinuteMetrics = new MetricsProperties()
                  {
                       MetricsLevel=MetricsLevel.ServiceAndApi,
                        RetentionDays=7,
                        Version="1.0"
                  },
                Logging = new LoggingProperties()
                  {
                       LoggingOperations=LoggingOperations.All,
                        RetentionDays=14,
                         Version="1.0"
                  },

            });

            // the permission granted by the account SAS also permit you to retrieve service properties

            ServiceProperties serviceProperties = blobClientWithSAS.GetServiceProperties();
        }
    }


}
