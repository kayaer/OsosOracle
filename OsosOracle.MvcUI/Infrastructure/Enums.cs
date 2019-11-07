using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OsosOracle.MvcUI.Infrastructure
{
    public class Enums
    {
        // Bu enuma ekleme yaptığınızda AutocompleteDegerFuction enumuna da ekleme yapınız

        /// <summary>
        /// Ototamamları besleyecek servis tipi
        /// </summary>
        public enum AutocompleteFuction
        {
            [Description("/SayacModel/AjaxAra")]
            SayacModelGetir = 1,
            [Description("/Sayac/AjaxAra")]
            SayacGetir = 2,
            [Description("/Abone/AjaxAra")]
            AboneGetir = 3,
            [Description("/Tarife/SuAjaxAra")]
            SuTarifeGetir = 4,
            [Description("/PRMTARIFEELK/AjaxAra")]
            ElektrikTarifeGetir = 5,
            [Description("/SYSMENU/AjaxAra")]
            MenuGetir = 6,
            [Description("/Kurum/AjaxAra")]
            KurumGetir = 7,
            [Description("/SYSKULLANICI/AjaxAra")]
            KullaniciGetir = 8,
            [Description("/SYSROL/AjaxAra")]
            RolGetir = 9,
            [Description("/CSTHUMARKA/AjaxAra")]
            ModemMarkaGetir = 10,
            [Description("/CSTHUMODEL/AjaxAra")]
            ModemModelGetir = 11,
            [Description("/PRMTARIFEORTAKAVM/AjaxAra")]
            KalorimetreTarifeGetir = 12,

        }

        public enum AutocompleteDegerFuction
        {
            [Description("/SayacModel/AjaxTekDeger")]
            SayacModelGetir = 1,
            [Description("/Sayac/AjaxTekDeger")]
            SayacGetir = 2,
            [Description("/Abone/AjaxTekDeger")]
            AboneGetir = 3,
            [Description("/Tarife/SuAjaxTekDeger")]
            SuTarifeGetir = 4,
            [Description("/PRMTARIFEELK/AjaxTekDeger")]
            ElektrikTarifeGetir = 5,
            [Description("/SYSMENU/AjaxTekDeger")]
            MenuGetir = 6,
            [Description("/Kurum/AjaxTekDeger")]
            KurumGetir = 7,
            [Description("/SYSKULLANICI/AjaxTekDeger")]
            KullaniciGetir = 8,
            [Description("/SYSROL/AjaxTekDeger")]
            RolGetir = 9,
            [Description("/CSTHUMARKA/AjaxTekDeger")]
            ModemMarkaGetir = 10,
            [Description("/CSTHUMODEL/AjaxTekDeger")]
            ModemModelGetir = 11,
            [Description("/PRMTARIFEORTAKAVM/AjaxTekDeger")]
            KalorimetreTarifeGetir = 12,

        }

        public enum AutocompleteCokDegerFuction
        {
            [Description("/SayacModel/AjaxCokDeger")]
            SayacModelGetir = 1,
            [Description("/Sayac/AjaxCokDeger")]
            SayacGetir = 2,
            [Description("/Abone/AjaxCokDeger")]
            AboneGetir = 3,
            [Description("/Tarife/SuAjaxCokDeger")]
            SuTarifeGetir = 4,
            [Description("/PRMTARIFEELK/AjaxCokDeger")]
            ElektrikTarifeGetir = 5,
            [Description("/SYSMENU/AjaxCokDeger")]
            MenuGetir = 6,
            [Description("/Kurum/AjaxCokDeger")]
            KurumGetir = 7,
            [Description("/SYSKULLANICI/AjaxCokDeger")]
            KullaniciGetir = 8,
            [Description("/SYSROL/AjaxCokDeger")]
            RolGetir = 9,
            [Description("/CSTHUMARKA/AjaxCokDeger")]
            ModemMarkaGetir = 10,
            [Description("/CSTHUMODEL/AjaxCokDeger")]
            ModemModelGetir = 11,
            [Description("/PRMTARIFEORTAKAVM/AjaxCokDeger")]
            KalorimetreTarifeGetir = 12,

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
}