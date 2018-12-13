using System;
using System.Reflection;
using Autodesk.Forge;
using System.Threading.Tasks;
using da_dotnetcore_lib;

// Using https://www.areilly.com/2017/04/21/command-line-argument-parsing-in-net-core-with-microsoft-extensions-commandlineutils/
using Microsoft.Extensions.CommandLineUtils;

namespace da_console
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientId = null, clientSecret = null;

            var app = new CommandLineApplication();
            app.Name = "Design Automation API CLI";
            app.Description = "Command Line Interface for Design Automation API";
            app.HelpOption("-?|-h|--help");

            var clientIdOption = app.Option("-ci|--clientid <ForgeClientID>", 
                "Forge Client ID",
                CommandOptionType.SingleValue);

            var clientSecretOption = app.Option("-cs|--clientsecret <ForgeClientSecret>", 
                "Forge Client Secret",
                CommandOptionType.SingleValue);

            app.VersionOption("-v|--version", () => {
                return string.Format("Version {0}", Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);
            });

            app.Command("listactivities", (command) => {
                command.Description = "This is the description for listactivities.";
                command.HelpOption("-?|-h|--help");
            
                command.OnExecute(async () => {
                    string accessToken = await da.GetAccessToken(clientId, clientSecret);
                    await da.ListActivities(accessToken);
                    
                    return 0;
                });
            });

            app.Command("listappbundles", (command) => {
                command.Description = "This is the description for listappbundles.";
                command.HelpOption("-?|-h|--help");
            
                command.OnExecute(async () => {
                    string accessToken = await da.GetAccessToken(clientId, clientSecret);
                    await da.ListAppBundles(accessToken);
                    return 0;
                });
            });

            app.OnExecute(() => {
                Console.WriteLine("No command was selected");

                return 0;
            });

            try {
                clientId = Environment.GetEnvironmentVariable("FORGE_CLIENT_ID");
                clientSecret = Environment.GetEnvironmentVariable("FORGE_CLIENT_SECRET");
            } catch { }

            if (clientIdOption.HasValue()) 
                clientId = clientIdOption.Value();

            if (clientSecretOption.HasValue()) 
                clientSecret = clientSecretOption.Value();

            if (clientId == null || clientSecret == null) {
                Console.WriteLine("FORGE_CLIENT_ID and/or FORGE_CLIENT_SECRET not defined either as environment variables or command options -ci/-cs.");
                return;
            }

            app.Execute(args);    
        }
    }
}
