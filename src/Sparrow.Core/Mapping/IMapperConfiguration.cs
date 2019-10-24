using AutoMapper;

namespace Sparrow.Core.Mapping
{
    public interface IMapperConfiguration<TEntity, TCreateDTO, TUpdateDTO, TDTO>
    {
        void Config(IMapperConfigurationExpression cfg);
    }
}
