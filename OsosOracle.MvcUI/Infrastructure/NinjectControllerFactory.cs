using Ninject;
using OsosOracle.Business.DependencyResolvers.Ninject;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OsosOracle.MvcUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
  
            if (controllerType == null)
            {
                throw new HttpException(404, "Kaynak Bulunamadı");
                // return null;
            }
            var kernel = new StandardKernel(new ResolveModule());

            return (IController)kernel.Get(controllerType);

        }
    }
}