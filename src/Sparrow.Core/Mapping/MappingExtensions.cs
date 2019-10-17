using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Sparrow.Core.Mapping
{
    public static class MappingExtensions
    {
        public static void AppliyMapperConfigurations(this IMapperConfigurationExpression cfg, params Assembly[] assemblies)
        {
            cfg.RegisterMappingFromAssembly(typeof(MapperConfigurationBase<,,,>).Assembly);

            foreach (var assembly in assemblies)
            {
                cfg.RegisterMappingFromAssembly(assembly);
            }
        }

        private static void RegisterMappingFromAssembly(this IMapperConfigurationExpression cfg, Assembly assembly)
        {
            var configurations = assembly.GetTypes()
                        .Where(s => !s.IsAbstract && s.IsSubclassOf(typeof(MapperConfigurationBase)))
                        .ToList();

            configurations.ForEach(s =>
                        {
                            var target = s.GetConstructor(new Type[] { }).Invoke(new object[] { });
                            var args = new object[] { cfg };

                            s.GetMethod(nameof(MapperConfigurationBase.Config)).Invoke(target, args);
                        });
        }
    }
}
