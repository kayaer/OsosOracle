using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Business.DependencyResolvers.Ninject
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            //Service Modül Bindings
            //Bind<IYorumService>().ToConstant(WcfProxy<IYorumService>.CreateChannel()).InSingletonScope();
            //Bind<IDilService>().ToConstant(WcfProxy<IDilService>.CreateChannel()).InSingletonScope();
            //Bind<IDuyuruService>().ToConstant(WcfProxy<IDuyuruService>.CreateChannel()).InSingletonScope();
            //Bind<INesneDegerService>().ToConstant(WcfProxy<INesneDegerService>.CreateChannel()).InSingletonScope();
            //Bind<INesneTipService>().ToConstant(WcfProxy<INesneTipService>.CreateChannel()).InSingletonScope();
            //Bind<IUlkeService>().ToConstant(WcfProxy<IUlkeService>.CreateChannel()).InSingletonScope();
            //Bind<IBirimGrupService>().ToConstant(WcfProxy<IBirimGrupService>.CreateChannel()).InSingletonScope();
            //Bind<IBirimGrupBirimService>().ToConstant(WcfProxy<IBirimGrupBirimService>.CreateChannel()).InSingletonScope();
            //Bind<IBirimYeniService>().ToConstant(WcfProxy<IBirimYeniService>.CreateChannel()).InSingletonScope();
        }
    }
}
