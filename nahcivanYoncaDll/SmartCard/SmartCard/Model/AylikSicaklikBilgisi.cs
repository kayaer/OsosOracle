using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Model
{
    public struct AylikSicaklikBilgisi
    {
        public List<string> Dergerler;

        public AylikSicaklikBilgisi(byte[] OkunanDegerler)
        {

            string[] _aylar = new string[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };

            Dergerler = new List<string>();

            string tmp = string.Empty;

            //i => okunan değer boyutu
            //x => tekmi çifmi ayrımı yapmak için
            //a => aylar
            //Sayacçta 24 saat sicaklık değeri geliyor bu değerlerin ik hanesi min ikinci hanesi ise max değerleri içerir
            //Sayaçtan FF(255) ilk takıldığında gelebilir yada yuksek bir değer gelebilir buda hatalı değer kabul edilecek
            for (int i = 0, x = 1, a = 0; i < OkunanDegerler.Length; i++, x++)
            {
                if (i <= 23)
                {
                    if ((OkunanDegerler[i] >= 0 && OkunanDegerler[i] <= 50)) // data bozuktur veya FF sayaç yeni takılmıştır
                    {
                        if (x % 2 == 1)
                        {
                            tmp = _aylar[a] + "-" + OkunanDegerler[i];//min
                            a++;
                        }
                        else
                        {
                            tmp += "-" + OkunanDegerler[i];//max
                        }

                        if (x == 2)
                        {
                            Dergerler.Add(tmp);
                            x = 0;
                        }

                        if (a == 12)
                            a = 0;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
