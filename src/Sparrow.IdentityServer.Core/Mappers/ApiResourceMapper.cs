using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using System.Linq;
using ApiResourceDTO = IdentityServer4.Models.ApiResource;

namespace Sparrow.IdentityServer.Core.Mappers
{
    public class ApiResourceProfile : Profile
    {
        public ApiResourceProfile()
        {
            CreateMap<ApiResourceDTO, ApiResource>()
                .ForMember(dst => dst.Id, exp => exp.Ignore())
                .ForMember(dst => dst.Created, exp => exp.Ignore())
                .ForMember(dst => dst.Updated, exp => exp.Ignore())
                .ForMember(dst => dst.LastAccessed, exp => exp.Ignore())
                .ForMember(dst => dst.NonEditable, exp => exp.Ignore())

                .ForMember(
                    dst => dst.Secrets,
                    exp => exp.MapFrom(
                        src => src.ApiSecrets.Select(
                            secret => new ApiSecret
                            {
                                Type = secret.Type,
                                Value = secret.Value,
                                Expiration = secret.Expiration,
                                Description = secret.Description
                            })
                    )
                )
                .ForMember(
                    dst => dst.Scopes,
                    exp => exp.MapFrom(
                        src => src.Scopes.Select(
                            scope => new ApiScope
                            {
                                Name = scope.Name,
                                DisplayName = scope.DisplayName,
                                Description = scope.Description,
                                Emphasize = scope.Emphasize,
                                Required = scope.Required,
                                ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument,
                                UserClaims = scope.UserClaims.Select(
                                    claim => new ApiScopeClaim
                                    {
                                        Type = claim
                                    }).ToList()
                            })
                    )
                )
                .ForMember(
                    dst => dst.UserClaims,
                    exp => exp.MapFrom(
                        src => src.UserClaims.Select(
                            claim => new ApiResourceClaim
                            {
                                Type = claim
                            })
                    )
                )
                .ForMember(
                    dst => dst.Properties,
                    exp => exp.MapFrom(
                        src => src.Properties.Select(
                            prop => new ApiResourceProperty
                            {
                                Key = prop.Key,
                                Value = prop.Value
                            })
                    )
                );
        }
    }

    public static class ApiResourceMapper
    {
        public static ApiResource ToEntity(this ApiResourceDTO resource, IMapper mapper)
        {
            return mapper.Map<ApiResource>(resource);
        }
    }
}
