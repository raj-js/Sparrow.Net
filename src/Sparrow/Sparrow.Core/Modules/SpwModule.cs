using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sparrow.Core.Collections.Extensions;
using Sparrow.Core.Dependency;

namespace Sparrow.Core.Modules
{
    public abstract class SpwModule
    {
        protected internal IIocManager IocManager { get; internal set; }

        public virtual void PreInitialize()
        {

        }

        public virtual void Initialize()
        {

        }

        public virtual void PostInitialize()
        {

        }

        public virtual void Shutdown()
        {

        }

        public virtual Assembly[] GetAdditionalAssemblies()
        {
            return new Assembly[0];
        }

        public static bool IsSpwModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(SpwModule).IsAssignableFrom((Type) type);
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsSpwModule(moduleType))
            {
                throw new Exception("This type is not an Sparrow module: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.GetTypeInfo().IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetTypeInfo()
                    .GetCustomAttributes(typeof(DependsOnAttribute), true)
                    .Cast<DependsOnAttribute>();

                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }

            return list;
        }

        public static List<Type> FindDependedModuleTypesRecursivelyIncludingGivenModule(Type moduleType)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesRecursively(list, moduleType);
            list.AddIfNotContains(typeof(SpwKernelModule));
            return list;
        }

        private static void AddModuleAndDependenciesRecursively(List<Type> modules, Type module)
        {
            if (!IsSpwModule(module))
            {
                throw new Exception("This type is not an Spw module: " + module.AssemblyQualifiedName);
            }

            if (modules.Contains(module))
            {
                return;
            }

            modules.Add(module);

            var dependedModules = FindDependedModuleTypes(module);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesRecursively(modules, dependedModule);
            }
        }
    }
}
