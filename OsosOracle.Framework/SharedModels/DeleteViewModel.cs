using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.SharedModels
{
    public class DeleteViewModel
    {
        /// <summary>
        /// Silinecek item id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// silinecek item idler , ile ayrılmış
        /// </summary>
        public string Idler { get; set; }

        /// <summary>
        /// vazgeç dönüş adresi
        /// </summary>
        public string RedirectUrlForCancel { get; set; }

        /// <summary>
        /// gösterilecek mesaj
        /// </summary>
        public string WarningMessage { get; set; }

        /// <summary>
        /// silinecek Guid Id
        /// </summary>
        public Guid GuidId { get; set; }
    }
}
