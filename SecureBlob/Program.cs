using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureBlob
{
    class Program
    {
        static void Main(string[] args)
        {
            //const string fileName = "AZURE SERVER 20190415.log";
            //const string containerName = "webjoblog";

            const string fileName = "1.png";
            const string containerName = "securedocuments";
            const string policyPrefix = "HR-One-policy-";
            string sharedAccessPolicyName = policyPrefix + DateTime.Now.Ticks.ToString();


           // SharedAccessSignaturesAccount.GetAccountSASToken();
            SharedAccessSignaturesAccount.useAccountSAS(SharedAccessSignaturesAccount.GetAccountSASToken());
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlobStorageConnectionString"));

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            try
            {
                container.CreateIfNotExists();
            }
            catch (StorageException)
            {
                // Ensure that the storage emulator is running if using emulator connection string.
                Console.WriteLine("If you are running with the default connection string, please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                Console.ReadLine();
                throw;
            }


        //    StoredAccessPolicy.CreateSharedAccessPolicyAsync(container, sharedAccessPolicyName);

            //var blob = container.GetBlockBlobReference(fileName);

            //Generate a SAS URI for a blob within the container, using the stored access policy to set constraints on the SAS.
            string sharedPolicyBlobSAS = SharedAccessSignature.GetBlobSasUri(container, fileName, null);
            Console.WriteLine("4. SAS for blob (stored access policy): " + sharedPolicyBlobSAS);
            Console.WriteLine();
            Console.Read();

        }

    }
}
