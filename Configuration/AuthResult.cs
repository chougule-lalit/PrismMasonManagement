using System.Collections.Generic;

namespace PrismMasonManagement.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }

        public bool IsSuccess { get; set; }

        public List<string> Errors { get; set; }
    }
}