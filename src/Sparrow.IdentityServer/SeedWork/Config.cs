using IdentityServer4.Models;
using System.Collections.Generic;

namespace Sparrow.IdentityServer.SeedWork
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() 
        {
            return new List<IdentityResource> 
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() ,
            };
        }
    }
}
