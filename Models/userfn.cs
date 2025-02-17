namespace ASPNETMaker2023.Models;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

// Partial class
public partial class defa
{
    /// <summary>
    /// Global events
    /// </summary>

    // ContentType Mapping event
    public static void ContentTypeMapping(IDictionary<string, string> mappings)
    {
        // Example:
        //mappings[".image"] = "image/png"; // Add new mappings
        //mappings[".rtf"] = "application/x-msdownload"; // Replace an existing mapping
        //mappings.Remove(".mp4"); // Remove MP4 videos
    }

    // Class Init event
    public static void ClassInit()
    {
        // Enter your code here
    }

    // Page Loading event
    public static void PageLoading()
    {
        // Enter your code here
    }

    // Page Rendering event
    public static void PageRendering()
    {
        //Log("Page Rendering");
    }

    // Page Unloaded event
    public static void PageUnloaded()
    {
        // Enter your code here
    }

    // Personal Data Downloading event
    public static void PersonalDataDownloading(Dictionary<string, object> row)
    {
        //Log("PersonalData Downloading");
    }

    // Personal Data Deleted event
    public static void PersonalDataDeleted(Dictionary<string, object> row)
    {
        //Log("PersonalData Deleted");
    }

    // AuditTrail Inserting event
    public static bool AuditTrailInserting(Dictionary<string, object> rsnew)
    {
        return true;
    }

    // Chart Rendered event
    public static void ChartRendered(ChartJsRenderer renderer)
    {
        // Example:
        //var data = renderer.Data;
        //var options = renderer.Options;
        //DbChart chart = renderer.Chart;
        //if (chart.ID == "<Report>_<Chart>") { // Check chart ID
        //}
    }

    // One Time Password Sending event
    public static bool OtpSending(string user, dynamic client)
    {
        // Example:
        // Log(user, client); // View user and client (Email or Sms object)
        // if (SameText(Config.TwoFactorAuthenticationType, "email")) { // Possible values: "email" or "sms"
        //     client.Content = ...; // Change content
        //     client.Recipient = ...; // Change recipient
        //     // return false; // Return false to cancel
        // }
        return true;
    }

    // Routes Add event
    public static void RouteAction(IEndpointRouteBuilder app)
    {
        // Example:
        // app.MapGet("/", () => "Hello World!");
    }

    // Services Add event
    public static void ServiceAdd(IServiceCollection services)
    {
        // Example:
        // services.AddSignalR();
    }

    // Container Build event
    public static void ContainerBuild(ContainerBuilder builder)
    {
        // Enter your code here
    }

    // App Build event
    public static void AppBuild(WebApplicationBuilder builder)
    {
        // Example:
        // builder.Configuration.AddAzureKeyVault(...);
    }
    // Global user code
    public class IpPortResult
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool IsReachable { get; set; }
    }

    // Example of adding IpPortChecker static class here
    public static class IpPortChecker
    {
        public static async Task<bool> CheckIpPortAsync(string ip, int port)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var connectTask = tcpClient.ConnectAsync(ip, port);
                    var delayTask = Task.Delay(5000); // Timeout after 5 seconds

                    var completedTask = await Task.WhenAny(connectTask, delayTask);

                    if (completedTask == connectTask)
                    {
                        // Connection successful
                        return true;
                    }
                    else
                    {
                        // Connection timed out
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                // Connection failed
                return false;
            }
        }

        public static async Task<Dictionary<(string Ip, int Port), bool>> CheckMultipleIpPortsAsync(List<(string Ip, int Port)> ipPortList)
        {
            var results = new Dictionary<(string, int), bool>();

            foreach (var (ip, port) in ipPortList)
            {
                bool isReachable = await CheckIpPortAsync(ip, port);
                results.Add((ip, port), isReachable);
            }

            return results;
        }
    }
}