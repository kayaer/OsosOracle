using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Business.DependencyResolvers.Ninject
{
    public class ResolveModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Load(new BusinessModule());
            //var soaSetting = ConfigurationManager.AppSettings["SOA"];

            //var soa = !string.IsNullOrEmpty(soaSetting) && soaSetting.ToBoolean();

            //if (soa)
            //{
            //    Kernel.Load(new ServiceModule());
            //}
            //else
            //{
            //    Kernel.Load(new BusinessModule());
            //}
        }
    }
}
