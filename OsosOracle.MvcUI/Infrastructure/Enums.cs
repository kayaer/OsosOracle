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
            [Description("/PRMTARIFEKALORIMETRE/AjaxAra")]
            KalorimetreTarifeGetir = 12,
            [Description("/CONDIL/AjaxAra")]
            DilGetir = 13,
            [Description("/NESNETIP/AjaxAra")]
            NESNETIPGetir = 14,
            [Description("/Sayac/YesilVadiAjaxAra")]
            YesilVadiSayacGetir = 15,
            [Description("/NESNEDEGER/AjaxAra?nesneTipId=1")]
            SayacTuruGetir = 16,
            [Description("/NESNEDEGER/AjaxAra?nesneTipId=2")]
            SatisTipiGetir = 17,
            [Description("/NESNEDEGER/AjaxAra?nesneTipId=3")]
            DurumGetir = 18,
            [Description("/PRMTARIFEGaz/AjaxAra")]
            GazTarifeGetir = 19,
            [Description("/NESNEDEGER/AjaxAra?nesneTipId=4")]
            OdemeTipiGetir = 20,
            [Description("/NESNEDEGER/AjaxAra?nesneTipId=7")]
            ZamanlanmisGorevPeriyotGetir = 21,
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
            [Description("/PRMTARIFEKALORIMETRE/AjaxTekDeger")]
            KalorimetreTarifeGetir = 12,
            [Description("/CONDIL/AjaxTekDeger")]
            DilGetir = 13,
            [Description("/NESNETIP/AjaxTekDeger")]
            NESNETIPGetir = 14,
            [Description("/Sayac/YesilVadiAjaxTekDeger")]
            YesilVadiSayacGetir = 15,
            [Description("/NESNEDEGER/AjaxTekDeger")]
            SayacTuruGetir = 16,
            [Description("/NESNEDEGER/AjaxTekDeger")]
            SatisTipiGetir = 17,
            [Description("/NESNEDEGER/AjaxTekDeger")]
            DurumGetir = 18,
            [Description("/PRMTARIFEGaz/AjaxTekDeger")]
            GazTarifeGetir = 19,
            [Description("/NESNEDEGER/AjaxTekDeger")]
            OdemeTipiGetir = 20,
            [Description("/NESNEDEGER/AjaxTekDeger")]
            ZamanlanmisGorevPeriyotGetir = 21,
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
            [Description("/PRMTARIFEKALORIMETRE/AjaxCokDeger")]
            KalorimetreTarifeGetir = 12,
            [Description("/CONDIL/AjaxCokDeger")]
            DilGetir = 13,
            [Description("/NESNETIP/AjaxCokDeger")]
            NESNETIPGetir = 14,
            [Description("/Sayac/AjaxCokDeger")]
            YesilVadiSayacGetir = 15,
            [Description("/NESNEDEGER/AjaxTekDeger")]
            SayacTuruGetir = 16,
            [Description("/NESNETIP/AjaxCokDeger")]
            SatisTipiGetir = 17,
            [Description("/NESNEDEGER/AjaxCokDeger")]
            DurumGetir = 18,
            [Description("/PRMTARIFEGaz/AjaxCokDeger")]
            GazTarifeGetir = 19,
            [Description("/NESNEDEGER/AjaxCokDeger")]
            OdemeTipiGetir = 20,
            [Description("/NESNEDEGER/AjaxCokDeger")]
            ZamanlanmisGorevPeriyotGetir = 21,
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