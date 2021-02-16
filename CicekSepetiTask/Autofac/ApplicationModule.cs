using Autofac;
using CicekSepetiTask.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module = Autofac.Module;

namespace CicekSepetiTask.Autofac
{
    public class ApplicationModule
        : Module
    {
        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        public string QueriesConnectionString { get; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShoppingCartService>()
                .As<IShoppingCartService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();
           
        }
    }

}
