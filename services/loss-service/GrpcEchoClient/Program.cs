using Grpc.Net.Client;
using GrpcServices;
using Microsoft.Extensions.Configuration;
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
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, SslPolicyErrors) => true;

            if (args[0].ToLower() == "g")
                TestGrpc(Convert.ToInt32(args[1]));
            else
                await TestRestful(Convert.ToInt32(args[1]));
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
    }
}
