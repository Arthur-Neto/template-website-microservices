using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Template.Application;
using Template.Domain;
using Template.Infra.Data.EF.Contexts;
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

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<IEnterpriseProvider, EnterpriseProvider>();
            services.AddScoped<IMasterDbContext, MasterDbContext>();
            services.AddScoped<ITenantDbContext, TenantDbContext>();
            services.AddHttpContextAccessor();
        }

        private static Assembly[] GetAssemblies()
        {
            var applicationAssembly = Assembly.GetAssembly(typeof(AppHandlerBase<>));
            var domainAssembly = Assembly.GetAssembly(typeof(IUnitOfWork<>));
            var dataAssembly = Assembly.GetAssembly(typeof(IDatabaseContext));

            return new[] { applicationAssembly, domainAssembly, dataAssembly };
        }
    }
}
