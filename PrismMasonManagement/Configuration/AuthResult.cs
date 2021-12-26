using System.Collections.Generic;

namespace PrismMasonManagement.Api.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }
    }
}