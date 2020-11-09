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
    using LossResultSet = LossResult.Types.ResultSet;
    using AnnualDetailResultRow =
        LossResult
        .Types.ResultSet
        .Types.AnnualDetail
        .Types.ResultRow;
    using Uncertainty =
        LossResult
        .Types.ResultSet
        .Types.AnnualDetail
        .Types.ResultRow
        .Types.Uncertainty;


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

            Console.WriteLine($"request = {lossRequest}");
            var response = new LossResult()
            {
                Package = lossRequest.Package,
                ResultSet = new LossResultSet()
            };

            try
            {
                response.ResultSet.AnnualDetails.Add(new LossResultSet.Types.AnnualDetail());

                response.ResultSet.AnnualDetails[0].ResultRows.Add(
                    new AnnualDetailResultRow
                    {
                        ReturnPeriod = 50,
                        AggregateLoss = 10000 * rand.NextDouble(),
                        AggregateYear = rand.Next(1, 10000),
                        Uncertainty = new Uncertainty
                        {
                            Type = Uncertainty.Types.Type.Aggregate,
                            Percentiles =
                                {
                            new Uncertainty.Types.Percentile
                            {
                                Percentile_ = 90,
                                Loss = rand.NextDouble() * 2000
                            },
                            new Uncertainty.Types.Percentile
                            {
                                Percentile_ = 95,
                                Loss = rand.NextDouble() * 2000
                            },
                                }
                        }
                    });
                switch (lossRequest.LocationsOrContractCase)
                {
                    case LossRequest.LocationsOrContractOneofCase.Contract:
                        response.Contract = lossRequest.Contract.Clone();
                        break;
                    case LossRequest.LocationsOrContractOneofCase.LocationSet:
                        try
                        {
                            response.LocationSet = new LocationSet();
                            response.LocationSet.Locations.AddRange(lossRequest.LocationSet.Locations);
                            for (int i = 0; i < lossRequest.LocationSet.Locations.Count; i++)
                            {

                                response.ResultSet.AnnualSummaries.Add(
                                    new LossResultSet.Types.AnnualSummary
                                    {
                                        Id = lossRequest.LocationSet.Locations[i]?.Id ?? 0,
                                        MeanAggregate = rand.NextDouble() * 1000,
                                        StdDevAggregate = rand.NextDouble() * 100,
                                    });

                                if (lossRequest.LocationSet.Locations[i]?.LocationTerms[0]?.Perils != null)
                                {
                                    response.ResultSet.AnnualSummaries[i]?.Perils.AddRange(lossRequest.LocationSet.Locations[i].LocationTerms[0].Perils);

                                }

                            }
                            response.ResultSet.AnnualDetails[0]?.Perils.AddRange(lossRequest.LocationSet.Locations[0]?.LocationTerms[0]?.Perils);
                            response.Perils.AddRange(lossRequest.LocationSet.Locations[0]?.LocationTerms[0]?.Perils);
                        }
                        catch
                        {
                            //whoops
                        }
                        break;

                    case LossRequest.LocationsOrContractOneofCase.None:
                    default:
                        response.Errors.Add("Location or Contract data was either absent or invalid");
                        response.ResultSet = null;
                        break;
                }

            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
                response.ResultSet = null;
            }

            return Task.FromResult(response);
        }
    }
}
