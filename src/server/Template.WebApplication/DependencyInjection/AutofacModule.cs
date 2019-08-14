using System.Reflection;
using Autofac;
using Template.Application;
using Template.Domain.CommonModule;
using Module = Autofac.Module;

namespace Template.WebApplication.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var appservices = Assembly.GetAssembly(typeof(BaseAppService<>));
            var repositories = Assembly.GetAssembly(typeof(TRepository<>));

            builder.RegisterAssemblyTypes(appservices, repositories)
                   .Where(t => t.Name.EndsWith("AppService") || t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
