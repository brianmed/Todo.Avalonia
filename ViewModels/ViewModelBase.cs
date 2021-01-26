using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using ReactiveUI;

namespace Todo.DependencyInjection
{
    public class ViewModelBase : ReactiveObject, IDisposable
    {
        public IServiceScope ServiceScope { get; private set; }

        static public T Create<T>() where T : ViewModelBase
        {
            Type vmType = typeof(T);

            IServiceScope serviceScope = Program.Host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            ConstructorInfo vmConstructor = vmType
                .GetConstructors()
                .Single();

            List<object> services = new();

            foreach (ParameterInfo info in vmConstructor.GetParameters())
            {
                services.Add(serviceProvider.GetRequiredService(info.ParameterType));
            }

            T vm = (T)Activator.CreateInstance(vmType, services.ToArray());

            typeof(ViewModelBase)
                .GetProperty(nameof(ServiceScope), BindingFlags.Public | BindingFlags.Instance)
                .GetSetMethod(nonPublic: true)
                .Invoke(vm, new[] { serviceScope });

            return vm;
        }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }

    public class Scoped
    {
        static public void Run<T1>(Action<T1> method)
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                ParameterInfo[] parameterInfos = method.GetMethodInfo().GetParameters();

                List<object> services = Services(serviceScope,parameterInfos);

                method.DynamicInvoke(services.ToArray());
            }
        }

        static public void Run<T1, T2>(Action<T1, T2> method)
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                ParameterInfo[] parameterInfos = method.GetMethodInfo().GetParameters();

                List<object> services = Services(serviceScope,parameterInfos);

                method.DynamicInvoke(services.ToArray());
            }
        }

        static public void Run<T1, T2, T3>(Action<T1, T2, T3> method)
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                ParameterInfo[] parameterInfos = method.GetMethodInfo().GetParameters();

                List<object> services = Services(serviceScope,parameterInfos);

                method.DynamicInvoke(services.ToArray());
            }
        }

        static private List<object> Services(IServiceScope serviceScope, ParameterInfo[] parameterInfos)
        {
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            List<object> services = new();

            foreach (ParameterInfo info in parameterInfos)
            {
                services.Add(serviceProvider.GetRequiredService(info.ParameterType));
            }

            return services;
        }
    }
}
