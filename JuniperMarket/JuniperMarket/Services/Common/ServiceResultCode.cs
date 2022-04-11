using System;
using System.Collections.Generic;
using System.Text;

namespace JuniperMarket.Services
{
    public enum ServiceResultCode
    {
        Ok,
        Failed,
        FailedNotAuthorized,
        FailedObjectNotFound,
        FailedInvalidArgs,
        FailedOperationTimeout,
        FailedOperationException,
        FailedInvalidHttpOperation
    }
}
