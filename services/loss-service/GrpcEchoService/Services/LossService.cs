using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServices.Services
{
    public class LossService : Loss.LossBase
    {
        private readonly ILogger<LossService> _logger;
        public LossService(ILogger<LossService> logger)
        {
            _logger = logger;
        }

        public override Task<CountIntegerResponse> CountInteger(CountIntegerRequest request, ServerCallContext context)
        {
            var response = new CountIntegerResponse();
            for (int i = 0; i < request.Count; i++)
            {
                response.Numbers.Add(1);
            }

            return Task.FromResult(response);
        }

        public override Task<GetEventResultResponse> GetEventResult(GetResultRequest request, ServerCallContext context)
        {
            var random = new Random();

            var response = new GetEventResultResponse
            {
                AnalysisSid = request.AnalysisSid,
            };

            for (int i = 0; i < request.AnalysisSid; i++)
            {
                response.Result.Add(new EventResult
                {
                    PerilSetCode = 4,
                    YearId = random.Next(request.AnalysisSid),
                    ModelCode = random.Next(255),
                    EventId = random.Next(),
                    Gross = random.NextDouble(),
                    GroundUp = random.NextDouble(),
                    Retained = random.NextDouble(),
                    PreCatNet = random.NextDouble()
                });
            }

            return Task.FromResult(response);
        }

        public override Task<LossResult> RunLoss(LossRequest lossRequest, ServerCallContext context)
        {

            var rand = new Random();
            int i = 1;
            var geoPt = lossRequest.Locations[0].Lat;
            //var response = new LossResult { Temp = (int)(geoPt ?? default), Package = lossRequest.Package};
            var summaries =  new Google.Protobuf.Collections.RepeatedField<LossResult.Types.AnnualSummary>();
            var response = new LossResult
            {
                Request = lossRequest
            };

            foreach (var loc in lossRequest.Locations)
            {
                response.AnnualSummaries.Add(new LossResult.Types.AnnualSummary
                {
                    MeanAggregate = (rand.NextDouble() + lossRequest.Locations.Count) * 1000,
                    StdDevAggregate = rand.NextDouble() * lossRequest.Locations.Count

                });
            }

            response.AnnualDetails.Add(
                new LossResult.Types.AnnualDetail
                {
                    ReturnPeriod = 50,
                    AggregateLoss = 10000 * rand.NextDouble() * lossRequest.Locations.Count,
                    AggregateYear = rand.Next(1, 10000)
                });

            response.AnnualDetails.Add(
                new LossResult.Types.AnnualDetail
                {
                    ReturnPeriod = 100,
                    AggregateLoss = 10000 * rand.NextDouble() * lossRequest.Locations.Count,
                    AggregateYear = rand.Next(1, 10000)
                });

            Console.WriteLine($"geoPt = {geoPt}, pkg = { lossRequest.Package}");
            Console.WriteLine($"request = {lossRequest}");
            return Task.FromResult(response);
        }
    }
}
