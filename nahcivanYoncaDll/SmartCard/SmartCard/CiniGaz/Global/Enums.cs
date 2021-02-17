using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.CiniGaz.Global
{
    public static class Enums
    {
        public enum CardType
        {
            CONTACT = 0,
            CONTACTLESS
        }

        public enum ResultSC
        {
            BASARILI = 0,
            BASARISIZ,
            KART_OKUYUCU_BULUNAMADI,
            KART_TAKILI_DEGIL,
            ISSUER_HATA,
            CIHAZNO_HATA,
            CAP_HATA,
            SIFRE_HATA,
            BLOCK_HATA,
            PARAMETRE_HATA,
            OKUMA_HATASI,
            YAZMA_HATASI,
            ABONE_KARTI_DEGIL,
            YETKI_KARTI_DEGIL,
            URETIM_KARTI_DEGIL,
            BOS_KART_DEGIL,
            NETWORK_HATA,
            LOG_HATA
        }

        public enum ProcessType
        {
            ABONE_YAP,
            KREDI_YAZ
        }
    }
}
