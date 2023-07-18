using System.Security.Claims;

namespace User_Contact_Management_System.Utils
{
    public class UserUtils
    {
        public string? GetCurrentUser(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var userId = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }

            return null;
        }
    }
}
