using System.ComponentModel.DataAnnotations;

namespace PrismMasonManagement.Application.Contracts.DTOs.Requests
{
    public class TokenRequestDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}