using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MonitorMyAPI
{
    public class Program
    {
        private static IConfiguration _config;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Get the current hosting environment
                    var env = context.HostingEnvironment;

                    // Build an instance of the config reader
                    _config = config.Build();

                    // The following requires a User-Assigned System Identity (MSI)
                    if (env.IsDevelopment())
                    {
                        // This path results in variables retrieved from secrets.json file.
                        Environment.SetEnvironmentVariable("AzureServicesAuthConnectionString", "RunAs=Developer; DeveloperTool=AzureCli");
                    }
                    else
                    {
                        var clientId = _config["ClientId"];

                        Environment.SetEnvironmentVariable("AzureServicesAuthConnectionString", $"RunAs=App; AppId={clientId}");
                    }

                    var keyVaultName = _config["KeyVaultName"];

                    if (!string.IsNullOrWhiteSpace(keyVaultName))
                    {
                        // Add Microsoft.Extensions.Configuration.AzureKeyVault (latest)
                        // Add Microsoft.Azure.Services.AppAuthentication (latest)
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();

                        var keyVaultClient = new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(
                                azureServiceTokenProvider.KeyVaultTokenCallback));

                        string keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

                        Console.WriteLine($"KeyVault URI: {keyVaultUri}");

                        config.AddAzureKeyVault(keyVaultUri, keyVaultClient,
                            new DefaultKeyVaultSecretManager());
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
