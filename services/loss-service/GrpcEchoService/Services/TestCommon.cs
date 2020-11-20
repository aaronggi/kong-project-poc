using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using System.Net.Http;
using Grpc.Net.Client;

namespace CommonTest1.Services
{    
    public class Test1Service : Test1.Test1Base
    {
        private readonly ILogger<Test1Service> _logger;
        public Test1Service(ILogger<Test1Service> logger)
        {
            _logger = logger;
        }

        public override Task<AIR.CommonRes> RunTest1(AIR.CommonReq req1, ServerCallContext context)
        {
            var response = new AIR.CommonRes(){
                Resone = "RUNTEST1" + req1.Fieldone,
                Restwo = req1.Fieldtwo.ToString()
            };
            return Task.FromResult(response);
        }
    }
}
