using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBase.ExtensionFunctions
{
    public static class ConsulExtension
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient consulClient = new ConsulClient(
                p =>
                {
                    p.Address = new Uri("http://127.0.0.1:8500");

                    //p.Address = new Uri(configuration["ip"]);

                    p.Datacenter = "dc1";
                });

            string ip = "127.0.0.1";

            int port = 5000;

            //int port = int.Parse(configuration["port"]);

            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service" + Guid.NewGuid(),
                Name = "GroupName组名称",
                Address = ip,
                Port = port,
                Tags = { },
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),
                    HTTP = $"http://{ip}:{port}/WeatherForecast/Health",
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5)
                }
            });

        }

    }
}
