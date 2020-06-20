using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public class Error
    {
        private int _Kod;
        private string _Aciklama;

        public int Kod
        {
            get { return _Kod; }
            set { _Kod = value; }
        }

        public string Aciklama
        {
            get { return _Aciklama; }
            set { _Aciklama = value; }
        }

        public Error()
        {
            _Kod = 0;
            _Aciklama = "Başarılı";
        }

        public void Set(int Kod)
        {
            _Kod = Kod;
            switch (Kod)
            {
                case -1:
                    _Aciklama = "Başarısız";
                    break;
                case 1:
                    _Aciklama = "Konsantratöre bağlantı hatası";
                    break;
                case 2:
                    _Aciklama = "Konsantratör paketi okunamadı";
                    break;
                case 3:
                    _Aciklama = "Konsantratöre login olunamadı";
                    break;
                case 4:
                    _Aciklama = "Paket Okuma Hatası";
                    break;
                case 5:
                    _Aciklama = "Programlama hata";
                    break;
                case 6:
                    _Aciklama = "Konsantratörle bağlantı kesilemedi";
                    break;
                case 7:
                    _Aciklama = "Sayaç Konsantratöre kayıtlı değil";
                    break;
                case 8:
                    _Aciklama = "Yük profili okunamadı";
                    break;
                case 9:
                    _Aciklama = "Bozuk paket";
                    break;
                case 10:
                    _Aciklama = "Belirtilen Ipdeki Konsantrator Imei Numarası ile bağlanılan Konsantratorün Imei Numarası uyumsuz";
                    break;
                case 11:
                    _Aciklama = "Bilinmeyen hata";
                    break;
                case 12:
                    _Aciklama = "Header parse hata";
                    break;
                default:
                    Kod = 0;
                    _Aciklama = "Başarılı";
                    break;
            }
        }
    }
}
