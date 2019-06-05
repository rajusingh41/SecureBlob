using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace SecureBlob
{
    public static class StoredAccessPolicy
    {
        internal static async Task CreateSharedAccessPolicyAsync(CloudBlobContainer container,string policyName)
        {
            // create a new shared access policy and define its constraints.
            // the access policy provides create, write, read, list and delete permission.
            SharedAccessBlobPolicy sharedAccessBlobPolicy = new SharedAccessBlobPolicy()
            {
                // when the start time for the SAS is omitted, the start time is assumed to be the time 
                // when the storage service receives the request.
                // Omitting the satart time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write
                               | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Delete | SharedAccessBlobPermissions.Create
            };

            // Get the container's existing permissions.
            BlobContainerPermissions permissions = await container.GetPermissionsAsync();

            // Add the new policy to the container's permission and set the container's permissions.

            permissions.SharedAccessPolicies.Add(policyName, sharedAccessBlobPolicy);
            await container.SetPermissionsAsync(permissions);
        }
    }
}
