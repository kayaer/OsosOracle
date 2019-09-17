using OsosOracle.Business.DependencyResolvers.Ninject;

namespace OsosOracle.Business.Concrete.Infrastructure
{
    public class BaseManager
    {
        public T ServisGetir<T>()
        {
            return DependencyResolver<T>.Resolve();
        }
    }
}
