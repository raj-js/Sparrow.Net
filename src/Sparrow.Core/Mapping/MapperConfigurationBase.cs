using AutoMapper;

namespace Sparrow.Core.Mapping
{
    public abstract class MapperConfigurationBase
    {
        public abstract void Config(IMapperConfigurationExpression cfg);
    }

    public abstract class MapperConfigurationBase<TEntity, TCreateReqDto, TUpdateReqDto, TRespDto> :
        MapperConfigurationBase,
        IMapperConfiguration<TEntity, TCreateReqDto, TUpdateReqDto, TRespDto>
    {
        public override void Config(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TCreateReqDto, TEntity>();
            cfg.CreateMap<TUpdateReqDto, TEntity>();
            cfg.CreateMap<TEntity, TRespDto>();
        }
    }
}
