using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismMasonManagement.Infrastructure.PrismMasonManagementDomainService
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Guid Id => GetCurrentUserId();

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private Guid GetCurrentUserId()
        {
            try
            {
                Guid userId = Guid.Empty;
                var data = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault();
                Guid.TryParse(data, out userId);
                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Guid.Empty;
            }
        }
    }
}
