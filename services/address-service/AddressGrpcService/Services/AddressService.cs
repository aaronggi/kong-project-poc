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

    public class AddressService : Address.AddressBase
    {
        private readonly ILogger<AddressService> _logger;
        public AddressService(ILogger<AddressService> logger)
        {
            _logger = logger;
        }

        public override Task<GeoPoint> GetAddress(AIRAddress addressRequest, ServerCallContext context)
        {
            

            Console.WriteLine($"request = {addressRequest}");
            var response = new GeoPoint()
            {
                Lat = 11.11,
                Lon = 22.22
            };

            return Task.FromResult(response);
        }
    }
}
