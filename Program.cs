using Google.Cloud.Compute.V1;

// TODO(developer): Set your own default values for these parameters or pass different values when calling this method.
string projectId = "XXXXXXXX";
string zone = "europe-southwest1-a";
string machineName = "luis-test-machine";
string machineType = "e2-micro";
string diskImage = "projects/debian-cloud/global/images/family/debian-10";
long diskSizeGb = 10;
string networkName = "default";

Instance instance = new Instance
{
    Name = machineName,
    // See https://cloud.google.com/compute/docs/machine-types for more information on machine types.
    MachineType = $"zones/{zone}/machineTypes/{machineType}",
    // Instance creation requires at least one persistent disk.
    Disks =
            {
                new AttachedDisk
                {
                    AutoDelete = true,
                    Boot = true,
                    Type = ComputeEnumConstants.AttachedDisk.Type.Persistent,
                    InitializeParams = new AttachedDiskInitializeParams
                    {
                        // See https://cloud.google.com/compute/docs/images for more information on available images.
                        SourceImage = diskImage,
                        DiskSizeGb = diskSizeGb
                    }
                }
            },
    NetworkInterfaces = { new NetworkInterface { Name = networkName } }
};

// Initialize client that will be used to send requests. This client only needs to be created
// once, and can be reused for multiple requests.
InstancesClient client = await InstancesClient.CreateAsync();

// Insert the instance in the specified project and zone.
var instanceCreation = await client.InsertAsync(projectId, zone, instance);

// Wait for the operation to complete using client-side polling.
// The server-side operation is not affected by polling,
// and might finish successfully even if polling times out.
await instanceCreation.PollUntilCompletedAsync();

