using AgroTrace.Aplication.Interfaces;
using System.Security.Claims;

namespace AgroTrace.Aplication.Service
{
    public class UserAudiServices: IUserAudi
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAudiServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Name)?
                .Value ?? "Sistema";
        }



    }
}
