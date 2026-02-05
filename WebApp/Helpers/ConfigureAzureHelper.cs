using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace WebApp.Helpers;
public static class ConfigureAzureHelper
{
    public static IHostBuilder ConfigureKeyVault(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            IConfigurationRoot builtConfig = config.Build();

            if (context.HostingEnvironment.IsDevelopment())
            {
                config.AddAzureKeyVault(
                    new Uri(builtConfig["KeyVault:BaseUrl"]),
                    new DefaultAzureCredential());
            }
            else
            {
                string? tenantId = Environment.GetEnvironmentVariable("KEYVAULT_TENANTID");
                string? clientId = Environment.GetEnvironmentVariable("KEYVAULT_CLIENTID");
                string? clientSecret = Environment.GetEnvironmentVariable("KEYVAULT_CLIENTSECRET");
                string? vaultBaseUrl = Environment.GetEnvironmentVariable("KEYVAULT_BASEURL");

                if (string.IsNullOrEmpty(tenantId) ||
                    string.IsNullOrEmpty(clientId) ||
                    string.IsNullOrEmpty(clientSecret) ||
                    string.IsNullOrEmpty(vaultBaseUrl))
                {
                    throw new InvalidOperationException("Key Vault configuration not found.");
                }

                ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

                config.AddAzureKeyVault(new Uri(vaultBaseUrl), credential);
            }
        });
    }
}
