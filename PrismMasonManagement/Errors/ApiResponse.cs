using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrismMasonManagement.Api.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request to PrismMasonManagement",
                401 => "Unauthorized Request to PrismMasonManagement",
                404 => "Resource not found in PrismMasonManagement API",
                500 => "Internal Server Error in PrismMasonManagement API",
                _ => null
            };
        }
    }
}
