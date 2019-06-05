using Microsoft.Azure.Storage.Blob;
using System;

namespace SecureBlob
{
    public static class SharedAccessSignature
    {
        internal static string GetContainerSasUri(CloudBlobContainer container, string storedPolicyName)
        {
            string sasContainerToken;

            // if no stored policy is specified, create a new access poloicy and define it's constraints.

            if (string.IsNullOrEmpty(storedPolicyName))
            {
                // note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad hoc SAS ,
                // and to construct a shared access policy that is saved to the container's shared access policies.
                SharedAccessBlobPolicy adHocPolicy = new SharedAccessBlobPolicy()
                {
                    // when the start time for the SAS is omitted, the start time is assumed to be the time when the storage service recevies the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.

                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List
                };

                // Generate the shared access signature on the container, setting the constraints directly on the signature.
                sasContainerToken = container.GetSharedAccessSignature(adHocPolicy, null);
            }
            else
            {
                // Generate the shared access signature on the container. In this case, all of the constraints for the 
                // shared access signature are specified on the stored access policy, which is provided by name.
                // Is is also possible to specify some constraints on an ad hoc SAS and others on the stored access policy.
                sasContainerToken = container.GetSharedAccessSignature(null, storedPolicyName);
            }
            return container.Uri + sasContainerToken;
        }


        internal static string GetBlobSasUri(CloudBlobContainer container, string blobName, string policyName)
        {
            string sasBlobToken;

            // Get a reference to a blob within the container.
            // note that the blob may not exist yet, but a SAS can still be created for it.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (string.IsNullOrEmpty(policyName))
            {
                // create a new access policy and define its constraints.
                // note that the shared access blob policy class is used both to define the parameters of an ad hoc SAS,
                // and to construct a shared access policy that is saved to the container's shared access policies.
                SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
                {
                    // when the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                    // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List,
                    SharedAccessStartTime=DateTime.UtcNow
                    
                };
                sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);
            }
            else
            {
                // generate the shared access signature on the blob. in this case, all of the constraints for the
                // shared access signature are specified on the container's stored access policy.
                sasBlobToken = blob.GetSharedAccessSignature(null, policyName);
            }
            return blob.Uri  + sasBlobToken;
        }
    }
}
