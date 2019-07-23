using Microsoft.AspNetCore.Identity;

namespace Sparrow.Security.Models
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }
}
