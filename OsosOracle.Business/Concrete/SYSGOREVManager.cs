using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.SYSGOREVComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Entities.ComplexType.SYSGOREVROLComplexTypes;
using System.Linq;
using OsosOracle.Framework.Aspects.TransactionAspects;

namespace OsosOracle.Business.Concrete
{
    public class SYSGOREVManager : BaseManager, ISYSGOREVService
    {
        private readonly ISYSGOREVDal _sYSGOREVDal;
        public SYSGOREVManager(ISYSGOREVDal sYSGOREVDal)
        {
            _sYSGOREVDal = sYSGOREVDal;
        }

        public SYSGOREV GetirById(int id)
        {
            return _sYSGOREVDal.Getir(id);
        }

        public SYSGOREVDetay DetayGetirById(int id)
        {
            return _sYSGOREVDal.DetayGetir(id);
        }

        public List<SYSGOREV> Getir(SYSGOREVAra filtre)
        {
            return _sYSGOREVDal.Getir(filtre);
        }

        public int KayitSayisiGetir(SYSGOREVAra filtre)
        {
            return _sYSGOREVDal.KayitSayisiGetir(filtre);
        }

        public List<SYSGOREVDetay> DetayGetir(SYSGOREVAra filtre)
        {
            return _sYSGOREVDal.DetayGetir(filtre);
        }

        public SYSGOREVDataTable Ara(SYSGOREVAra filtre = null)
        {
            return _sYSGOREVDal.Ara(filtre);
        }


        [FluentValidationAspect(typeof(SYSGOREVValidator))]
        private void Validate(SYSGOREV entity)
        {
            //Kontroller Yapılacak
        }

        public void Ekle(List<SYSGOREV> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            _sYSGOREVDal.Ekle(entityler.ConvertEfList<SYSGOREV, SYSGOREVEf>());
        }

        public void Guncelle(List<SYSGOREV> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            _sYSGOREVDal.Guncelle(entityler.ConvertEfList<SYSGOREV, SYSGOREVEf>());
        }

        public void Sil(List<int> idler)
        {
            _sYSGOREVDal.Sil(idler);
        }

        public bool RolGorevSil(int rolid, int gorevid)
        {
            var gorevrolDal = ServisGetir<ISYSGOREVROLDal>();
            SYSGOREVROL gorevrol = gorevrolDal.Getir(new SYSGOREVROLAra { ROLKAYITNO = rolid, GOREVKAYITNO = gorevid }).FirstOrDefault();
            if (gorevrol != null)
            {
                gorevrolDal.Sil(gorevrol.KAYITNO.List());
                return true;
            }
            return false;

        }

        [TransactionScopeAspect]
        public void GorevOperasyonEkle(SYSGOREV entity, List<int> operasyonList,int olusturan)
        {
            Validate(entity);
            if (entity.KAYITNO > 0)
            {
                _sYSGOREVDal.Guncelle(entity.ConvertEfList<SYSGOREV, SYSGOREVEf>());
            }
            else
            {
                entity = _sYSGOREVDal.Ekle(entity.ConvertEfList<SYSGOREV, SYSGOREVEf>()).FirstOrDefault();
            }
           
            var operasyonService = ServisGetir<ISYSOPERASYONGOREVService>();
            operasyonService.OperasyonGorevSil(entity.KAYITNO);

            foreach (var operasyon in operasyonList)
            {
                operasyonService.Ekle(new SYSOPERASYONGOREV() { GOREVKAYITNO = entity.KAYITNO, OPERASYONKAYITNO = operasyon, OLUSTURAN = olusturan }.List());

            }
        }
    }
}