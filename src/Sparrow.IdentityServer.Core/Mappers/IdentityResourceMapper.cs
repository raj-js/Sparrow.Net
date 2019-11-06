using IdentityServer4.EntityFramework.Entities;
using System.Linq;

namespace Sparrow.IdentityServer.Core.Mappers
{
    public static class IdentityResourceMapper
    {
        public static IdentityResource ToEntity(this IdentityServer4.Models.IdentityResource resource)
        {
            return new IdentityResource
            {
                Enabled = resource.Enabled,
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Required = resource.Required,
                Emphasize = resource.Emphasize,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                UserClaims = resource.UserClaims.Select(claim => new IdentityClaim 
                {
                    Type = claim 
                }).ToList(),
                Properties = resource.Properties.Select(prop => new IdentityResourceProperty
                {
                    Key = prop.Key,
                    Value = prop.Value
                }).ToList()
            };
        }
    }
}
