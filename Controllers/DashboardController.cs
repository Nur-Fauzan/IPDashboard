using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETMaker2023.Controllers
{
    // Partial class
    public partial class HomeController : Controller
    {
        // Dashboard (custom)
        [Route("Dashboard/{**key}", Name = "Dashboard-Dashboard-custom")]
        [Route("Home/Dashboard/{**key}", Name = "Dashboard-Dashboard-custom-2")]
        public async Task<IActionResult> Dashboard()
        {
            // Create page object
            dashboard = new GLOBALS.Dashboard(this);
            var ipPortCheckResult = await IpPortCheck();

            // Run the page
            return await dashboard.Run();
        }

        public async Task<IActionResult> IpPortCheck()
        {
            var ipPortList = Configuration.GetSection("IPs").Get<List<IpPortConfig>>();

            var results = new List<IpPortResult>();

            foreach (var ipPortConfig in ipPortList)
            {
                try
                {
                    var isReachable = await IsIpPortReachable(ipPortConfig.Ip, ipPortConfig.Port);
                    results.Add(new IpPortResult { Ip = ipPortConfig.Ip, Port = ipPortConfig.Port, IsReachable = isReachable });
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error checking IP {ipPortConfig.Ip} port {ipPortConfig.Port}: {ex.Message}");
                    // Optionally handle or log the error
                    // Example: results.Add(new IpPortResult { Ip = ip, Port = port, IsReachable = false });
                }
            }

            return View("Dashboard", results); // Ensure you are passing 'results' as the model
        }
        public class IpPortConfig
        {
            public string Ip { get; set; }
            public int Port { get; set; }
        }

        private async Task<bool> IsIpPortReachable(string ip, int port)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var connectTask = tcpClient.ConnectAsync(ip, port);
                    var timeoutTask = Task.Delay(5000); // Timeout after 5 seconds
                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        return false;
                    }

                    return tcpClient.Connected;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
