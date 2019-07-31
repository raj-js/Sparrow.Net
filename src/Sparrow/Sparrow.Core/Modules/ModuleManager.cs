using Sparrow.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sparrow.Core.Modules
{
    public class ModuleManager : IModuleManager
    {
        public ModuleInfo StartupModule { get; private set; }

        public IReadOnlyList<ModuleInfo> Modules => _modules.ToImmutableList();

        private ModuleCollection _modules;

        private readonly IIocManager _iocManager;

        public ModuleManager(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        public virtual void Initialize(Type startupModule)
        {
            _modules = new ModuleCollection(startupModule);
            LoadAllModules();
        }

        public virtual void StartModules()
        {
            var sortedModules = _modules.GetSortedModuleListByDependency();
            sortedModules.ForEach(module => module.Instance.PreInitialize());
            sortedModules.ForEach(module => module.Instance.Initialize());
            sortedModules.ForEach(module => module.Instance.PostInitialize());
        }

        public virtual void ShutdownModules()
        {
            var sortedModules = _modules.GetSortedModuleListByDependency();
            sortedModules.Reverse();
            sortedModules.ForEach(sm => sm.Instance.Shutdown());
        }

        private void LoadAllModules()
        {
            List<Type> plugInModuleTypes;
            var moduleTypes = FindAllModuleTypes(out plugInModuleTypes).Distinct().ToList();

            RegisterModules(moduleTypes);
            CreateModules(moduleTypes, plugInModuleTypes);

            _modules.EnsureKernelModuleToBeFirst();
            _modules.EnsureStartupModuleToBeLast();

            SetDependencies();
        }

        private List<Type> FindAllModuleTypes(out List<Type> plugInModuleTypes)
        {
            plugInModuleTypes = new List<Type>();

            var modules = SpwModule.FindDependedModuleTypesRecursivelyIncludingGivenModule(_modules.StartupModuleType);

            //foreach (var plugInModuleType in _abpPlugInManager.PlugInSources.GetAllModules())
            //{
            //    if (modules.AddIfNotContains(plugInModuleType))
            //    {
            //        plugInModuleTypes.Add(plugInModuleType);
            //    }
            //}

            return modules;
        }

        private void CreateModules(ICollection<Type> moduleTypes, List<Type> plugInModuleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                var moduleObject = _iocManager.Resolve(moduleType) as SpwModule;
                if (moduleObject == null)
                {
                    throw new Exception("This type is not an Spw module: " + moduleType.AssemblyQualifiedName);
                }

                moduleObject.IocManager = _iocManager;

                var moduleInfo = new ModuleInfo(moduleType, moduleObject, plugInModuleTypes.Contains(moduleType));

                _modules.Add(moduleInfo);

                if (moduleType == _modules.StartupModuleType)
                {
                    StartupModule = moduleInfo;
                }
            }
        }

        private void RegisterModules(ICollection<Type> moduleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                _iocManager.RegisterIfNot(moduleType);
            }
        }

        private void SetDependencies()
        {
            foreach (var moduleInfo in _modules)
            {
                moduleInfo.Dependencies.Clear();

                //Set dependencies for defined DependsOnAttribute attribute(s).
                foreach (var dependedModuleType in SpwModule.FindDependedModuleTypes(moduleInfo.Type))
                {
                    var dependedModuleInfo = _modules.FirstOrDefault(m => m.Type == dependedModuleType);
                    if (dependedModuleInfo == null)
                    {
                        throw new Exception("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + moduleInfo.Type.AssemblyQualifiedName);
                    }

                    if ((moduleInfo.Dependencies.FirstOrDefault(dm => dm.Type == dependedModuleType) == null))
                    {
                        moduleInfo.Dependencies.Add(dependedModuleInfo);
                    }
                }
            }
        }
    }
}
