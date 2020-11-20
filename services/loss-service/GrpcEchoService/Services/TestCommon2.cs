using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GrpcServices.Services;
using static GrpcServices.Loss;
using System.Diagnostics;

namespace CommonTest2.Services
{ 

    public class Test2Service : Test2.Test2Base
    {
        private readonly ILogger<Test2Service> _logger;
        public Test2Service(ILogger<Test2Service> logger)
        {
            _logger = logger;
        }

        public override Task<AIR.CommonRes> RunTest2(AIR.CommonReq req1, ServerCallContext context)
        {

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler1 = new HttpClientHandler();

            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler1.ServerCertificateCustomValidationCallback
                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var httpClient1 = new HttpClient(httpHandler1);
            var sw = Stopwatch.StartNew();
            var endpoint1 = $"http://kong:9080";
            //var channel1 = GrpcChannel.ForAddress(endpoint1, new GrpcChannelOptions { HttpClient = httpClient1, Credentials = ChannelCredentials.Insecure });
            var channel1 = GrpcChannel.ForAddress(endpoint1, new GrpcChannelOptions { HttpClient = httpClient1, Credentials = Grpc.Core.ChannelCredentials.Insecure });

            var client1 = new LossClient(channel1);


            var response = client1.RunLoss(new GrpcServices.LossRequest() { Package = "Pkg" + req1.Fieldone });
            sw.Stop();
            Console.WriteLine($"Total time: through kong: {sw.ElapsedMilliseconds}");

            sw = Stopwatch.StartNew();
            endpoint1 = $"http://loss:80";
            //var channel1 = GrpcChannel.ForAddress(endpoint1, new GrpcChannelOptions { HttpClient = httpClient1, Credentials = ChannelCredentials.Insecure });
            channel1 = GrpcChannel.ForAddress(endpoint1, new GrpcChannelOptions { HttpClient = httpClient1, Credentials = Grpc.Core.ChannelCredentials.Insecure });

            client1 = new LossClient(channel1);


            response = client1.RunLoss(new GrpcServices.LossRequest() { Package = "Pkg" + req1.Fieldone });
            sw.Stop();
            Console.WriteLine($"Total time: through service directly: {sw.ElapsedMilliseconds}");

            return Task.FromResult(new AIR.CommonRes() { Resone = response.Package}); 
        }
    }
}
