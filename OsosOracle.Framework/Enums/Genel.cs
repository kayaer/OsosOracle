using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Framework.Enums
{
    public class Enums
    {
        // Bu enuma ekleme yaptığınızda AutocompleteDegerFuction enumuna da ekleme yapınız

        /// <summary>
        /// Ototamamları besleyecek servis tipi
        /// </summary>
        public enum AutocompleteFunction
        {
            [Description("/NesneDeger/AjaxAra?nesneTipId=1")]
            KursTuruGetir = 1,


        }


        public enum AutocompleteDegerFuction
        {
            [Description("/NesneDeger/AjaxTekDeger")]
            KursTuruGetir = 1,

        }


        public enum AutocompleteCokDegerFuction
        {
            [Description("/NesneDeger/AjaxCokDeger")]
            KursTuruGetir = 1,
        }

        public enum AutoCompleteType
        {
            /// <summary>
            /// liste şeklinde seçenekleri gösterir
            /// </summary>
            List = 0,

            /// <summary>
            /// ağaç yapısında gösterir
            /// </summary>
            Tree = 1,

            /// <summary>
            /// listeleme butonunu kaldırır
            /// </summary>
            None = 2,

            /// <summary>
            /// seçilecek veririn listeleme sayfasını açar,
            /// BU SEÇENKETE mutlaka viewURL belirtilmelidir
            /// </summary>
            CustomView = 3
        }

    }
    public enum EnumSiralamaTuru
    {
        Asc,
        Desc
    }
}
