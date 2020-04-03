using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSAYACComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Aspects.TransactionAspects;
using System.Linq;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;

namespace OsosOracle.Business.Concrete
{
    public class ENTSAYACManager : BaseManager, IENTSAYACService
    {
        private readonly IENTSAYACDal _eNTSAYACDal;
        public ENTSAYACManager(IENTSAYACDal eNTSAYACDal)
        {
            _eNTSAYACDal = eNTSAYACDal;
        }

        public ENTSAYAC GetirById(int id)
        {
            return _eNTSAYACDal.Getir(id);
        }

        public ENTSAYACDetay DetayGetirById(int id)
        {
            return _eNTSAYACDal.DetayGetir(id);
        }

        public List<ENTSAYAC> Getir(ENTSAYACAra filtre)
        {
            return _eNTSAYACDal.Getir(filtre);
        }

        public int KayitSayisiGetir(ENTSAYACAra filtre)
        {
            return _eNTSAYACDal.KayitSayisiGetir(filtre);
        }

        public List<ENTSAYACDetay> DetayGetir(ENTSAYACAra filtre)
        {
            return _eNTSAYACDal.DetayGetir(filtre);
        }

        public ENTSAYACDataTable Ara(ENTSAYACAra filtre = null)
        {
            return _eNTSAYACDal.Ara(filtre);
        }


        [FluentValidationAspect(typeof(ENTSAYACValidator))]
        private void Validate(ENTSAYAC entity)
        {
            var sayac = _eNTSAYACDal.Getir(new ENTSAYACAra { SayacModelKayitNo = entity.SayacModelKayitNo, SERINO = entity.SERINO, KURUMKAYITNO = entity.KURUMKAYITNO,DURUM=1,KapakSeriNo=entity.KapakSeriNo }).FirstOrDefault();
            if (sayac != null)
            {
                throw new NotificationException("Sayaç modeli ve seri nosu aynı olan başka bir sayaç kayıtlıdır");
            }
        }

        public void Ekle(List<ENTSAYAC> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            _eNTSAYACDal.Ekle(entityler.ConvertEfList<ENTSAYAC, ENTSAYACEf>());
        }

        public void Guncelle(List<ENTSAYAC> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            _eNTSAYACDal.Guncelle(entityler.ConvertEfList<ENTSAYAC, ENTSAYACEf>());
        }

        [TransactionScopeAspect]
        public void Sil(List<int> idler)
        {
            //var abonesayacService = ServisGetir<IENTABONESAYACService>();
            //var aboneSayac=abonesayacService.Getir(new Entities.ComplexType.ENTABONESAYACComplexTypes.ENTABONESAYACAra { SAYACKAYITNO = idler[0] }).FirstOrDefault();
            //if (aboneSayac != null)
            //{
            //    abonesayacService.Sil(aboneSayac.KAYITNO.List());
            //}
            _eNTSAYACDal.Sil(idler);
        }

    }
}