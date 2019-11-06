using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using System.Linq;
using ClientDTO = IdentityServer4.Models.Client;

namespace Sparrow.IdentityServer.Core.Mappers
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientDTO, Client>()
                .ForMember(dst => dst.Id, exp => exp.Ignore())
                .ForMember(dst => dst.Created, exp => exp.Ignore())
                .ForMember(dst => dst.Updated, exp => exp.Ignore())
                .ForMember(dst => dst.LastAccessed, exp => exp.Ignore())
                .ForMember(dst => dst.NonEditable, exp => exp.Ignore())

                .ForMember(
                    dst => dst.IdentityProviderRestrictions,
                    exp => exp.MapFrom(
                        src => src.IdentityProviderRestrictions.Select(
                            idp => new ClientIdPRestriction
                            {
                                Provider = idp
                            })
                        )
                    )
                .ForMember(
                    dst => dst.AllowedCorsOrigins,
                    exp => exp.MapFrom(
                        src => src.AllowedCorsOrigins.Select(
                            origin => new ClientCorsOrigin
                            {
                                Origin = origin
                            })
                        )
                    )
                .ForMember(
                    dst => dst.Properties,
                    exp => exp.MapFrom(
                        src => src.Properties.Select(
                            prop => new ClientProperty
                            {
                                Key = prop.Key,
                                Value = prop.Value
                            })
                        )
                    )
                .ForMember(
                    dst => dst.Claims,
                    exp => exp.MapFrom(
                        src => src.Claims.Select(
                            claim => new ClientClaim
                            {
                                Type = claim.Type,
                                Value = claim.Value
                            })
                        )
                    )
                .ForMember(
                    dst => dst.AllowedScopes,
                    exp => exp.MapFrom(
                        src => src.AllowedScopes.Select(
                            scope => new ClientScope
                            {
                                Scope = scope
                            })
                        )
                    )
                .ForMember(
                    dst => dst.ClientSecrets,
                    exp => exp.MapFrom(
                        src => src.ClientSecrets.Select(
                            secret => new ClientSecret
                            {
                                Type = secret.Type,
                                Value = secret.Value,
                                Expiration = secret.Expiration,
                                Description = secret.Description
                            })
                        )
                    )
                .ForMember(
                    dst => dst.AllowedGrantTypes,
                    exp => exp.MapFrom(
                        src => src.AllowedGrantTypes.Select(
                            grantType => new ClientGrantType
                            {
                                GrantType = grantType
                            })
                        )
                    )
                .ForMember(
                    dst => dst.RedirectUris,
                    exp => exp.MapFrom(
                        src => src.RedirectUris.Select(
                            redirectUri => new ClientRedirectUri
                            {
                                RedirectUri = redirectUri
                            })
                        )
                    )
                .ForMember(
                    dst => dst.PostLogoutRedirectUris,
                    exp => exp.MapFrom(
                        src => src.PostLogoutRedirectUris.Select(
                            postLogoutRedirectUri => new ClientPostLogoutRedirectUri
                            {
                                 PostLogoutRedirectUri = postLogoutRedirectUri
                            })
                        )
                    );
        }
    }

    public static class ClientMapper
    {
        public static Client ToEntity(this ClientDTO client, IMapper mapper)
        {
            return mapper.Map<Client>(client);
        }
    }
}
