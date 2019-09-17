using OsosOracle.Framework.CrossCuttingConcern.Security.Web;
using System.Threading;

namespace OsosOracle.Framework.Infrastructure
{
    /// <summary>
    /// Aktif kullanıcı bilgisini getrir
    /// Login olmamış kullanıcı id -1 gelir
    /// Thread.CurrentPrincipal.Identity den veri gelir 
    /// </summary>
    public class CurrenUser
    {
        public static UserIdentity Identity => (UserIdentity)Thread.CurrentPrincipal.Identity;
    }
}
