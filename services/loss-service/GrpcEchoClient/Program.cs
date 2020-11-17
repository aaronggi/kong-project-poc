using Grpc.Net.Client;
using GrpcServices;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace GrpcEchoClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /* 
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, SslPolicyErrors) => true;

            if (args[0].ToLower() == "g")
                TestGrpc(Convert.ToInt32(args[1]));
            else
                await TestRestful(Convert.ToInt32(args[1]));*/
            TestGrpcCommon();
        }

        private static async Task TestRestful(int port)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);

            client.BaseAddress = new Uri($"https://localhost:{port}");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            HttpResponseMessage response = await client.GetAsync("loss/1/result/event");
            response.EnsureSuccessStatusCode();
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
        }

        private static void TestGrpc(int port)
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpHandler);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var endpoint = $"https://localhost:{port}";
            var channel = GrpcChannel.ForAddress(endpoint, new GrpcChannelOptions { HttpClient = httpClient });
            var client = new Loss.LossClient(channel);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = client.GetEventResult(new GetResultRequest
            {
                AnalysisSid = 1
            });
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
        }

        private static bool TestGrpcCommon()
        {
            var rv = false;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);




            var httpHandler1 = new HttpClientHandler();
            var httpHandler2 = new HttpClientHandler();

            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler1.ServerCertificateCustomValidationCallback
                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler2.ServerCertificateCustomValidationCallback
                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var httpClient1 = new HttpClient(httpHandler1);
            var httpClient2 = new HttpClient(httpHandler2);


            var request = new AIR.CommonReq { Fieldone = "hello", Fieldtwo = 21 };

            var endpoint1 = $"http://localhost:{5005}";
            var endpoint2 = $"http://localhost:{5006}";

            var channel1 = GrpcChannel.ForAddress(endpoint1, new GrpcChannelOptions { HttpClient = httpClient1, Credentials = Grpc.Core.ChannelCredentials.Insecure });
            var channel2 = GrpcChannel.ForAddress(endpoint2, new GrpcChannelOptions { HttpClient = httpClient2, Credentials = Grpc.Core.ChannelCredentials.Insecure });

            var client1 = new CommonTest1.Test1.Test1Client(channel1);
            var client2 = new CommonTest2.Test2.Test2Client(channel2);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var result1 = client1.RunTest1(request);
                var result2 = client2.RunTest2(request);

                if (result1.Equals(result2))
                {
                    Console.WriteLine($"Success! {result1}");
                    rv = true;
                }

                result1 = client1.RunTest1(request);
                result2 = client2.RunTest2(new AIR.CommonReq { Fieldone = "goodbye", Fieldtwo = 0  });

                if (!result1.Equals(result2))
                {
                    Console.WriteLine($"Success! {result1}");
                    rv = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure" + e.Message);
            }

            stopwatch.Stop();

            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}");
            return rv;
        }
    }
}
