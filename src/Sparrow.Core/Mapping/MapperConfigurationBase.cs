using AutoMapper;

namespace Sparrow.Core.Mapping
{
    public abstract class MapperConfigurationBase
    {
        public abstract void Config(IMapperConfigurationExpression cfg);
    }

    public abstract class MapperConfigurationBase<TEntity, TCreateDTO, TUpdateDTO, TDTO> :
        MapperConfigurationBase,
        IMapperConfiguration<TEntity, TCreateDTO, TUpdateDTO, TDTO, TDTO>
    {
        public override void Config(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TCreateDTO, TEntity>();
            cfg.CreateMap<TUpdateDTO, TEntity>();
            cfg.CreateMap<TEntity, TDTO>();
        }
    }

    public abstract class MapperConfigurationBase<TEntity, TCreateDTO, TUpdateDTO, TListItemDTO, TDTO> :
        MapperConfigurationBase,
        IMapperConfiguration<TEntity, TCreateDTO, TUpdateDTO, TListItemDTO, TDTO>
    {
        public override void Config(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TCreateDTO, TEntity>();
            cfg.CreateMap<TUpdateDTO, TEntity>();
            cfg.CreateMap<TEntity, TListItemDTO>();
            cfg.CreateMap<TEntity, TDTO>();
        }
    }
}
