using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var response = new AIR.CommonRes(){
                Resone = req1.Fieldone,
                Restwo = req1.Fieldtwo.ToString()
            };
            return Task.FromResult(response);
        }
    }
}
