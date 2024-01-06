using System.Text.Json;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Basic.Util;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var client = new ArmClient(Credential.credential, Credential.SubscriptionId);
        if (client == null) { Log.Alert("Client is null"); return; }

        var subscription = await client.GetDefaultSubscriptionAsync();
        if (subscription == null) { Log.Alert("subscription is null"); return; }

        var resourceGroupCollection = subscription.GetResourceGroups();
        Log.Out($"resource group collection count {resourceGroupCollection.Count()}");
        // await ListResources(resourceGroupCollection);
        var rgName = "test-rg-001";
        // await MakeResourceGroup(subscription, rgName);
        await DeleteResourceGroup(subscription, rgName);
    }

    private static async Task DeleteResourceGroup(SubscriptionResource subscription, string rgName)
    {
        var rgId = ResourceGroupResource.CreateResourceIdentifier(subscription.Id, rgName);

        var rg = await subscription.GetResourceGroupAsync(rgName);
        var result = await rg.Value.DeleteAsync(Azure.WaitUntil.Completed);
        System.Console.WriteLine(JsonSerializer.Serialize(await result.UpdateStatusAsync()));
    }

    private static async Task MakeResourceGroup(SubscriptionResource subscription, string newRgName)
    {
        var rgCollection = subscription.GetResourceGroups();
        var location = AzureLocation.JapanEast;
        var rgData = new ResourceGroupData(location);
        rgData.Tags.Add("develop", "test");
        var rgOp = await rgCollection.CreateOrUpdateAsync(Azure.WaitUntil.Completed, newRgName,rgData);
        Log.Ok(JsonSerializer.Serialize(rgOp.Value));
    }

    private static async Task ListResources(ResourceGroupCollection resourceGroupCollection)
    {
        await foreach (var resourceGroup in resourceGroupCollection)
        {
            Log.Alert(JsonSerializer.Serialize(resourceGroup.Data.Name));
            await foreach (var resource in resourceGroup.GetGenericResourcesAsync())
            {
                Log.Alert($"name {resource.Id.Name}");
                Log.Ok($"resource : {JsonSerializer.Serialize(resource.Data)}");
            }
        }
    }
}