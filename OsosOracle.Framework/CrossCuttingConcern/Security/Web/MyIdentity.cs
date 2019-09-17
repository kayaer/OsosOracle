using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.CrossCuttingConcern.Security.Web
{
    [Serializable]
    public class UserIdentity : IIdentity
    {
        /// <summary>
        /// kullanıcı adı
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// aktif profil id
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// işlem yapılan ip adresi
        /// </summary>
        public string UserIp { get; set; }

        /// <summary>
        /// işlem yapılan adres
        /// </summary>
        public string RequestUrl { get; set; }



        /// <summary>
        /// IIdentity özellikleri
        /// </summary>
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// TC Kimlik Numarası
        /// </summary>
        public long? TckNo { get; set; }

        /// <summary>
        /// Ek taşınacak bilgiler 
        /// [Serializeable] olmalıdır
        /// </summary>
        public object EkBilgi { get; set; }
    }
}
