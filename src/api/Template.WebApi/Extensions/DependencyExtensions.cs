using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Template.Application;
using Template.Domain;
using Template.Infra.Data.EF.Context;
using Template.Infra.Data.EF.UnitOfWork;

namespace Template.WebApi.Extensions
{
    public static class DependencyExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            var interfaces = new List<Type>();
            var implementations = new List<Type>();

            foreach (var assembly in GetAssemblies())
            {
                interfaces.AddRange(assembly.ExportedTypes.Where(p => p.IsInterface && (p.FullName.EndsWith("Repository") || p.FullName.EndsWith("Service"))));
                implementations.AddRange(assembly.ExportedTypes.Where(p => !p.IsInterface && (p.FullName.EndsWith("Repository") || p.FullName.EndsWith("Service"))));
            }

            foreach (var @interface in interfaces)
            {
                var implementation = implementations.FirstOrDefault(p => @interface.IsAssignableFrom(p) && $"I{p.Name}" == @interface.Name);

                if (implementation == null) { continue; }

                services.AddScoped(@interface, implementation);
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDatabaseContext, ApiContext>();
            services.AddHttpContextAccessor();
        }

        private static Assembly[] GetAssemblies()
        {
            var applicationAssembly = Assembly.GetAssembly(typeof(IUnitOfWork));
            var domainAssembly = Assembly.GetAssembly(typeof(Entity));
            var dataAssembly = Assembly.GetAssembly(typeof(UnitOfWork));

            return new[] { applicationAssembly, domainAssembly, dataAssembly };
        }
    }
}
