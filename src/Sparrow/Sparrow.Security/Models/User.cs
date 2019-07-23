using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Sparrow.Security.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
