using System.Reflection;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Application;
using Template.Domain.CommonModule;
using Template.Infra.Data.EF.Context;
using Template.Infra.Data.EF.Repositories;
using Template.Infra.Data.EF.UnitOfWork;
using Module = Autofac.Module;

namespace Template.Api.DependencyResolution
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var appServices = Assembly.GetAssembly(typeof(BaseAppService<>));
            var repositoriesInterfaces = Assembly.GetAssembly(typeof(AddRepository<>));
            var repositoriesImplementations = Assembly.GetAssembly(typeof(GenericRepository<>));

            builder.RegisterType<UnitOfWork>()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();

                var opt = new DbContextOptionsBuilder<ExampleContext>();
                opt.UseSqlServer(config.GetConnectionString("Example_Database"));

                return new ExampleContext(opt.Options);
            }).AsSelf().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(appServices, repositoriesInterfaces, repositoriesImplementations)
                   .Where(t => t.Name.EndsWith("AppService") || t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
