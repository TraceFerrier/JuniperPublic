using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JuniperMarket.Services
{
    public class ServiceOperationStatus
    {
        public ServiceOperationStatus()
        {
            ResultCode = ServiceResultCode.Ok;
        }

        public ServiceResultCode ResultCode { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public string Message { get; set; }

        public bool Succeeded
        {
            get
            {
                return ResultCode == ServiceResultCode.Ok;
            }
        }

    }
}
