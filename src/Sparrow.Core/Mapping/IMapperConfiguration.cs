using AutoMapper;

namespace Sparrow.Core.Mapping
{
    public interface IMapperConfiguration<TEntity, TCreateDTO, TUpdateDTO, TDTO>
        : IMapperConfiguration<TEntity, TCreateDTO, TUpdateDTO, TDTO, TDTO>
    { 
    
    }

    public interface IMapperConfiguration<TEntity, TCreateDTO, TUpdateDTO, TListItemDTO, TDTO>
    {
        void Config(IMapperConfigurationExpression cfg);
    }
}
