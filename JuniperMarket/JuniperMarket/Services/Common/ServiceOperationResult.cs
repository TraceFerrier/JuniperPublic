using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace JuniperMarket.Services
{
    public class ServiceOperationResult<T>
    {
        public ServiceOperationResult()
        {
            Status = new ServiceOperationStatus();
            List = new List<T>();
        }

        public ServiceOperationResult(T persistable) : this()
        {
            Persistable = persistable;
        }

        public ServiceOperationResult(List<T> ts): this()
        {
            List = ts.ToList();
        }

        public ServiceOperationResult(ServiceResultCode serviceResultCode) : this()
        {
            Status.ResultCode = serviceResultCode;
        }

        public ServiceOperationResult(HttpStatusCode httpStatusCode, string message) : this()
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    Status.ResultCode = ServiceResultCode.Ok;
                    break;
                case HttpStatusCode.NotFound:
                    Status.ResultCode = ServiceResultCode.FailedObjectNotFound;
                    break;
                case HttpStatusCode.Unauthorized:
                    Status.ResultCode = ServiceResultCode.FailedNotAuthorized;
                    break;
                case HttpStatusCode.BadRequest:
                    Status.ResultCode = ServiceResultCode.FailedInvalidArgs;
                    break;
                default:
                    Status.ResultCode = ServiceResultCode.FailedInvalidHttpOperation;
                    break;
            }
            Status.HttpStatusCode = httpStatusCode;
            Status.Message = message;
        }

        public ServiceOperationResult(Exception e) : this()
        {
            Status.ResultCode = ServiceResultCode.FailedOperationException;
            Status.Message = e.Message;
        }

        public ServiceOperationStatus Status { get; set; }

        public T Persistable { get; set; }

        public List<T> List { get; set; }
    }
}
