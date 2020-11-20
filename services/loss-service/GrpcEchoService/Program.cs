using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GrpcServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
#if RELEASELOSS
                        options.Listen(IPAddress.Any, 80, ListenOptions => ListenOptions.Protocols = HttpProtocols.Http2);
#elif RELEASE2
                        options.Listen(IPAddress.Any, 5006, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
#elif RELEASE
                        options.Listen(IPAddress.Any, 5005, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
#endif
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
