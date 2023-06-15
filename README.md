# GoogleCloud_Sample1-Create-VM

This code is written in C# and uses the Google Cloud Compute API to create a virtual machine instance in Google Cloud Platform (GCP). Let's go through the code step by step to understand its functionality.

## 1. Importing the necessary namespaces:
using Google.Cloud.Compute.V1;

This line imports the required namespace for accessing the Google Cloud Compute API.

## 2. Setting up default parameter values:

```csharp
string projectId = "XXXXXXXXXX";
string zone = "europe-southwest1-a";
string machineName = "luis-test-machine";
string machineType = "e2-micro";
string diskImage = "projects/debian-cloud/global/images/family/debian-10";
long diskSizeGb = 10;
string networkName = "default";
```
These variables hold the default values for various parameters required to create a virtual machine instance. You can modify these values to fit your specific requirements.

## 3. Creating an instance object:

```csharp
Instance instance = new Instance
{
    Name = machineName,
    MachineType = $"zones/{zone}/machineTypes/{machineType}",
    Disks =
    {
        new AttachedDisk
        {
            AutoDelete = true,
            Boot = true,
            Type = ComputeEnumConstants.AttachedDisk.Type.Persistent,
            InitializeParams = new AttachedDiskInitializeParams
            {
                SourceImage = diskImage,
                DiskSizeGb = diskSizeGb
            }
        }
    },
    NetworkInterfaces = { new NetworkInterface { Name = networkName } }
};
```

An Instance object is created with the specified properties. The Name property is set to the provided machineName. The MachineType property is set to the specific machine type based on the zone and machineType variables. The Disks property is set to an AttachedDisk object, which represents a persistent disk attached to the instance. The AutoDelete property is set to true indicating that the disk should be automatically deleted when the instance is deleted. The Boot property is set to true to indicate that this is the boot disk for the instance. The Type property is set to Persistent indicating it is a persistent disk. The InitializeParams property contains the initialization parameters for the disk, including the SourceImage and DiskSizeGb.

## 4. Initializing the client:
An InstancesClient object is created using the CreateAsync method. This client is used to send requests to the Google Cloud Compute API.
```csharp
InstancesClient client = await InstancesClient.CreateAsync();
```

## 5. Inserting the instance:
The InsertAsync method is called on the client object to insert the instance into the specified project and zone. The method returns an Operation object representing the asynchronous operation for creating the instance.
```csharp
var instanceCreation = await client.InsertAsync(projectId, zone, instance);
```

## 6. Polling for the completion of the operation:
```csharp
await instanceCreation.PollUntilCompletedAsync();
```

The PollUntilCompletedAsync method is called on the instanceCreation object to wait until the operation is completed. This is a client-side polling mechanism, and the server-side operation is not affected by it. The method will continue to poll until the operation is completed or times out.

That's an overview of the provided code. It sets up default parameters, creates an instance object, initializes the client, inserts the instance, and waits for the operation to complete.

## Code

```csharp
using Google.Cloud.Compute.V1;

// TODO(developer): Set your own default values for these parameters or pass different values when calling this method.
string projectId = "focus-cache-387205";
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
```
