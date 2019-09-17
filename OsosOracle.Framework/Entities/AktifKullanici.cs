using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OsosOracle.Framework.Entities
{
    public class AktifKullanici
    {

        public int KayitNo { get; set; }

        public string KullaniciAd { get; set; }

        public string Sifre { get; set; }

        public int Versiyon { get; set; }
        public int Olusturan { get; set; }
        public DateTime? OlusturmaTarih { get; set; }
        public byte? Guncelleyen { get; set; }
        public DateTime? GuncellemeTarih { get; set; }

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public bool Durum { get; set; }


        public int Dil { get; set; }
        public int KullaniciTur { get; set; }

        public int KurumKayitNo { get; set; }

        public string KurumAdi { get; set; }

        public string Ip
        {

            get
            {

                if (HttpContext.Current != null) //servis bu metodu çağırıyorsa HttpContext.Current null olabilir
                {
                    //load balancer ip adresi farklı geliyorsa
                    if (HttpContext.Current.Request.Headers["X-Forwarded-For"] != null &&
                        HttpContext.Current.Request.Headers["X-Forwarded-For"] != String.Empty)
                    {
                        return HttpContext.Current.Request.Headers["X-Forwarded-For"];
                    }

                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return Environment.MachineName;
            }



        }

        public static bool GelistiriciMi { get; set; }
    }
}
