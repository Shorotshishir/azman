using Azure.Identity;

// // // Dinf key in drive
public class Credential
{
    private static string tenantId = "";
    public static string SubscriptionId = "";
    private static string clientId = "";
    private static string AppName = "adt-app";
    private static string ApplicationId = "";
    private static string ObjectName = "";
    
    private static string clientSecret = "";
    private static string clientSecretValue = "";
    private static string clientSecretId = "";
    public static ClientSecretCredential credential = new ClientSecretCredential(
        tenantId,
        clientId,
        clientSecretValue
    );
}