using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using System.Linq;
using IdentityResourceDTO = IdentityServer4.Models.IdentityResource;

namespace Sparrow.IdentityServer.Core.Mappers
{
    //public class IdentityResourceProfile : Profile
    //{
    //    public IdentityResourceProfile()
    //    {
    //        CreateMap<IdentityResourceDTO, IdentityResource>()
    //            .ForMember(dst => dst.Id, exp => exp.Ignore())
    //            .ForMember(dst => dst.Created, exp => exp.Ignore())
    //            .ForMember(dst => dst.Updated, exp => exp.Ignore())
    //            .ForMember(dst => dst.NonEditable, exp => exp.Ignore())

    //            .ForMember(
    //                dst => dst.UserClaims,
    //                exp => exp.MapFrom(
    //                    src => src.UserClaims.Select(
    //                        claim => new IdentityClaim
    //                        {
    //                            Type = claim
    //                        })
    //                    ))
    //            .ForMember(
    //                dst => dst.Properties,
    //                exp => exp.MapFrom(
    //                    src => src.Properties.Select(
    //                        prop => new IdentityResourceProperty
    //                        {
    //                             Key = prop.Key,
    //                             Value = prop.Value
    //                        })
    //                    ));
    //    }
    //}

    //public static class IdentityResourceMapper
    //{
    //    public static IdentityResource ToEntity(this IdentityResourceDTO resource, IMapper mapper)
    //    {
    //        return mapper.Map<IdentityResource>(resource);
    //    }
    //}
}
