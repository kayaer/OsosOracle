using OsosOracle.Framework.Utilities.ExtensionMethods;
using System.Linq;

namespace OsosOracle.Framework.DataAccess.Filter
{
    public class FilterHelper<T>
    {
        public int KayitSayisi { get; set; }

        public IQueryable<T> Sayfala(IQueryable<T> result, Ara filtre = null)
        {
            KayitSayisi = result.Count();
            result = Sirala(result, filtre);
            return result;
        }

        public int KayitSayisiGetir(IQueryable<T> result, Ara filtre = null)
        {
            KayitSayisi = result.Count();
            return KayitSayisi;
        }


        public IQueryable<T> Sirala(IQueryable<T> result, Ara filtre = null)
        {
            if (filtre != null)
            {
                if (filtre.Siralama != null)
                {
                    var i = 0;
                    foreach (var siralama in filtre.Siralama)
                    {
                        result = i == 0 ? result.OrderBy(siralama.KolonAdi, siralama.SiralamaTipi) : result.ThenBy(siralama.KolonAdi, siralama.SiralamaTipi);
                        i++;
                    }
                }
                if (filtre.Baslangic != null)
                {
                    result = result.Skip(filtre.Baslangic.Value);
                }

                if (filtre.Uzunluk > 0)
                {
                    result = result.Take(filtre.Uzunluk.Value);
                }
            }


            return result;
        }
    }
}
