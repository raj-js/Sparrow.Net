using AutoMapper;

namespace Sparrow.Core.Mapping
{
    public interface IMapperConfiguration<TEntity, TCreateReqDto, TUpdateReqDto, TRespDto>
    {
        void Config(IMapperConfigurationExpression cfg);
    }
}
