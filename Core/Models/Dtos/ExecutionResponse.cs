using System;

namespace BlinkCash.Core.Dtos
{
    public class ExecutionResponse<T> where T : class
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public T Data { get; set; } 
    }
 

    public enum ResponseCode
    {
        Ok = 0,
        ValidationError = 1,
        NotFound = 2,
        ServerException = 3,
        AuthorizationFailed = 4,
        RetryTransaction = 5,
        BadRequest = 6,
        AlreadyExist = 7,
        BankNotSupported = 8
    }
}
